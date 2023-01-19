using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public GameObject background;
    bool wait = true;
    public TextMeshProUGUI tf;
    public TextMeshProUGUI arrow;
    public float timeForCharacter;
    float characterTime;
    string dialog;
    bool isTypingEnd = false;
    float timer;
    public static bool isTyping = false;
    public static int index = 0;
    public string[] str;
    char[] chars;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        tf.text = "";
        switch (GameStats.Stage)
        {
            case 0:
                Debug.Log(0);
                str = new string[] { ".....","¹¹Áö?","È¥¶õ½º·´´Ù" };
                Debug.Log(str[1]);
                break;
        }
        index = 0;
        isTyping = false;
        isTypingEnd = true;
        timer = timeForCharacter;
        characterTime = timeForCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTyping&&isTypingEnd)
        {
            isTyping = true;
            chars = str[index].ToCharArray();
            StartCoroutine(Typer(chars, tf));
        }
        if (isTyping&&isTypingEnd&&wait)
        {
            wait = false;
            StartCoroutine(goNext());
        }

    }
    IEnumerator goNext()
    {
        Debug.Log(index);
        float k = 0;
        bool turn = true;
        while (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0))
                break;
            if (turn)
            {
                k += 0.01f;
                yield return new WaitForSeconds(0.01f*Time.deltaTime);
                if (k >= 1f)
                    turn = false;
            }
            else
            {
                k -= 0.01f;
                yield return new WaitForSeconds(0.01f * Time.deltaTime);
                if (k <= 0f)
                    turn = true;
            }
            arrow.color = new Color(1f, 1f, 1f, k);

        }
        index++;
        if(index == str.Length)
        {
            tf.text = "";
            TextManager.isTyping = false;
            wait = true;
            Destroy(background);
        }
        tf.text = "";
        TextManager.isTyping = false;
        wait = true;



    }
    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;
        while (currentChar < charLength)
        {
                if (timer >= 0)
                {
                    yield return null;
                    timer -= Time.deltaTime;
                }
                else
                {
                     this.audioSource.Stop();
                    this.audioSource.Play();
                    textObj.text += chars[currentChar].ToString();
                    currentChar++;
                    timer = characterTime;
                }

        }
        if (currentChar >= charLength)
        {
            isTypingEnd = true;
            yield break;
        }
    }



}
