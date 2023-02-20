using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  public int questId = 10;  // Quest Numbering
  public int questActionIndex;  // Quest NPC Order

  public GameObject[] questObject;
  Dictionary<int, QuestData> questList;

  void Awake()
  {
    questList = new Dictionary<int, QuestData>();
    GenerateData();
  }

  void GenerateData()
  {
    questList.Add(10, new QuestData("마을 사람들과 대화하기", new int[] { 1000, 1100 }));
    questList.Add(20, new QuestData("동전 찾아주기", new int[] { 1100, 5000, 1100 }));
    questList.Add(30, new QuestData("퀘스트 올 클리어!", new int[] { 0 }));
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
    questId += 10;
    questActionIndex = 0;
  }

  private void ControlObject()
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
