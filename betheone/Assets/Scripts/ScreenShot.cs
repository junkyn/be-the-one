using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnDestroy()
    {
        GameStats.Instance.Stage5CheckMessage = true;
    }
}
