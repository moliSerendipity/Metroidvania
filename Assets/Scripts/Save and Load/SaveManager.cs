using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        saveManagers = FindAllSaveManagers();
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No saved data found.");
            NewGame();
        }

        for(int i = 0;i < saveManagers.Count;i++)
        {
            saveManagers[i].LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        for(int i = 0;i < saveManagers.Count;i++)
        {
            saveManagers[i].SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("Game saved.");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        dataHandler.Delete();
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
            return true;
        return false;
    }
}
