using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public int day = 1;

    [SerializeField] MonologueTrigger monologueTrigger;

    [SerializeField] GameObject JiHyeMessage;

    public void FirstOpenID()
    {
        if (GameStats.Instance.Stage.Equals(0))
        {
            monologueTrigger.TriggerMonologue("CheckName");
        }
    }
    public void FirstOutID()
    {
        if(GameStats.Instance.Stage.Equals(0))
          StartCoroutine(FirstDayEvent());
    }
    IEnumerator FirstDayEvent()
    {
        yield return new WaitForSeconds(3f);
        {
            monologueTrigger.TriggerMonologue("GetMessage");
            JiHyeMessage.SetActive(true);
        }

        GameStats.Instance.Stage = 1;
    }

    public void OpenFirstJiHye()
    {
        monologueTrigger.TriggerMonologue("FirstDayJiHye");

        GameStats.Instance.Stage = 2;
    }
    public void Day2DeleteJiHye()
    {
        JiHyeMessage.SetActive(false);
    }
}
