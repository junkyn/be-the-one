using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chat
{
    public string name;

    [TextArea(3, 10)]
    public List<string> sentences;

    public bool replyable = true;
}
