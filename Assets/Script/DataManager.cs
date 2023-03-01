using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;


public class DataManager : MonoBehaviour
{
  // Singleton
  public static DataManager instance;

  public QuestManager questManager;

  public PlayerData nowPlayer;
  private List<IDataPersistence> dataPersistenceObjects;

  private string path;
  private string fileName;

  private void Awake()
  {
    #region 싱글톤
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(this.gameObject);
    }
    else
    {
      Destroy(instance.gameObject);
      return;
    }
    #endregion
    path = Application.persistentDataPath;
    fileName = "saveData.json";
    dataPersistenceObjects = FindAllDataPersistenceObjects();
  }

  public void NewGame()
  {
    nowPlayer = new PlayerData();
  }

  // Run LoadData function in all scripts within the project to load data.
  public void LoadGame()
  {
    // Read json file
    nowPlayer = Load();

    // No json file -> player data initialize
    if (nowPlayer == null)
    {
      NewGame();
    }

    // Load player data
    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
    {
      dataPersistenceObj.LoadData(nowPlayer);
    }
    questManager.ControlObject();
  }

  // Run SaveData function in all scripts within the project to save data.
  public void SaveGame()
  {
    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
    {
      dataPersistenceObj.SaveData(ref nowPlayer);
    }
    // And save json file
    Save(nowPlayer);
  }

  // Reads data stored in a json file
  public PlayerData Load()
  {
    string fullPath = Path.Combine(path, fileName);
    PlayerData loadedData = null;
    try
    {
      if (File.Exists(fullPath))
      {
        string data = File.ReadAllText(fullPath);
        using (FileStream stream = new FileStream(fullPath, FileMode.Open))
        {
          using (StreamReader reader = new StreamReader(stream))
          {
            data = reader.ReadToEnd();
          }
        }
        loadedData = JsonUtility.FromJson<PlayerData>(data);
      }
    }
    catch (Exception e)
    {
      Debug.LogError("Failed to load data: " + fullPath + "\n" + e.Message);
    }
    return loadedData;
  }

  // Save data as a json file
  public void Save(PlayerData data)
  {
    string fullPath = Path.Combine(path, fileName);
    try
    {
      // Create the directory the file will be written to if it doesn't already exist
      Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

      // Serialize player information in JSON and save to a file
      string dataToStore = JsonUtility.ToJson(data, true);
      using (StreamWriter writer = new StreamWriter(fullPath, false))
      {
        writer.Write(dataToStore);
      }
    }
    catch (Exception e)
    {
      Debug.LogError("Failed to save data: " + fullPath + "\n" + e.Message);
    }
  }

  // Sava data file initialize
  public void ClearData()
  {
    nowPlayer = new PlayerData();
    Save(nowPlayer);
  }

  private List<IDataPersistence> FindAllDataPersistenceObjects()
  {
    IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
        .OfType<IDataPersistence>();

    return new List<IDataPersistence>(dataPersistenceObjects);
  }
}
