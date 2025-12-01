using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Transform player;
    [SerializeField] private Checkpoint[] checkpoints;

    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        Debug.Log(checkpoints.Length);

        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckpointsAndPlaceAtClosestCheckpoint(GameData _data)
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (_data.checkpoints.ContainsKey(checkpoint.id) && _data.checkpoints[checkpoint.id] == true)
            {
                checkpoint.ActivateCheckpoint();
                Debug.Log("Activated checkpoint: " + checkpoint.id);

                if (_data.closestCheckpointID == checkpoint.id)
                    PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector2(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            LostCurrencyController lostCurrencyScript = newLostCurrency.GetComponent<LostCurrencyController>();
            lostCurrencyScript.currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);
        LoadLostCurrency(_data);
        LoadCheckpointsAndPlaceAtClosestCheckpoint(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        _data.closestCheckpointID = FindClosestCheckpoint()?.id;
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        Checkpoint closestCheckpoint = null;
        float closestDistance = Mathf.Infinity;
        Vector2 playerPosition = player.position;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(playerPosition, checkpoint.transform.position);
            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}
