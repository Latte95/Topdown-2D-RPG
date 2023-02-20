using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
  public GameObject talkPanel;  // Script Window
  public GameObject scanObject;
  public TalkManager talkManager;
  public QuestManager questManager;
  public Text talkText; // Script Text
  public Image portraitImg; // NPC Expressions

  public bool isAction; // Determine if a player is interacting
  private int talkIndex = 0;  // Text Order


  private void Start()
  {
    Debug.Log(questManager.CheckQuest());
  }
  private void Awake()
  {
    // Script Window Off
    talkPanel.SetActive(false);
  }

  // Player presses the space bar
  public void Action(GameObject scanObj)
  {
    // Check object or NPC name
    scanObject = scanObj;
    ObjectData objectData = scanObject.GetComponent<ObjectData>();

    // Open Script
    Talk(objectData.id, objectData.isNpc);
    talkPanel.SetActive(isAction);
  }

  private void Talk(int id, bool isNpc)
  {
    // Set Talk Data
    int questTalkIndex = questManager.GetQuestTalkIndex(id);
    string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

    // End Talk
    if (talkData == null)
    {
      isAction = false;
      talkIndex = 0;
      questManager.CheckQuest(id);
      return;
    }
    // Continue Talk
    // NPC
    if (isNpc)
    {
      // Text
      talkText.text = talkData.Split('`')[0];
      // Expressions
      portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('`')[1]));
      portraitImg.color = new Color(1, 1, 1, 1);
    }
    // Object
    else
    {
      // Text
      talkText.text = talkData;
      // Hide Potrait
      portraitImg.color = new Color(1, 1, 1, 0);
    }
    isAction = true;
    // Next Script
    talkIndex++;
  }
}
