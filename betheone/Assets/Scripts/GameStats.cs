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
    public bool Stage3CheckInternet = false;
    public bool Stage3CheckGallery = false;
    public bool Stage4CheckMessage = false;
    public bool Stage4CheckMemo = false;
    public bool CheckClear(int stage)
    {
        if(Stage == 3 || Stage == 4)
        {
            return (Stage2CheckGallery && Stage2CheckMessage && Stage2CheckMap);
        }
        else if (Stage == 5)
        {
            return (Stage3CheckGallery && Stage3CheckInternet);
        }
        else if (Stage == 6)
        {
            return (Stage3CheckInternet);
        }
        return false;
    }
}
