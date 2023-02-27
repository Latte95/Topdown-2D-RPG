using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  public int questId = 10;  // Quest Numbering
  public int questCount = 10;
  public int questActionIndex;  // Quest NPC Order

  public GameObject[] questObject;
  Dictionary<int, QuestData> questList;

  public TalkManager talkManager;

  void Awake()
  {
    questList = new Dictionary<int, QuestData>();
    talkManager = FindObjectOfType<TalkManager>();
    GenerateData();
  }

  void GenerateData()
  {
    questList.Add(questId, new QuestData("마을 사람들과 대화하기", new int[] { talkManager.idLuna, talkManager.idLudo }));
    questList.Add(questId + questCount, new QuestData("동전 찾아주기", new int[] { talkManager.idLudo, talkManager.idQuestCoin, talkManager.idLudo }));
    questList.Add(questId + questCount * 2, new QuestData("퀘스트 올 클리어!", new int[] { 0 }));
  }

  public int GetQuestTalkIndex(int id)
  {
    return questId + questActionIndex;
  }

  public string CheckQuest(int id)
  {
    if (id == questList[questId].npcId[questActionIndex])
      questActionIndex++;

    // Control Quest Object
    ControlObject();
    // Finishes talking with all NPCs, proceed to the next quest.
    if (questActionIndex == questList[questId].npcId.Length)  // 
      NextQuest();

    // Quest Name
    return questList[questId].questName;
  }

  public string CheckQuest()
  {
    // Quest Name
    return questList[questId].questName;
  }

  private void NextQuest()
  {
    questId += questCount;
    questActionIndex = 0;
  }

  public void ControlObject()
  {
    switch (questId)
    {
      case 20:
        if (questActionIndex == 1)
          questObject[0].SetActive(true);
        else
          questObject[0].SetActive(false);
        break;
    }
  }
}
