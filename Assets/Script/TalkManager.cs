using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
  Dictionary<int, string[]> talkData; // id, data
  Dictionary<int, Sprite> portraitData;
  public Sprite[] portraitArr;
  public TextAlignment npcName;
  public int idLuna = 1000;
  public int idLudo = 2000;
  public int idBox = 100;
  public int idTable = 200;
  public int idQuestCoin = 5000;

  public int npcIdCount = 1000;
  private int npcNum = 2;
  private int portraitNum = 4;  // Angry, Idle, Smile, Talk
  public QuestManager questManager;

  private void Awake()
  {
    talkData = new Dictionary<int, string[]>();
    portraitData = new Dictionary<int, Sprite>();
    questManager = FindObjectOfType<QuestManager>();
    GenerateData();
  }

  private void GenerateData()
  {
    int _questId = questManager.questId;
    // Script Manage
    // NPC
    talkData.Add(idLuna, new string[] { "안녕하세요.`3" }); // Female
    talkData.Add(idLudo, new string[] { "...`0" }); // Male
    // Object
    talkData.Add(idBox, new string[] { "상자 안에 아무 것도 없다." });  // Box
    talkData.Add(idTable, new string[] { "알 수 없는 낙서가 그려져있다." });  // Table
    // Quest
    // 1
    talkData.Add(_questId + idLuna, new string[] { "마을에 오신 걸 환영해요.`2" });
    talkData.Add(1 + _questId + idLudo, new string[] { "환영하네.`3" });
    _questId += _questId;
    // 2
    talkData.Add(_questId + idLudo, new string[] { "잃어버린 동전 좀 찾아주겠나?`3" });
    talkData.Add(_questId + idQuestCoin, new string[] { "나무 밑에 있는 동전을 발견했다." });
    talkData.Add(2 + _questId + idLudo, new string[] { "고맙네.`2" });
    _questId += _questId;


    // Portraits Manage
    for (int npc = 0; npc < npcNum; npc++)
      for (int i = 0; i < portraitNum; i++)
      {
        portraitData.Add(npcIdCount * (npc + 1) + i, portraitArr[i + portraitNum * npc]);
      }
  }

  public string GetTalk(int id, int talkIndex)
  {
    if (!talkData.ContainsKey(id)) // Dictionary에 키가 존재하는지 검사
    {
      if (!talkData.ContainsKey(id - id % questManager.questCount))
        // First Tlak (No Quest)
        id -= id % (questManager.questCount * 10);
      else
        // First Quest Talk
        id -= id % questManager.questCount;
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
