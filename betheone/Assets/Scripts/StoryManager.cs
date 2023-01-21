using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public GameObject textObj;
    // Start is called before the first frame update
    void Start()
    {
        switch (GameStats.Stage)
        {
            case 1:
                textObj.SetActive(true); break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
