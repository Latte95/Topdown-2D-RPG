using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
  public float positionX;
  public float positionY;
  public int questId;
  public int questActionIndex;
}

public class DataManager : MonoBehaviour
{
  // Singleton
  public static DataManager instance;

  public PlayerData nowPlayer = new PlayerData();
  public string path;

  void Awake()
  {
    #region 싱글톤
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(instance.gameObject);
    DontDestroyOnLoad(this.gameObject);
    #endregion

    path = Application.persistentDataPath + "/saveData.json";
  }

  public void SaveData()
  {
    string data = JsonUtility.ToJson(nowPlayer);
    File.WriteAllText(path, data);
  }

  public void LoadData()
  {
    string data = File.ReadAllText(path);
    nowPlayer = JsonUtility.FromJson<PlayerData>(data);
  }

  public void DataClear()
  {
    nowPlayer = new PlayerData();
  }
}
