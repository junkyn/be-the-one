using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClockCtrl : MonoBehaviour
{
    float _sec = 0;
    int _Min = 0;
    int _Hour = 2;
    public TextMeshPro Timetxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        if (_Min < 10)
        {
            Timetxt.text = _Hour + ":0" + _Min;
        }
        else
        {
            Timetxt.text = _Hour + ":" + _Min;
        }
        if (_Hour == 4)
        {

        }
    }
    void Timer()
    {
        _sec += Time.deltaTime;
        if ((int)_sec > 59)
        {
            _sec = 0;
            _Min++;
        }
        if (_Min > 59)
        {
            _Min = 0;
            _Hour++;
        }
    }
}
