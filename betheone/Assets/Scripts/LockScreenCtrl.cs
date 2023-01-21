using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class LockScreenCtrl : MonoBehaviour
{
    public GameObject textObj;
    public SpriteRenderer locksprite;
    public Sprite unlocksp;
    public GameObject app;
    int _date;
    int month = 7;
    string[] daylist = { "월", "화", "수", "목", "금", "토", "일" };
    string day;
    public TextMeshPro Datetxt;
    public TextMeshPro Daytxt;
    // Start is called before the first frame update
    void Start()
    {
        _date = GameStats.Date;
        day = daylist[_date % 7];
        if (_date > 31)
        {
            month++;
            _date -= 31;
        }
        Datetxt.text = month + "월 " + _date + "일";
        Daytxt.text = day+"요일";
    }

    // Update is called once per frame
    void Update()
    {
        if (!textObj.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                if (hit.collider != null)
                {
                    GameObject click_obj = hit.transform.gameObject;
                    Debug.Log(click_obj.name);
                    if (click_obj.name == "fingerprint")
                    {
                        StartCoroutine(unlock());
                    }
                }
            }
        }
    }
    IEnumerator unlock()
    {
        locksprite.sprite = unlocksp;
        yield return new WaitForSeconds(0.3f);
        app.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
