using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class NoteApp : MonoBehaviour
{
    [SerializeField] StoryManager storyManager;
    // Start is called before the first frame update
    [SerializeField] GameManager gameManager;
    void OnEnable()
    {
        if (GameStats.Instance.Stage >= 9 && GameStats.Instance.Stage <= 13)
        {
            GameStats.Instance.Stage5CheckMemo = true;
        }
    }
    private void OnDisable()
    {
        if(GameStats.Instance.Stage == 11 || GameStats.Instance.Stage ==12 || GameStats.Instance.Stage == 13)
        {
            gameManager.PhoneCallEventFinder();
        }
    }
}
