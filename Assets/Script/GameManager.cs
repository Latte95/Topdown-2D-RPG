using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class GameManager : MonoBehaviour, IDataPersistence
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
  public Text objectNameText;

  public bool isAction; // Determine if a player is interacting
  private int talkIndex = 0;  // Text Order


  private void Start()
  {
    DataManager.instance.LoadGame();
    questManager.ControlObject();
    questTitle.text = questManager.CheckQuest();
  }

  #region 플레이어 위치 저장 및 로드
  public void LoadData(PlayerData data)
  {
    player.transform.position = data.playerPosition;
  }
  public void SaveData(ref PlayerData data)
  {
    data.playerPosition = player.transform.position;
  }
  #endregion

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
    Talk(objectData.id, objectData.isNpc, objectData.objectName);
    talkPanel.SetBool("isShow", isAction);
  }

  private void Talk(int id, bool isNpc, string objectName)
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
    // Name
    objectNameText.text = objectName;
    if (isNpc)
      objectNameText.color = new Color(0, 1, 0);
    else
      objectNameText.color = new Color(0, 0, 0);
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

  public void OnSaveGameClicked()
  {
    DataManager.instance.SaveGame();
  }

  public void GameExit()
  {
    Application.Quit();
  }
}
