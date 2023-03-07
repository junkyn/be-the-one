using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class NoteApp : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (GameStats.Instance.Stage >= 9 && GameStats.Instance.Stage <= 13)
        {
            GameStats.Instance.Stage5CheckMemo = true;
        }
    }
}
