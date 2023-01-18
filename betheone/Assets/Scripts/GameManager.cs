using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    Color transparent = new Color(1, 1, 1, 0);

    [SerializeField] ScreenSwitch screenSwitch1, screenSwitch2;

    [SerializeField] Image screenSaver;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image iconLock;
    [SerializeField] Sprite spriteUnlock;
    [SerializeField] TextMeshProUGUI touchToUnlock;

    void Start()
    {
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(1f);

        screenSwitch1.gameObject.SetActive(true);
        touchToUnlock.DOColor(transparent, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void Unlock()
    {
        StartCoroutine(UnlockCoroutine());
    }

    IEnumerator UnlockCoroutine()
    {
        title.DOColor(transparent, .5f);
        iconLock.sprite = spriteUnlock;
        iconLock.DOColor(transparent, .5f).SetDelay(.5f);
        touchToUnlock.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        screenSwitch1.Disable();
        screenSwitch2.gameObject.SetActive(true);
    }

    void Update()
    {
        
    }
}
