using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    private static GameStats instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameStats Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    public int Stage = 0;
}
