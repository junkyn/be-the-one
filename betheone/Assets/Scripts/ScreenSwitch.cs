using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenSwitch : MonoBehaviour
{
    Image screenSaver;
    Color transparent = new Color(0, 0, 0, 0), black = new Color(0, 0, 0, 1);

    private void OnEnable()
    {
        if (screenSaver == null)
            screenSaver = transform.GetChild(transform.childCount - 1).GetComponent<Image>();

        screenSaver.DOColor(transparent, .5f);
        screenSaver.raycastTarget = false;
    }

    public void Disable()
    {
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        screenSaver.DOColor(black, .5f);
        screenSaver.raycastTarget = true;

        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
