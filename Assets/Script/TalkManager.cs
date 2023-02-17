using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
  Dictionary<int, string[]> talkData; // id, data
  Dictionary<int, Sprite> portraitData;
  public Sprite[] portraitArr;

  private void Awake()
  {
    talkData = new Dictionary<int, string[]>();
    portraitData = new Dictionary<int, Sprite>();
    GenerateData();
  }

  private void GenerateData()
  {
    // Script Manage
    // NPC
    talkData.Add(1000, new string[] { "안녕하세요.`3", "마을에 오신 걸 환영해요.`2" }); // Female
    talkData.Add(1100, new string[] { "...`0" }); // Male
    // Object
    talkData.Add(100, new string[] { "상자 안에 아무 것도 없다." });  // Box
    talkData.Add(200, new string[] { "알 수 없는 낙서가 그려져있다." });  // Table

    // Expressions Manage
    // Female NPC expressions
    portraitData.Add(1000 + 0, portraitArr[0]); // Angrty
    portraitData.Add(1000 + 1, portraitArr[1]); // Idle
    portraitData.Add(1000 + 2, portraitArr[2]); // Smile
    portraitData.Add(1000 + 3, portraitArr[3]); // Talk
    // Male NPC expressions
    portraitData.Add(1100 + 0, portraitArr[4]);
    portraitData.Add(1100 + 1, portraitArr[5]);
    portraitData.Add(1100 + 2, portraitArr[6]);
    portraitData.Add(1100 + 3, portraitArr[7]);
  }

  public string GetTalk(int id, int talkIndex)
  {
    if (talkIndex == talkData[id].Length)
      return null;
    else
      return talkData[id][talkIndex];
  }

  public Sprite GetPortrait(int id, int portraitIndex)
  {
    return portraitData[id + portraitIndex];
  }
}
