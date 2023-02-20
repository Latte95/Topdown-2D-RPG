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
    talkData.Add(1000, new string[] { "안녕하세요.`3" }); // Female
    talkData.Add(1100, new string[] { "...`0" }); // Male
    // Object
    talkData.Add(100, new string[] { "상자 안에 아무 것도 없다." });  // Box
    talkData.Add(200, new string[] { "알 수 없는 낙서가 그려져있다." });  // Table
    // Quest
    // 1
    talkData.Add(10 + 1000, new string[] { "마을에 오신 걸 환영해요.`2" });
    talkData.Add(11 + 1100, new string[] { "환영하네.`3" });
    // 2
    talkData.Add(20 + 1100, new string[] { "잃어버린 동전 좀 찾아주겠나?`3" });
    talkData.Add(20 + 5000, new string[] { "나무 밑에 있는 동전을 발견했다." });
    talkData.Add(22 + 1100, new string[] { "고맙네.`2" });


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
    if (!talkData.ContainsKey(id)) // Dictionary에 키가 존재하는지 검사
    {
      if (!talkData.ContainsKey(id - id % 10))
        // First Tlak (No Quest)
        return GetTalk(id - id % 100, talkIndex);
      else
        // First Quest Talk
        return GetTalk(id - id % 10, talkIndex);
    }
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
