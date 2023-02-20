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
}
