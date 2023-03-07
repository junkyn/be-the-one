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
    public int Route = 0;
    public bool Stage2CheckGallery = false;
    public bool Stage2CheckMessage = false;
    public bool Stage2CheckMap = false;
    public bool Stage3CheckInternet = false;
    public bool Stage3CheckGallery = false;
    public bool Stage4CheckMessage = false;
    public bool Stage4CheckMemo = false;
    public bool Stage5CheckMemo = false;
    public bool Stage5CheckMessage = false;
    public bool Stage5CheckCall = false;
    public bool Stage5DeletePhoto = false;
    public bool Stage6CheckMemo = false;
    public bool Stage6CheckNews = false;
    public bool Stage6Call = false;
    public bool CheckClear(int stage)
    {
        if (Stage == 3 || Stage == 4)
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
        else if (Stage == 7 || Stage == 8)
        {
            return (Stage4CheckMemo && Stage4CheckMessage);
        }
        else if (Stage == 9)
        {
            return (Stage5CheckMemo);

        }
        else if (Stage == 20)
        {
            return (Stage5CheckMemo && Stage5CheckMessage);
        }
        else if (Stage == 17 || Stage == 18 || Stage == 19)
        {
            return (Stage5CheckMemo && Stage5CheckCall);
        }
        else if (Stage == 21)
            return (Stage5DeletePhoto);
        else if (Stage == 22)
            return (Stage5CheckCall);
        else if (Stage == 23) return (Stage6CheckMemo && Stage6CheckNews);
        else if (Stage == 24) return (Stage6CheckNews);
        else if (Stage == 25) return (Stage6CheckMemo && Stage6CheckNews && Stage6Call);
        else if (Stage == 26) return (Stage6CheckNews && Stage6Call);
        else if (Stage == 27) return (Stage6CheckNews && Stage6Call);
        else if (Stage == 28) return (Stage6CheckNews);
        return false;
    }
}
