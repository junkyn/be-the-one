using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public GameObject JiHyeMessage;
    // Start is called before the first frame update
    [SerializeField] MonologueTrigger monologueTrigger;

    public void IDScreenEvent()
    {
        if (GameStats.Stage == 0)
        {
            monologueTrigger.TriggerMonologue("CheckName");
        }
    }
    public void FirstDisableID()
    {
        StartCoroutine(FirstDayEvent());
    }
    IEnumerator FirstDayEvent()
    {
        if (GameStats.Stage == 0)
        {
            GameStats.Stage = 1;
            yield return new WaitForSeconds(3f);
            {
                monologueTrigger.TriggerMonologue("GetMessage");
                JiHyeMessage.SetActive(true);
            }
        }
    }
}
