using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TextCtrl : MonoBehaviour
{
   
    TextMeshProUGUI textObj;
    public float timeForCharacter;

    float characterTime;
    string dialog;
    bool isTypingEnd = false;
    bool isSkip = false;
    float timer;
    int dialogNumber = 0;
    TextMeshProUGUI arrow;
    public TextCtrl(string dialogs,TextMeshProUGUI tf, TextMeshProUGUI arrow)
    {
        this.textObj = tf;
        this.arrow = arrow;
        TextManager.isTyping = true;
        dialog = dialogs;
        isTypingEnd = false;
        timer = timeForCharacter;
        characterTime = timeForCharacter;
        char[] chars = dialogs.ToCharArray();
        StartCoroutine(Typer(chars, textObj));
    }
    ~TextCtrl()
    {
        TextManager.index++;
    }
    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;
        while(currentChar < charLength)
        {
            if (!isSkip)
            {
                if (timer >= 0)
                {
                    yield return null;
                    timer -= Time.deltaTime;
                }
                else
                {
                    textObj.text += chars[currentChar].ToString();
                    currentChar++;
                    timer = characterTime;
                }
            }
            else
            {
                textObj.text = dialog.ToString();
                currentChar = charLength;
                isTypingEnd = true;
                yield break;
            }
        }if(currentChar >= charLength)
        {
            isTypingEnd = true;
            dialogNumber++;
            yield break;
        }
    }
    private void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (!isSkip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSkip = true;
            }

        }
        if (isTypingEnd)
        {
            StartCoroutine(goNext());
        }

    }
    IEnumerator goNext()
    {
        float k = 0;
        bool turn = false;
        while (!Input.GetMouseButtonDown(0))
        {
            if (turn)
            {
                k += 0.05f;
                yield return new WaitForSeconds(0.1f);
                if (k >= 1f)
                    turn = false;
            }
            else
            {
                k -= 0.05f;
                yield return new WaitForSeconds(0.1f);
                if(k<=0f)
                    turn = true;
            }
            arrow.color = new Color(1f, 1f, 1f, k);
        }
        TextManager.isTyping = false;
        

    }
}
