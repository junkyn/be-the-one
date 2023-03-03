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
    public bool Stage2CheckGallery = false;
    public bool Stage2CheckMessage = false;
    public bool Stage2CheckMap = false;
    public bool CheckClear(int stage)
    {
        if(Stage>=3 || Stage <= 6)
        {
            return (Stage2CheckGallery && Stage2CheckMessage && Stage2CheckMap);
        }
        return false;
    }
}
