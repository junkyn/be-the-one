using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monologue
{
    public string stage;

    [TextArea(3, 10)]
    public string[] sentences;
}
