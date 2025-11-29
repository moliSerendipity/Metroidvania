using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncrypt = false;
    private readonly string codeWord = "firefly&NHD";

    public FileDataHandler(string _dataDirPath, string _dataFileName,bool _useEncrypt)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        useEncrypt = _useEncrypt;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(_data, true);
            if (useEncrypt)
                dataToStore = EncryptDecrypt(dataToStore);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        try
        {
            if (File.Exists(fullPath))
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string dataToLoad = reader.ReadToEnd();
                        if (useEncrypt)
                            dataToLoad = EncryptDecrypt(dataToLoad);
                        loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No data file found at: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data from file: " + fullPath + "\n" + e);
        }
        return loadedData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error deleting data file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        return modifiedData;
    }
}
