using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public int day = 1;

    [SerializeField] MonologueTrigger monologueTrigger;

    [SerializeField] GameObject JiHyeMessage;

    public void FirstDisableID()
    {
        if (GameStats.Instance.Stage.Equals(0))
        {
            monologueTrigger.TriggerMonologue("CheckName");
            StartCoroutine(FirstDayEvent());
        }
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
}
