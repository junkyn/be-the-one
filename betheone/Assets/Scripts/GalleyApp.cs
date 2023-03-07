using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleyApp : MonoBehaviour
{
    [SerializeField] StoryManager storyManager;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if(GameStats.Instance.Stage == 21)
        {
            storyManager.ToDeleteMonol();
        }
    }
}
