using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageApp : MonoBehaviour
{
    [SerializeField] StoryManager storyManager;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (GameStats.Instance.Stage == 4)
        {
            storyManager.Stage4CheckMessage();
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
