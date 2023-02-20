using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
  string targetMsg;
  float interval;
  public int CPS; // Character Per Seconds

  int index;  // Dialogue Letters Index
  public bool isTalking;  // Is the dialogue still being displayed?

  Text msgText;
  AudioSource audioSource;
  public GameObject EndCursor;

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
    // Initialization
    msgText.text = "";
    index = 0;
    isTalking = true;
    EndCursor.SetActive(false);

    // Set the number of letters to be displayed per second
    interval = 1.0f / CPS;
    Invoke("EffectOn", interval);
  }

  private void EffectOn()
  {
    // targetMsg[index] is not a space or period, play a sound effect.
    if (targetMsg[index] != ' ' && targetMsg[index] != '.')
      audioSource.Play();

    msgText.text += targetMsg[index];
    index++;
    // All letters has been displayed, exit.
    if (msgText.text == targetMsg)
    {
      EffectEnd();
      return;
    }

    Invoke("EffectOn", interval);
  }

  private void EffectEnd()
  {
    isTalking = false;
    EndCursor.SetActive(true);
  }
}
