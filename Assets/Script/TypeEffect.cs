using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
  string targetMsg;
  public int CPS; // Character Per Seconds
  Text msgText;
  int index;
  public GameObject EndCursor;
  float interval;
  AudioSource audioSource;
  public bool isTalking;

  private void Awake()
  {
    msgText = GetComponent<Text>();
    audioSource = GetComponent<AudioSource>();
  }

  public void SetMsg(string msg)
  {
    if (isTalking)
    {
      msgText.text = targetMsg;
      CancelInvoke();
      EffectEnd();
    }
    else
    {
    targetMsg = msg;
    EffectStart();
    }
  }

  private void EffectStart()
  {
    msgText.text = "";
    index = 0;
    EndCursor.SetActive(false);

    isTalking = true;
    interval = 1.0f / CPS;
    Invoke("EffectOn", interval);
  }

  private void EffectOn()
  {
    if (msgText.text == targetMsg)
    {
      EffectEnd();
      return;
    }

    // Sound
    if (targetMsg[index] != ' ' || targetMsg[index] != '.')
      audioSource.Play();

    msgText.text += targetMsg[index];
    index++;

    Invoke("EffectOn", interval);
  }

  private void EffectEnd()
  {
    isTalking = false;
    EndCursor.SetActive(true);
  }
}
