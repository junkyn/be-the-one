using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] StoryManager storyManager;
    public List<Chat> chats;

    public void TriggerChat(string name)
    {
        for (int i = 0; i < chats.Count; i++)
            if (chats[i].name.Equals(name))
            {
                StartCoroutine(gameManager.MessageChatUpdate(chats[i]));

                if(name.Equals("손지혜") && GameStats.Instance.Stage.Equals(1))
                {
                    storyManager.OpenFirstJiHye();
                }
                if(name.Equals("손지혜")&& GameStats.Instance.Stage.Equals(3))
                {
                    storyManager.Stage3OpenJiHye();
                }
                if (name.Equals("건국 정신병원") && (GameStats.Instance.Stage.Equals(7) || GameStats.Instance.Stage.Equals(8)))
                {
                    if(!GameStats.Instance.Stage4CheckMessage)
                        storyManager.Day4CheckMessage();
                }
            }
    }
    public void Stage3JiHye()
    {
        for(int i = 0; i<chats.Count; i++)
        {
            if (chats[i].name.Equals("손지혜"))
            {
                chats[i].sentences.Add("-----------------------\n\n미안 지금 일어났어 ㅋㅋ 무슨일이야?\n\n-----------------------");
                chats[i].sentences.Add("-----------------------\n\n아니야 우리 근데 언제 만나?\n\n-----------------------");
                chats[i].sentences.Add("-----------------------\n\n음... 내일 11시 어때?\n\n-----------------------");
                chats[i].sentences.Add("-----------------------\n\n알았엉 그럼 내일 봐~\n\n-----------------------");
                StartCoroutine(gameManager.MessageChatUpdate(chats[i]));
            }
        }
    }
    public void Stage4Ashylum()
    {
        for (int i = 0; i < chats.Count; i++)
        {
            if (chats[i].name.Equals("건국 정신병원"))
            {
                chats[i].sentences.Add("-----------------------\n\n 진료결과 이해성씨의 증상은 해리성 정체감 장애중 특이 케이스로 분류될 것 같습니다.\n" +
                    "낮과 밤의 뇌 활동이 달라 서로 기억하지 못하는 증상이 있을 수 있고,\n신체 장애가 발생할 가능성도 있습니다.\n" +
                    "만약 그 증상이 있다면 메모를 통해 소통하는 것을 추천드리며\n뇌의 활동이 비슷해지면 자연스럽게 치료 될 것으로 추측됩니다.\n"
                    + "또 다른 궁금점이나 문제점이 있다면 전화 상담이나, 다시 방문해주시기 바랍니다.\n감사합니다.\n\n-----------------------");
            }
        }
    }
}
