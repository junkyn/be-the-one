using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.IO;
using JetBrains.Annotations;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    Color transparent = new Color(1, 1, 1, 0);

    [SerializeField] GameObject screenContinue, screenWarning;

    [SerializeField] RectTransform screenCurrent;
    Vector3 screenPos;
    WaitForSeconds appDelay;
    [SerializeField] Image fadeImage;


    [System.Serializable]
    public class SaveData
    {
        public int stage;
        public int day;
    }
    string path;
    public int stageSaved;
    public int daySaved;

    [Header("Story")]
    [SerializeField] StoryManager storyManager;

    [Header("Lock Screen")]
    [SerializeField] GameObject clickBlock;
    [SerializeField] RectTransform screenLocked, screenMain;
    [SerializeField] GameObject LockScreen;
    [SerializeField] Image iconLock;
    [SerializeField] Sprite spriteUnlock, spriteLock;
    [SerializeField] TextMeshProUGUI touchToUnlock;

    [SerializeField] TextMeshProUGUI dateText, dayText;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;
    public float time = 0;
    int currentMinute = 0, currentHour = 2;
    new WaitForSeconds timer;

    [Header("Monologue")]
    [SerializeField] MonologueManager monologueManager;
    [SerializeField] MonologueTrigger monologueTrigger;
    public GameObject Monologue;

    [Header("Gallery")]
    [SerializeField] Image galleryZoomed;
    Vector3 galleryZoomedPos;
    bool galleryBinActivated;
    [SerializeField] RectTransform galleryBinButton;
    [SerializeField] TextMeshProUGUI galleryBinButtonText;
    [SerializeField] GameObject galleryDeleteButton;
    [SerializeField] GameObject GalleryScreen;
    GameObject photoCurrent;
    [SerializeField] GameObject photoPrefab;
    [SerializeField] Transform galleryContent, binContent;

    [Header("Call")]
    [SerializeField] TextMeshProUGUI callDialText;
    [SerializeField] TextMeshProUGUI callIncoming, calling;

    public List<string> numbersEnable;
    [SerializeField] GameObject callScreen;
    [SerializeField] GameObject callPrefab;
    [SerializeField] Transform callContent;
    [SerializeField] ScrollRect callScrollRect;
    [SerializeField] TextMeshProUGUI callReply1, callReply2, callReply3;
    [SerializeField] GameObject callEnd;

    [Header("Message")]
    [SerializeField] ChatTrigger chatTrigger;
    [SerializeField] Chat chatCurrent;
    [SerializeField] GameObject chatPrefab;
    [SerializeField] TextMeshProUGUI chatName;
    [SerializeField] Transform chatContent;
    [SerializeField] ScrollRect chatScrollRect;
    [SerializeField] GameObject MessageScreen;
    [SerializeField] RectTransform chatRectTransform;
    bool replying = false;
    [SerializeField] Button replyButton;
    [SerializeField] TextMeshProUGUI reply1, reply2, reply3;

    [Header("Note")]
    [SerializeField] TextMeshProUGUI noteText;
    [SerializeField] Transform noteButtons;
    [SerializeField] RectTransform noteButtonsRect;
    [SerializeField] TextMeshProUGUI note1, note2, note3;
    [SerializeField] GameObject noteButton1;
    [SerializeField] GameObject noteButton2;
    [SerializeField] GameObject noteButton3;
    [SerializeField] GameObject NoteScreen;

    [Header("Setting")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource fx;
    [SerializeField] Slider bgmSlider, fxSlider;
    [SerializeField] Toggle slowToggle, fastToggle;

    public GameObject MapScreen;
    public GameObject InternetScreen;
    private void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            stageSaved = 0;
            daySaved = 0;
            SaveGame();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                stageSaved = saveData.stage;
                daySaved = saveData.day;
            }
        }

        touchToUnlock.DOColor(transparent, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        screenPos = screenMain.position;
        appDelay = new WaitForSeconds(.55f);

        galleryZoomedPos = galleryZoomed.transform.position;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.stage = GameStats.Instance.Stage;
        saveData.day = storyManager.day;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }

    public void LoadGame(bool load)
    {
        if (load)
        {
            GameStats.Instance.Stage = stageSaved;
            storyManager.day = daySaved - 1;

            StartCoroutine(GameStartCoroutine());
        }
        else
        {
            if (stageSaved != 0 || daySaved != 0)
                screenWarning.SetActive(true);
            else
                NewGame();
        }
    }

    public void NewGame()
    {
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        StartCoroutine(NextDay());
        yield return new WaitForSeconds(3.5f);

        screenContinue.SetActive(false);
        GameStart();
    }

    void GameStart()
    {
        timer = new WaitForSeconds(1);

        if (GameStats.Instance.Stage.Equals(0))
            monologueTrigger.TriggerMonologue("Intro");
    }

    //?????? ?????? ???????? ?????? ??????????????.
    IEnumerator TimeCheck()
    {
        yield return timer;
        time++;

        if (time >= 60)
        {
            if (currentMinute < 60)
            {
                currentMinute++;
                if (currentMinute >= 60)
                {
                    currentHour++;
                    if (currentHour == 4)
                        storyManager.Ending37();
                    currentMinute = 0;
                }
                time = 0;
            }
            string minute = string.Empty;
            if (currentMinute < 10)
                minute = "0" + currentMinute.ToString();
            else
                minute = currentMinute.ToString();

            timerText.text = currentHour.ToString() + ":" + minute;
        }

        StartCoroutine(TimeCheck());
    }

    //?????? ??????????.
    public void Unlock()
    {
        clickBlock.SetActive(true);
        StartCoroutine(UnlockCoroutine());
    }

    IEnumerator UnlockCoroutine()
    {
        iconLock.sprite = spriteUnlock;

        yield return new WaitForSeconds(.3f);

        screenLocked.DOPivotY(-.5f, 0);
        screenLocked.DOMove(screenPos, .5f);
        screenMain.gameObject.SetActive(true);

        if(GameStats.Instance.Stage.Equals(0))
            monologueTrigger.TriggerMonologue("Unlocked");

        yield return appDelay;
        screenLocked.gameObject.SetActive(false);
        screenLocked.DOPivotY(.5f, 0);
        screenLocked.DOMove(screenPos, 0);

        clickBlock.SetActive(false);
        StartCoroutine(TimeCheck());
    }
    
    //?????? ??????????.
    void Lock()
    {
        iconLock.sprite = spriteLock;
        screenLocked.gameObject.SetActive(true);

        screenCurrent.DOPivotX(.5f, 0);
        screenCurrent.DOMove(screenPos, 0);

        screenCurrent.DOPivotY(1.5f, 0);
        screenCurrent.DOMove(screenPos, 0);
        screenCurrent.gameObject.SetActive(false);
        screenCurrent.DOMove(screenPos, 0);

        screenMain.gameObject.SetActive(false);
    }

    //???? ??????????.
    public void AppStart(RectTransform app)
    {
        clickBlock.SetActive(true);
        screenCurrent = app;
        StartCoroutine(AppStartCoroutine());
    }

    IEnumerator AppStartCoroutine()
    {
        screenCurrent.DOPivotY(1.5f, 0);
        screenCurrent.DOMove(screenPos, 0);
        screenCurrent.gameObject.SetActive(true);
        screenCurrent.DOPivotY(.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);
        yield return appDelay;
        clickBlock.SetActive(false);
    }

    //???? ??????????.
    public void AppShutdown()
    {
        clickBlock.SetActive(true);
        StartCoroutine(AppShutdownCoroutine());
    }

    IEnumerator AppShutdownCoroutine()
    {
        screenCurrent.DOPivotY(1.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);
        yield return appDelay;
        screenCurrent.gameObject.SetActive(false);
        screenCurrent.DOMove(screenPos, 0);
        clickBlock.SetActive(false);
    }

    //?????????? ?????? ??????????.
    public void GalleryZoomIn(Image image)
    {
        galleryZoomed.sprite = image.sprite;
        galleryZoomed.gameObject.SetActive(true);

        photoCurrent = image.gameObject;
        if (image.sprite.name.Equals("crimeplace1") && !GameStats.Instance.Stage2CheckGallery)
        {
            storyManager.Stage2CheckGallery();
        }
        else if(image.sprite.name.Equals("victim")&& !GameStats.Instance.Stage3CheckGallery)
        {
            storyManager.Stage3CheckGallery();
        }
    }

    //?????????? ???????? ????????????.
    //?????????????? ????, ?????????? ??????????.
    public void GalleryBinActivate()
    {
        if(GameStats.Instance.Stage == 21)
        {
            if (!galleryBinActivated)
            {
                galleryBinActivated = true;

                galleryBinButton.DOSizeDelta(new Vector2(65, 35), 0);
                galleryBinButtonText.text = "??????";
                galleryDeleteButton.SetActive(true);
            }
            else
            {
                clickBlock.SetActive(true);
                StartCoroutine(GalleryBinActivateCoroutine());
            }
        }
        else
        {
            monologueTrigger.TriggerMonologue("TryBin");    
        }

    }

    IEnumerator GalleryBinActivateCoroutine()
    {
        screenCurrent.DOPivotX(1.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        galleryZoomed.transform.DOMove(galleryZoomedPos, 0);
        clickBlock.SetActive(false);
    }

    //?????????? ???????? ??????????.
    public void GalleryBinDeactivate()
    {
        clickBlock.SetActive(true);
        StartCoroutine(GalleryBinDeactivateCoroutine());
    }

    IEnumerator GalleryBinDeactivateCoroutine()
    {
        screenCurrent.DOPivotX(.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        galleryZoomed.transform.DOMove(galleryZoomedPos, 0);
        clickBlock.SetActive(false);
    }

    //?????????? ?????? ??????????.
    public void GalleryDelete()
    {
        GameObject deletedPhoto = Instantiate(photoPrefab, binContent);
        deletedPhoto.GetComponent<Image>().sprite = photoCurrent.GetComponent<Image>().sprite;
        deletedPhoto.transform.DOScale(Vector3.one, 0);

        deletedPhoto.GetComponent<Button>().onClick.AddListener(() => GalleryZoomIn(deletedPhoto.GetComponent<Image>()));
        GameStats.Instance.Stage5DeletePhoto = true;
        Destroy(photoCurrent.gameObject);
    }

    //???? ???? ?????? ???? ?????? ?????? ???? ????????.
    public void CallDialReset()
    {
        callDialText.text = "";
    }

    //???? ?????? ?????? ?????? ????????.
    public void CallDialInput(string dial)
    {
        if (callDialText.text.Length < 20)
            callDialText.text += dial;
    }

    //???? ?????? ?????? ???????? ????????.
    public void CallDialDelete()
    {
        if (callDialText.text != "")
            callDialText.text = callDialText.text.Remove(callDialText.text.Length - 1);
    }

    public void CallIncome(string number)
    {
        callScreen.SetActive(true);
        callIncoming.transform.parent.gameObject.SetActive(true);
        callIncoming.text = number;
        StartCoroutine(NotGetCall());
    }
    IEnumerator NotGetCall()
    {
        yield return new WaitForSeconds(10f);
        if (callIncoming.transform.parent.gameObject.activeSelf)
        {
            GameStats.Instance.Stage = 22;
            GameStats.Instance.Stage5CheckCall = true;
            callIncoming.transform.parent.gameObject.SetActive(false);
        } else yield return null;
    }
    public void CallTake(TextMeshProUGUI number)
    {
        foreach (Transform chats in callContent)
            Destroy(chats.gameObject);

        calling.text = number.text;
        CallStart(number.text);
    }

    public void Call(TextMeshProUGUI number)
    {
        if (numbersEnable.Contains(number.text))
        {
            foreach (Transform chats in callContent)
                Destroy(chats.gameObject);

            calling.text = number.text;
            CallStart(number.text);
        }
        else
        {
            monologueTrigger.TriggerMonologue("cantcall");
            callScreen.SetActive(false);
            CallHangUp();

        }
    }
    private int PoliceCall = 0;
    private bool Solution = true;
    public void CallStart(string number)
    {
        switch (number)
        {
            case "112":
                if (GameStats.Instance.Stage == 25) {
                    if (PoliceCall == 0)
                        StartCoroutine(CallCoroutine("?? ?????????? ????????????????. ?????? ????????????", "???? ?????? ????????.", "??????????. ??????????????.", ""));
                    else if (PoliceCall == 1)
                        StartCoroutine(CallCoroutine("?? ???? ???? ??????? ???? ???????? ?????? ???????????", "????????????.", "????????????.", "????????????."));
                    else if (PoliceCall == 2)
                        StartCoroutine(CallCoroutine("???? ?????? ?????????", "8?? 6??", "8?? 5??", "8?? 4??"));
                    else if (PoliceCall == 3)
                        StartCoroutine(CallCoroutine("?????? ???? ?????????", "????????", "????", "??????"));
                    else if (PoliceCall == 4)
                        StartCoroutine(CallCoroutine("?? ???? ???????????? ??????????????.", "(?????? ??????)", "", ""));
                }else if(GameStats.Instance.Stage == 26|| GameStats.Instance.Stage == 27)
                {
                    if (PoliceCall == 0)
                        StartCoroutine(CallCoroutine("?? ?????????? ????????????????. ?????? ????????????", "?? ?????? ??????.", "??????????. ??????????????.", ""));
                    else if (PoliceCall == 1)
                    {
                        StartCoroutine(CallCoroutine("??? ???? ???? ??????? ???? ???????? ???????????", "8?? 8??","8?? 7??", "8?? 6??"));

                    }
                    else if(PoliceCall == 2)
                    {
                        StartCoroutine(CallCoroutine("???? ?????? ?????? ???????????", "??????????", "????????", "??????"));
                    }else if(PoliceCall == 3)
                    {
                        if(GameStats.Instance.Stage == 27)
                           StartCoroutine(CallCoroutine("?????? ?????? ???? ???? ???????", "????????.", "????????", "(?????????? ????????.)"));
                        else
                            StartCoroutine(CallCoroutine("?????? ?????? ???? ???? ???????", "????????.", "????????", ""));
                    }
                    else if(PoliceCall == 4)
                    {
                        StartCoroutine(CallCoroutine("???????? ???? ??????.", "(?????? ??????)", "", ""));
                    }
                }
                else
                {
                    StartCoroutine(CallCoroutine("?? ?????????? ????????????????. ?????? ????????????", "??????????. ??????????????.", "",""));
                }
                break;
            case "01012345678":
                if (GameStats.Instance.Stage.Equals(0))
                    StartCoroutine(CallCoroutine("???? ??????????.", "????1", "????2", "????3"));
                if (GameStats.Instance.Stage.Equals(1))
                    StartCoroutine(CallCoroutine("???? ????2??????.", "????2-1", "????2-2", "????2-3"));
                if (GameStats.Instance.Stage.Equals(2))
                    StartCoroutine(CallCoroutine("?????? ????????.", "????1", "????2", "?????? ??????."));
                if (GameStats.Instance.Stage.Equals(3))
                    StartCoroutine(CallCoroutine("?????? ????????.", "????1", "????2", "????3", true));
                break;
            case "01059242942":
                if (GameStats.Instance.Stage.Equals(11)|| GameStats.Instance.Stage.Equals(12)||GameStats.Instance.Stage.Equals(13))
                    StartCoroutine(CallCoroutine("??????????, ???????? ????????? ???? ???????? ???? ???????????? ??????." +
                       "?????? ?????? ?????? ???????? ?????? ???????? ?????? ?????? ?????? ???? ???? ?? ???????? ???? ??????????????." +
                       "???? 7?? 16???? ???? ???????? ?????????????", "?? ???? ??????????.", "???? ?????? ?????? ??????????.", "???? ????????????."));
                else if (GameStats.Instance.Stage.Equals(16))
                    StartCoroutine(CallCoroutine("?? ????????? ???? ???? ?????? ?????????? ???? ???????? ???? ???? ?? ?????????", "?? ??????.", "????????, ???? ???? ????????.", ""));
                else if (GameStats.Instance.Stage.Equals(17))
                    StartCoroutine(CallCoroutine("?? ?????????... ????.. ??????????~","(?????? ??????)","",""));
                else if (GameStats.Instance.Stage.Equals(18))
                    StartCoroutine(CallCoroutine("???.. ?? ???????? ??????... ??????????!","(?????? ??????)","",""));
                break;

        }
    }

    public IEnumerator CallCoroutine(string sentence, string reply1, string reply2, string reply3, bool hangUp = false)
    {
        yield return appDelay;

        GameObject call = Instantiate(callPrefab, callContent);
        call.GetComponent<TextMeshProUGUI>().text = sentence;

        if (!hangUp)
            CallReplyUpdate(reply1, reply2, reply3);
        else
            CallHangUp();
    }

    void CallReplyUpdate(string reply1, string reply2, string reply3)
    {
        callReply1.transform.parent.GetComponent<Button>().interactable = true;
        callReply2.transform.parent.GetComponent<Button>().interactable = true;
        callReply3.transform.parent.GetComponent<Button>().interactable = true;

        callReply1.text = reply1;
        callReply2.text = reply2;
        callReply3.text = reply3;
    }

    public void CallReply(TextMeshProUGUI reply)
    {

        if (reply.text.Equals("?? ???? ??????????."))
        {
            GameStats.Instance.Stage = 16;
        }
        else if (reply.text.Equals("???? ?????? ?????? ??????????."))
        {
            GameStats.Instance.Stage = 17;
        }
        else if (reply.text.Equals("???? ????????????."))
        {
            GameStats.Instance.Stage = 18;
        }
        else if (reply.text.Equals("?? ??????."))
        {
            GameStats.Instance.Stage = 19;
            GameStats.Instance.Stage5CheckCall = true;
        }
        else if (reply.text.Equals("????????, ???? ???? ????????."))
        {
            GameStats.Instance.Stage = 17;
        }
        else if (reply.text.Equals("???? ?????? ????????."))
        {
            PoliceCall = 1;
        }
        else if (reply.text.Equals("??????????. ??????????????."))
        {
            Solution = false;
            CallHangUp();
            PoliceCall = 0;
            if (GameStats.Instance.Stage >= 25 && !GameStats.Instance.Stage6Call)
                GameStats.Instance.Stage6Call = true;
        }
        else if (reply.text.Equals("????????????.") || reply.text.Equals("????????????."))
        {
            Solution = false;
            PoliceCall++;
        }
        else if (reply.text.Equals("????????????."))
        {
            PoliceCall++;
        }
        else if (GameStats.Instance.Stage == 25 && (reply.text.Equals("8?? 6??") || reply.text.Equals("8?? 4??")))
        {

            Solution = false;
            PoliceCall++;
        }
        else if (reply.text.Equals("8?? 5??"))
        {
            PoliceCall++;
        }
        else if (reply.text.Equals("????????") || reply.text.Equals("????"))
        {
            Solution = false;
            PoliceCall++;
        }
        else if (reply.text.Equals("??????"))
            PoliceCall++;
        else if (reply.text.Equals("?? ?????? ??????."))
            PoliceCall++;
        else if (reply.text.Equals("8?? 7??") || reply.text.Equals("8?? 8??"))
        {
            Solution = false;
            PoliceCall++;
        }
        else if ((GameStats.Instance.Stage == 26 || GameStats.Instance.Stage == 27)&& reply.text.Equals("8?? 6??"))
        {
            PoliceCall++;
        }
        else if (reply.text.Equals("??????????") || reply.text.Equals("??????"))
        {
            Solution = false;
            PoliceCall++;
        }
        else if (reply.text.Equals("????????"))
            PoliceCall++;
        else if (reply.text.Equals("????????.") || reply.text.Equals("????????"))
        {
            Solution = false;
            PoliceCall ++;
        }else if(reply.text.Equals("(?????????? ????????.)"))
        {
            CallHangUp();
            PoliceCall = 0;
            if (GameStats.Instance.Stage >= 25 && !GameStats.Instance.Stage6Call)
                GameStats.Instance.Stage6Call = true;
        }
    if (reply.text != "(?????? ??????)")
        {
            callReply1.transform.parent.GetComponent<Button>().interactable = false;
            callReply2.transform.parent.GetComponent<Button>().interactable = false;
            callReply3.transform.parent.GetComponent<Button>().interactable = false;

            GameObject callReply = Instantiate(callPrefab, callContent);
            callReply.GetComponent<TextMeshProUGUI>().text = reply.text;
            callReply.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;

            Canvas.ForceUpdateCanvases();
            callScrollRect.verticalNormalizedPosition = 0;

            //GameStats.Instance.Stage++;
            CallStart(calling.text);
        }

        else
        {
            CallHangUp();
            if (!GameStats.Instance.Stage5CheckCall)
                GameStats.Instance.Stage5CheckCall = true;
            //GameStats.Instance.Stage++;
            PoliceCall = 0;
            if (GameStats.Instance.Stage >= 25 && !GameStats.Instance.Stage6Call)
                GameStats.Instance.Stage6Call = true;
        }
    }

    public void CallHangUp()
    {
        callReply1.transform.parent.GetComponent<Button>().interactable = false;
        callReply2.transform.parent.GetComponent<Button>().interactable = false;
        callReply3.transform.parent.GetComponent<Button>().interactable = false;

        monologueTrigger.TriggerMonologue("HangUp");
        callEnd.SetActive(true);
        CallReset();
    }

    public void CallReset()
    {
        callScreen.SetActive(false);
        calling.transform.parent.gameObject.SetActive(false);
    }

    //?????? ?????? ?????????? ??????????.
    public void MessageChatActivate(string name)
    {
        clickBlock.SetActive(true);
        chatTrigger.TriggerChat(name);
    }

    //???? ?????? ??????????.
    public IEnumerator MessageChatUpdate(Chat chat)
    {
        chatCurrent = chat;

        for (int i = 0; i < chatContent.childCount; i++)
            Destroy(chatContent.GetChild(i).gameObject);

        chatName.text = "< " + chat.name;
        for (int i = 0; i < chat.sentences.Count; i++)
        {
            GameObject newChat = Instantiate(chatPrefab, chatContent);
            newChat.GetComponent<TextMeshProUGUI>().text= chat.sentences[i];
            newChat.transform.DOScale(Vector3.one, 0);
            if (chat.name.Equals("??????")|| chat.name.Equals("??????"))
            {
                if ((i % 2).Equals(0))
                    newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                else
                    newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            }
            else
            {
                newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            }

        }

        if (!chat.replyable)
            replyButton.interactable = false;
        else
            replyButton.interactable = true;

        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0;

        screenCurrent.DOPivotX(1.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        clickBlock.SetActive(false);
    }

    public void MessageChatReply()
    {
        if (chatName.text.Equals("< ???? ????????"))
            monologueTrigger.TriggerMonologue("Asylum");
        else
        {
            if (!replying)
            {
                replying = true;

                chatRectTransform.DOSizeDelta(new Vector2(0, -120), .5f).SetRelative();
                replyButton.transform.DOMoveY(340, .5f).SetRelative();
                replyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "????";
                ReplyCase(chatName.text);

                //ReplyCase("< ??????");
            }
            else
            {
                replying = false;

                chatRectTransform.DOSizeDelta(new Vector2(0, 120), .5f).SetRelative();
                replyButton.transform.DOMoveY(-340, .5f).SetRelative();
                replyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "?????? ??????";
            }
        }
    }

    void ReplyCase(string name)
    { 
        switch(name)
        {
            case "< ??????":
                if (GameStats.Instance.Stage.Equals(2))
                    ReplyGenerate("?????", "(???????? ??????.)", "");
                break;
            case "< ??????":if (GameStats.Instance.Stage.Equals(10)) ReplyGenerate(":)", "(???????? ??????.)", "(????????)"); break;
        }
    }

    void ReplyGenerate(string reply1Text, string reply2Text, string reply3Text)
    {
        reply2.transform.parent.gameObject.SetActive(true);
        reply3.transform.parent.gameObject.SetActive(true);

        reply1.text = reply1Text;
        reply2.text = reply2Text;
        reply3.text = reply3Text;

        if (reply3.text.Equals(""))
            reply3.transform.parent.gameObject.SetActive(false);
        if (reply2.text.Equals(""))
            reply2.transform.parent.gameObject.SetActive(false);
    }

    //?????? ????????.
    public void Reply(TextMeshProUGUI text)
    {
        string reply = text.text;
        if (GameStats.Instance.Stage.Equals(2))
        {
            if (reply.Equals("(???????? ??????.)"))
            {
                chatCurrent.replyable = false;
                GameStats.Instance.Stage=3;
            }
            else
            {
                chatCurrent.replyable = false;
                chatCurrent.sentences.Add("-----------------------\n\n" + reply + "\n\n-----------------------");
                StartCoroutine(MessageChatUpdate(chatCurrent));
                GameStats.Instance.Stage=4;
            }
        }
        if (GameStats.Instance.Stage.Equals(10))
        {
            if(reply.Equals("(???????? ??????.)"))
            {
                chatCurrent.replyable = false;
                GameStats.Instance.Stage = 20;
                GameStats.Instance.Stage5CheckMessage = true;
            }
            else if (reply.Equals("(????????)"))
            {
                chatCurrent.replyable = false;
                GameStats.Instance.Stage = 21;
            }
            else
            {
                chatCurrent.replyable = false;
                chatCurrent.sentences.Add("-----------------------\n\n" + reply + "\n\n-----------------------");
                StartCoroutine(MessageChatUpdate(chatCurrent));
                GameStats.Instance.Stage = 8;
                GameStats.Instance.Stage5CheckMessage = true;

            }
        }

        MessageChatReply();
        if (GameStats.Instance.Stage.Equals(3)||GameStats.Instance.Stage.Equals(4))
            StartCoroutine(NextDay());
    }

    //?????? ?????? ???????? ??????????.
    public void MessageChatDeactivate()
    {
        clickBlock.SetActive(true);
        StartCoroutine(MessageChatDeactivateCoroutine());

        for (int i = 0; i < chatContent.childCount; i++)
            Destroy(chatContent.GetChild(0).gameObject);
    }

    IEnumerator MessageChatDeactivateCoroutine()
    {
        screenCurrent.DOPivotX(.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        clickBlock.SetActive(false);
    }

    //?????? ??????????.
    public void NoteWrite(TextMeshProUGUI note)
    {
        
        string noteToWrite = note.text;

        if(!note.text.Equals("(?????? ????)"))
        {
            noteText.text = noteToWrite;
        }
        if(note.text.Equals("???? ???? ?????????"))
        {
            GameStats.Instance.Route = 1;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if(note.text.Equals("(?????? ????)"))
        {
            GameStats.Instance.Route = 2;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("?? ???????? ?????"))
        {
            GameStats.Instance.Route = 3;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("??????"))
        {
            GameStats.Instance.Route = 4;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("???? ??? ?? ??????.."))
        {
            GameStats.Instance.Route = 5;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if(note.text.Equals("???? ?? ???? ???? ?????????"))
        {
            GameStats.Instance.Route = 1;
        }
        else if(note.text.Equals("?? ???? ?????"))
        {
            GameStats.Instance.Route = 2;
        }
        noteButtons.DOMoveY(-340, .5f).SetRelative();


    }

    //?????? ??????????.
    public void Settings()
    {
        bgm.volume = bgmSlider.value;
        fx.volume = fxSlider.value;

        if (slowToggle.isOn)
        {
            monologueManager.typeSpeed = .13f;
            monologueManager.TypeSpeedUpdate();
        }
        else if (fastToggle.isOn)
        {
            monologueManager.typeSpeed = .01f;
            monologueManager.TypeSpeedUpdate();
        }
    }

    IEnumerator NextDay()
    {
        if (callScreen.activeSelf)
            callScreen.SetActive(false);
        GameStats.Instance.Stage5DeletePhoto = false;
        GameStats.Instance.Stage5CheckCall = false;
        GameStats.Instance.Stage5CheckMemo = false;
        GameStats.Instance.Stage5CheckMessage = false;
        GameStats.Instance.Stage4CheckMemo = false;
        GameStats.Instance.Stage4CheckMessage = false;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(new Color(0, 0, 0, 1), 3f);
        yield return new WaitForSeconds(3.5f);

        //CallHangUp();
        SaveGame();

        storyManager.day++;
        dayText.text = "8?? " + storyManager.day.ToString() + "??";
        switch(storyManager.day % 7)
        {
            case 1: dateText.text = "??????"; break;
            case 2: dateText.text = "??????"; break;
            case 3: dateText.text = "??????"; break;
            case 4: dateText.text = "??????"; break;
            case 5: dateText.text = "??????"; break;
            case 6: dateText.text = "??????"; break;
            case 0: dateText.text = "??????"; break;
        }
        Lock();

        screenLocked.gameObject.SetActive(true);


        //???? ?????? ??????????.
        noteButton1.SetActive(true);
        noteButton2.SetActive(true);
        noteButton3.SetActive(true);
        //noteButtons.DOMoveY(340, .5f).SetRelative();
        note1.text = "";
        note2.text = "";
        note3.text = "";
        if(noteButtonsRect.anchoredPosition.y<-120)
            noteButtons.DOMoveY(340, .5f).SetRelative();

        fadeImage.DOColor(new Color(0, 0, 0, 0), .5f);
        yield return new WaitForSeconds(.5f);
        fadeImage.gameObject.SetActive(false);
        OpenDaySet();
        if (note1.text.Equals(""))
            noteButton1.SetActive(false);
        if (note2.text.Equals(""))
            noteButton2.SetActive(false);
        if (note3.text.Equals(""))
            noteButton3.SetActive(false);

    }

    public void OpenDaySet()
    {
        Debug.Log(noteButtons.transform.position.y);
        switch (GameStats.Instance.Stage)
        {
            case 3:
                monologueTrigger.TriggerMonologue("OpenDay2");
                chatTrigger.Stage3JiHye();
                storyManager.Day2Update();
                break;
            case 4:
                monologueTrigger.TriggerMonologue("OpenDay2");
                storyManager.Day2DeleteJiHye();
                storyManager.Day2Update();
                break;
            case 5:
                monologueTrigger.TriggerMonologue("OpenDay3");
                storyManager.Day3Update();
                break;
            case 6:
                monologueTrigger.TriggerMonologue("OpenDay3_2");
                storyManager.Day3Update();
                break;
            case 7:
                monologueTrigger.TriggerMonologue("OpenDay4");
                chatTrigger.Stage4Ashylum();
                storyManager.Day4Update();
                break;
            case 8:
                monologueTrigger.TriggerMonologue("OpenDay4");
                chatTrigger.Stage4Ashylum();
                storyManager.Day4Update();
                break;
            case 9:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 10:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 11:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 12:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 13:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 17: case 18: case 19: case 20: case 21: case 22: storyManager.Day6Update(); break;
            case 29:
                storyManager.Ending29();
                break;
            case 30:
                storyManager.Ending30();
                break;
            case 31:
                if (!Solution)
                    storyManager.Ending31();
                else
                    storyManager.Ending32();
                break;
            case 32:
                storyManager.Ending33();
                break;
            case 33:
                if (!Solution)
                    storyManager.Ending34();
                else
                    storyManager.Ending35();
                break;
            case 34:
                storyManager.Ending36();
                break;
        }
    }
    public void CheckEnding()
    {
        if (GameStats.Instance.CheckClear(GameStats.Instance.Stage))
        {
            if (GameStats.Instance.Stage == 5)
                GameStats.Instance.Stage = 7;
            else if (GameStats.Instance.Stage == 6)
                GameStats.Instance.Stage = 8;
            else if (GameStats.Instance.Stage == 4)
                GameStats.Instance.Stage = 6;
            else if (GameStats.Instance.Stage == 3)
                GameStats.Instance.Stage = 5;
            else if (GameStats.Instance.Stage == 8 || GameStats.Instance.Stage == 7)
            {
                switch (GameStats.Instance.Route)
                {
                    case 1:
                        GameStats.Instance.Stage = 9;
                        break;
                    case 2:
                        GameStats.Instance.Stage = 10;
                        break;
                    case 3:
                        GameStats.Instance.Stage = 11;
                        break;
                    case 4:
                        GameStats.Instance.Stage = 12;
                        break;
                    case 5:
                        GameStats.Instance.Stage = 13;
                        break;
                }
                GameStats.Instance.Route = 0;
            }
            else if (GameStats.Instance.Stage == 9)
            {
                switch (GameStats.Instance.Route)
                {
                    case 1:
                        GameStats.Instance.Stage = 13;
                        break;
                    case 2:
                        GameStats.Instance.Stage = 11;
                        break;
                }
                GameStats.Instance.Stage5CheckMemo = false;
                GameStats.Instance.Route = 0;
            }
            else if (GameStats.Instance.Stage == 23)
                GameStats.Instance.Stage = 29;
            else if (GameStats.Instance.Stage == 24)
                GameStats.Instance.Stage = 30;
            else if (GameStats.Instance.Stage == 25)
                GameStats.Instance.Stage = 31;
            else if (GameStats.Instance.Stage == 26)
                GameStats.Instance.Stage = 32;
            else if (GameStats.Instance.Stage == 27)
                GameStats.Instance.Stage = 33;
            else if (GameStats.Instance.Stage == 28)
                GameStats.Instance.Stage = 34;




            StartCoroutine(NextDay());
        }
    }
    public void PhoneCallEventFinder()
    {
        StartCoroutine(PhoneCallEvent());
    }
    IEnumerator PhoneCallEvent()
    {
        yield return new WaitForSeconds(3f);
        CallIncome("01059242942");
    }
    private void Update()
    {
        if(!LockScreen.activeSelf&&!Monologue.activeSelf && !GalleryScreen.activeSelf && !MapScreen.activeSelf && !MessageScreen.activeSelf && !InternetScreen.activeSelf && !NoteScreen.activeSelf)
             CheckEnding();
        /*
        if (Input.GetKeyDown(KeyCode.Space))
            CallIncome("01012345678");
        */
    }
}
