using System.Collections;
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
        if (GameStats.Instance.Stage >= 23 && !GameStats.Instance.Stage6CheckMemo)
            GameStats.Instance.Stage6CheckMemo = true;
    }
    private void OnDisable()
    {
        if(GameStats.Instance.Stage == 11 || GameStats.Instance.Stage ==12 || GameStats.Instance.Stage == 13)
        {
            gameManager.PhoneCallEventFinder();
        }

    }
}
