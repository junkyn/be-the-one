using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MonologueManager : MonoBehaviour
{
    public Queue<string> sentences;

    [SerializeField] MonologueTrigger monologueTrigger;
    [SerializeField] TextMeshProUGUI monologueText;

    public float typeSpeed;
    WaitForSeconds typeDelay;
    bool typing;
    string currentSentence;
    [SerializeField] TextMeshProUGUI arrow;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        sentences = new Queue<string>();

        TypeSpeedUpdate();
        arrow.DOColor(new Color(1, 1, 1, 0), .15f).SetLoops(-1, LoopType.Yoyo);
    }

    //타이핑 속도를 업데이트합니다.
    public void TypeSpeedUpdate()
    {
        typeDelay = new WaitForSeconds(typeSpeed);
    }

    private void Update()
    {
        //클릭하여 다음 문장으로 넘어갑니다.
        //문장이 타이핑되고 있다면, 타이핑을 스킵합니다.
        if (Input.GetMouseButtonDown(0))
        {
            if (!typing)
            DisplayNextSentence();
            else
            {
                monologueText.text = currentSentence;
                StopAllCoroutines();
                EndTyping();
            }
        }
    }

    //독백을 시작합니다.
    public void StartMonologue(Monologue monologue)
    {
        Debug.Log(monologue.stage);
        monologueTrigger.gameObject.SetActive(true);
        sentences.Clear();

        foreach (string sentence in monologue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    //다음 문장으로 넘어갑니다.
    public void DisplayNextSentence()
    {
        if (sentences.Count.Equals(0))
        {
            EndMonologue();
            return;
        }

        string sentence = sentences.Dequeue();
        currentSentence = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //문장을 타이핑합니다.
    IEnumerator TypeSentence(string sentence)
    {
        monologueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            typing = true;
            arrow.gameObject.SetActive(false);

            audioSource.Stop();
            audioSource.Play();

            monologueText.text += letter;
            yield return typeDelay;

            if (monologueText.text.Equals(sentence))
            {
                EndTyping();
            }
        }
    }
    
    //타이핑을 종료합니다.
    void EndTyping()
    {
        typing = false;
        arrow.gameObject.SetActive(true);
    }

    //독백을 종료합니다.
    void EndMonologue()
    {
        monologueTrigger.gameObject.SetActive(false);
    }

}
