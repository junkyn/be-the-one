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
    private void OnDisable()
    {
        if(GameStats.Instance.Stage == 21 && !GameStats.Instance.Stage5CheckMessage)
        {
            storyManager.HyeJinScreenShot();
        }        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
