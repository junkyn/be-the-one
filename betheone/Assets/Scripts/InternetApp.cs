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
        if (GameStats.Instance.Stage >= 23 && !GameStats.Instance.Stage6CheckNews)
            GameStats.Instance.Stage6CheckNews = true;
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
