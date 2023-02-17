using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
  public GameObject talkPanel;  // Script Window
  public GameObject scanObject;
  public TalkManager talkManager;
  public Text talkText;
  public Image portraitImg;

  public bool isAction; // Determine if a player is interacting
  public int talkIndex;

  private void Awake()
  {
    talkPanel.SetActive(false);
  }

  public void Action(GameObject scanObj)
  {
    scanObject = scanObj;
    ObjectData objectData = scanObject.GetComponent<ObjectData>();
    Talk(objectData.id, objectData.isNpc);

    talkPanel.SetActive(isAction);
  }

  private void Talk(int id, bool isNpc)
  {
    string talkData = talkManager.GetTalk(id, talkIndex);

    // No more script to be had
    if (talkData == null)
    {
      isAction = false;
      talkIndex = 0;
      return;
    }

    // More scripts exist
    if (isNpc)
    {
      talkText.text = talkData.Split('`')[0];

      portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('`')[1]));
      portraitImg.color = new Color(1,1,1,1);
    }
    else
    {
      talkText.text = talkData;

      portraitImg.color = new Color(1,1,1,0);
    }
    isAction = true;
    talkIndex++;
  }
}
