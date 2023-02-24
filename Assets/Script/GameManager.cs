using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
  public GameObject scanObject;
  public GameObject menuSet;
  public GameObject player;
  public TalkManager talkManager;
  public QuestManager questManager;
  public Animator talkPanel;  // Script Window
  public TypeEffect talk; // Script Text
  public Image portraitImg; // NPC Expressions
  public Animator portraitAnim;
  public Sprite prevPortrait;
  public Text questTitle;

  public bool isAction; // Determine if a player is interacting
  private int talkIndex = 0;  // Text Order


  private void Start()
  {
    GameLoad();
    questTitle.text = questManager.CheckQuest();
  }

  private void Update()
  {
    // Sub Menu
    if (Input.GetButtonDown("Cancel"))
    {
      if (menuSet.activeSelf)
        menuSet.SetActive(false);
      else
        menuSet.SetActive(true);
    }
  }

  // Player presses the space bar
  public void Action(GameObject scanObj)
  {
    // Check object or NPC name
    scanObject = scanObj;
    ObjectData objectData = scanObject.GetComponent<ObjectData>();

    // Open Script
    Talk(objectData.id, objectData.isNpc);
    talkPanel.SetBool("isShow", isAction);
  }

  private void Talk(int id, bool isNpc)
  {
    int questTalkIndex = 0;
    string talkData = "";

    // Set Talk Data
    // Key is pressed during the dialogue output, the dialogue will be immediately displayed
    if (talk.isTalking)
    {
      talk.SetMsg("");
      return;
    }
    // Key is pressed after all the dialogue has been displayed, the dialogue will either move on to the next one.
    else
    {
      questTalkIndex = questManager.GetQuestTalkIndex(id);
      talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
    }

    // End Talk
    if (talkData == null)
    {
      isAction = false;
      talkIndex = 0;
      questTitle.text = questManager.CheckQuest(id);
      return;
    }
    // Continue Talk
    // NPC
    if (isNpc)
    {
      // Text
      talk.SetMsg(talkData.Split('`')[0]);
      // Expressions
      portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('`')[1]));
      portraitImg.color = new Color(1, 1, 1, 1);
      // Animation
      if (prevPortrait != portraitImg.sprite)
      {
        portraitAnim.SetTrigger("doEffect");
        prevPortrait = portraitImg.sprite;
      }
    }
    // Object
    else
    {
      // Text
      talk.SetMsg(talkData);
      // Hide Potrait
      portraitImg.color = new Color(1, 1, 1, 0);
    }
    isAction = true;
    // Next Script
    talkIndex++;
  }

  public void GameSave()
  {
    PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
    PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
    PlayerPrefs.SetInt("QuestId", questManager.questId);
    PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
    PlayerPrefs.Save();
  }

  public void GameLoad()
  {
    if (!PlayerPrefs.HasKey("PlayerX"))
      return;

    float x = PlayerPrefs.GetFloat("PlayerX", player.transform.position.x);
    float y = PlayerPrefs.GetFloat("PlayerY", player.transform.position.y);
    int questId = PlayerPrefs.GetInt("QuestId", questManager.questId);
    int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex", questManager.questActionIndex);

    player.transform.position = new Vector3(x, y, -1);
    questManager.questId = questId;
    questManager.questActionIndex = questActionIndex;
    questManager.ControlObject();
  }

  public void GameExit()
  {
    Application.Quit();
  }
}
