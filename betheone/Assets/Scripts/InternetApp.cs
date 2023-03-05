using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetApp : MonoBehaviour
{
    [SerializeField] StoryManager storyManager;
    private void OnEnable()
    {
        if (GameStats.Instance.Stage == 5 || GameStats.Instance.Stage == 6)
        {
            storyManager.Stage3CheckInternet();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
