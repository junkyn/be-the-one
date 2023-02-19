using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class IDScreenCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] StoryManager storymanager;
    private void Start()
    {
        storymanager.IDScreenEvent();
    }
    private void OnDisable()
    {
        if(GameStats.Stage == 0)
        {
            storymanager.FirstDisableID();
        }
    }

}
