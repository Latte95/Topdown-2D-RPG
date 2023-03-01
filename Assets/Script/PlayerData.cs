using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
  public Vector3 playerPosition;
  public int questId;
  public int questActionIndex;

  public PlayerData()
  {
    this.playerPosition = Vector3.zero;
    this.questId = 10;
    this.questActionIndex = 0;
  }
}
