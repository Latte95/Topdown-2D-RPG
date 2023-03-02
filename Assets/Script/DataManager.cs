using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


public class DataManager : MonoBehaviour
{
  // Singleton
  public static DataManager instance;

  public PlayerData nowPlayer;
  private List<IDataPersistence> dataPersistenceObjects;

  private string path;
  private string fileName;

  private string key = "latte";
  private static readonly byte[] salt = new byte[] { 0x26, 0x19, 0x36, 0x29, 0x3F, 0x10, 0x01, 0x1A };

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
        using (FileStream stream = new FileStream(fullPath, FileMode.Open))
        {
          using (StreamReader reader = new StreamReader(stream))
          {
            string encryptedData = reader.ReadToEnd();
            string decryptedData = Decrypt(encryptedData);
            loadedData = JsonUtility.FromJson<PlayerData>(decryptedData);
          }
        }
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
      string encryptedData = Encrypt(dataToStore);
      using (StreamWriter writer = new StreamWriter(fullPath, false))
      {
        writer.Write(encryptedData);
      }
    }
    catch (Exception e)
    {
      Debug.LogError("Failed to save data: " + fullPath + "\n" + e.Message);
    }
  }

  private string Encrypt(string data)
  {
    byte[] plainTextBytes = Encoding.UTF8.GetBytes(data);

    using (Aes aes = Aes.Create())
    {
      Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt);
      aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
      aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

      using (MemoryStream ms = new MemoryStream())
      {
        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
          cs.Write(plainTextBytes, 0, plainTextBytes.Length);
          cs.FlushFinalBlock();
        }

        byte[] cipherTextBytes = ms.ToArray();
        return Convert.ToBase64String(cipherTextBytes);
      }
    }
  }

  private string Decrypt(string encryptedData)
  {
    byte[] cipherTextBytes = Convert.FromBase64String(encryptedData);

    using (Aes aes = Aes.Create())
    {
      Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt);
      aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
      aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

      using (MemoryStream ms = new MemoryStream())
      {
        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
        {
          cs.Write(cipherTextBytes, 0, cipherTextBytes.Length);
          cs.FlushFinalBlock();
        }

        byte[] plainTextBytes = ms.ToArray();
        return Encoding.UTF8.GetString(plainTextBytes, 0, plainTextBytes.Length);
      }
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
