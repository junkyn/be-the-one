using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public int day = 1;
    public TMP_Dropdown dropdown;

    [Header("Map")]
    public Image MapImage;
    public Sprite hereMap;
    public Sprite hongdaeMap;

    [Header("Internet")]
    public Image Internetimg;
    public Sprite news1;
    public Sprite ipconfig;
    [SerializeField] MonologueTrigger monologueTrigger;
    [SerializeField] GameObject JiHyeMessage;
    [SerializeField] GameObject CrimePic;
    
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
    public void Stage3OpenJiHye()
    {
        monologueTrigger.TriggerMonologue("2_XAns_CheckMe");
        GameStats.Instance.Stage2CheckMessage= true;
    }
    public void Day2DeleteJiHye()
    {
        JiHyeMessage.SetActive(false);
    }

    public void Stage4CheckMessage()
    {
        monologueTrigger.TriggerMonologue("2_Ans_CheckMe");
        GameStats.Instance.Stage2CheckMessage= true;
    }
    public void Stage2CheckGallery()
    {
        monologueTrigger.TriggerMonologue("CheckCrimePi");
        GameStats.Instance.Stage2CheckGallery= true;
    }
    public void Day2Update()
    {
        TMP_Dropdown.OptionData HongDae = new TMP_Dropdown.OptionData();
        HongDae.text = "È«´ëÀÔ±¸¿ª";
        HongDae.image = null;
        dropdown.options.Add(HongDae);
        CrimePic.SetActive(true);
    }
    public void Day3Update()
    {
        if (GameStats.Instance.Stage == 5)
        {
            Internetimg.sprite = news1;
            JiHyeMessage.SetActive(false);
        }
        else if(GameStats.Instance.Stage == 6)
        {
            Internetimg.sprite = ipconfig;
        }
    }
    public void MapChange(TMP_Dropdown option)
    {
        if (option.value.Equals(1))
        {
            if (!GameStats.Instance.Stage2CheckMap)
            {
                monologueTrigger.TriggerMonologue("CheckMapDay2");
                GameStats.Instance.Stage2CheckMap = true;
            }
            MapImage.sprite = hongdaeMap;
        }
        else if(option.value.Equals(0))
        {
            MapImage.sprite = hereMap;
        }
    }
}
