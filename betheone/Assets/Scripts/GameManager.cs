using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.IO;
using JetBrains.Annotations;

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

    //잠금이 해제된 시점부터 시간을 업데이트합니다.
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

    //잠금을 해제합니다.
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
    
    //잠금을 설정합니다.
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

    //앱을 실행합니다.
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

    //앱을 종료합니다.
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

    //갤러리에서 사진을 확대합니다.
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

    //갤러리에서 휴지통을 활성화합니다.
    //활성화되어있을 경우, 휴지통으로 이동합니다.
    public void GalleryBinActivate()
    {
        if (!galleryBinActivated)
        {
            galleryBinActivated = true;

            galleryBinButton.DOSizeDelta(new Vector2(65, 35), 0);
            galleryBinButtonText.text = "휴지통";
            galleryDeleteButton.SetActive(true);
        }
        else
        {
            clickBlock.SetActive(true);
            StartCoroutine(GalleryBinActivateCoroutine());
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

    //갤러리에서 휴지통을 벗어납니다.
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

    //갤러리에서 사진을 삭제합니다.
    public void GalleryDelete()
    {
        GameObject deletedPhoto = Instantiate(photoPrefab, binContent);
        deletedPhoto.GetComponent<Image>().sprite = photoCurrent.GetComponent<Image>().sprite;
        deletedPhoto.transform.DOScale(Vector3.one, 0);

        deletedPhoto.GetComponent<Button>().onClick.AddListener(() => GalleryZoomIn(deletedPhoto.GetComponent<Image>()));

        Destroy(photoCurrent.gameObject);
    }

    //통화 앱을 종료할 경우 입력된 정보를 모두 지웁니다.
    public void CallDialReset()
    {
        callDialText.text = "";
    }

    //통화 앱에서 다이얼 입력을 받습니다.
    public void CallDialInput(string dial)
    {
        if (callDialText.text.Length < 20)
            callDialText.text += dial;
    }

    //통화 앱에서 입력된 다이얼을 지웁니다.
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
    }

    public void CallTake(TextMeshProUGUI number)
    {
        calling.text = number.text;
        CallStart(number.text);
    }

    public void Call(TextMeshProUGUI number)
    {
        if (numbersEnable.Contains(number.text))
        {
            calling.text = number.text;
            CallStart(number.text);
        }
    }

    public void CallStart(string number)
    {
        switch (number)
        {
            case "01012345678":
                if (GameStats.Instance.Stage.Equals(0))
                    StartCoroutine(CallCoroutine("전화 예시입니다.", "답장1", "답장2", "답장3"));
                if (GameStats.Instance.Stage.Equals(1))
                    StartCoroutine(CallCoroutine("전화 예시2입니다.", "답장2-1", "답장2-2", "답장2-3"));
                if (GameStats.Instance.Stage.Equals(2))
                    StartCoroutine(CallCoroutine("전화를 끊습니다.", "답장1", "답장2", "전화를 끊는다."));
                if (GameStats.Instance.Stage.Equals(3))
                    StartCoroutine(CallCoroutine("전화가 끊깁니다.", "답장1", "답장2", "답장3", true));
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
        if (reply.text != "전화를 끊는다.")
        {
            callReply1.transform.parent.GetComponent<Button>().interactable = false;
            callReply2.transform.parent.GetComponent<Button>().interactable = false;
            callReply3.transform.parent.GetComponent<Button>().interactable = false;

            GameObject callReply = Instantiate(callPrefab, callContent);
            callReply.GetComponent<TextMeshProUGUI>().text = reply.text;
            callReply.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;

            Canvas.ForceUpdateCanvases();
            callScrollRect.verticalNormalizedPosition = 0;

            GameStats.Instance.Stage++;
            CallStart(calling.text);
        }
        else
        {
            CallHangUp();

            GameStats.Instance.Stage++;
        }
    }

    public void CallHangUp()
    {
        callReply1.transform.parent.GetComponent<Button>().interactable = false;
        callReply2.transform.parent.GetComponent<Button>().interactable = false;
        callReply3.transform.parent.GetComponent<Button>().interactable = false;

        monologueTrigger.TriggerMonologue("HangUp");
        callEnd.SetActive(true);
    }

    public void CallReset()
    {
        foreach (Transform chats in callContent)
            Destroy(chats.gameObject);

        callScreen.SetActive(false);
        calling.transform.parent.gameObject.SetActive(false);
    }

    //메시지 앱에서 채팅창으로 이동합니다.
    public void MessageChatActivate(string name)
    {
        clickBlock.SetActive(true);
        chatTrigger.TriggerChat(name);
    }

    //채팅 내용을 불러옵니다.
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
            if (chat.name.Equals("손지혜"))
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
        if (chatName.text.Equals("< 건국 정신병원"))
            monologueTrigger.TriggerMonologue("Asylum");
        else
        {
            if (!replying)
            {
                replying = true;

                chatRectTransform.DOSizeDelta(new Vector2(0, -120), .5f).SetRelative();
                replyButton.transform.DOMoveY(340, .5f).SetRelative();
                replyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "취소";

                ReplyCase("< 손지혜");
            }
            else
            {
                replying = false;

                chatRectTransform.DOSizeDelta(new Vector2(0, 120), .5f).SetRelative();
                replyButton.transform.DOMoveY(-340, .5f).SetRelative();
                replyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "메시지 보내기";
            }
        }
    }

    void ReplyCase(string name)
    { 
        switch(name)
        {
            case "< 손지혜":
                if (GameStats.Instance.Stage.Equals(2))
                    ReplyGenerate("아니?", "(답장하지 않는다.)", "");
                break;
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

    //답장을 보냅니다.
    public void Reply(TextMeshProUGUI text)
    {
        string reply = text.text;
        if (GameStats.Instance.Stage.Equals(2))
        {
            if (reply.Equals("(답장하지 않는다.)"))
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

        MessageChatReply();
        if (GameStats.Instance.Stage.Equals(3)||GameStats.Instance.Stage.Equals(4))
            StartCoroutine(NextDay());
    }

    //메시지 앱에서 채팅창을 벗어납니다.
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

    //메모를 입력합니다.
    public void NoteWrite(TextMeshProUGUI note)
    {
        
        string noteToWrite = note.text;

        if(!note.text.Equals("(남기지 않음)"))
        {
            noteText.text = noteToWrite;
        }
        if(note.text.Equals("어제 너가 죽인거야?"))
        {
            GameStats.Instance.Route = 1;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if(note.text.Equals("(남기지 않음)"))
        {
            GameStats.Instance.Route = 2;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("뭘 도와주면 될까?"))
        {
            GameStats.Instance.Route = 3;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("알았어"))
        {
            GameStats.Instance.Route = 4;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("대체 왜? 내 맘인데.."))
        {
            GameStats.Instance.Route = 5;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if(note.text.Equals("대체 왜 이런 짓을 하는거야?"))
        {
            GameStats.Instance.Route = 1;
        }
        else if(note.text.Equals("뭘 하면 될까?"))
        {
            GameStats.Instance.Route = 2;
        }
        noteButtons.DOMoveY(-340, .5f).SetRelative();


    }

    //설정을 변경합니다.
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
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(new Color(0, 0, 0, 1), 3f);
        yield return new WaitForSeconds(3.5f);

        SaveGame();

        storyManager.day++;
        dayText.text = "8월 " + storyManager.day.ToString() + "일";
        switch(storyManager.day % 7)
        {
            case 1:
                dateText.text = "화요일";
                break;
            case 2:
                dateText.text = "수요일";
                break;
            case 3:
                dateText.text = "목요일";
                break;
            case 4:
                dateText.text = "금요일";
                break;
            case 5:
                dateText.text = "토요일";
                break;
            case 6:
                dateText.text = "일요일";
                break;
            case 0:
                dateText.text = "월요일";
                break;
        }
        Lock();

        screenLocked.gameObject.SetActive(true);


        //노트 내용을 설정합니다.
        noteButton1.SetActive(true);
        noteButton2.SetActive(true);
        noteButton3.SetActive(true);
        //noteButtons.DOMoveY(340, .5f).SetRelative();
        note1.text = "";
        note2.text = "";
        note3.text = "";
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
            else if(GameStats.Instance.Stage == 8 || GameStats.Instance.Stage == 7)
            {
                switch(GameStats.Instance.Route)
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
            else if(GameStats.Instance.Stage == 9)
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




            StartCoroutine(NextDay());
        }
    }
    private void Update()
    {
        if(!Monologue.activeSelf && !GalleryScreen.activeSelf && !MapScreen.activeSelf && !MessageScreen.activeSelf && !InternetScreen.activeSelf && !NoteScreen.activeSelf)
             CheckEnding();

        if (Input.GetKeyDown(KeyCode.Space))
            CallIncome("01012345678");
    }
}
