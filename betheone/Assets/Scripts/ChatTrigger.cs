using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public List<Chat> chats;

    public void TriggerChat(string name)
    {
        for (int i = 0; i < chats.Count; i++)
            if (chats[i].name.Equals(name))
                StartCoroutine(gameManager.MessageChatUpdate(chats[i]));
    }
}
