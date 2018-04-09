using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    #region simple singleton

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

#endregion

    public bool GameOver = false;

    public bool ballIsStopped = true;
    private bool isShowGuide = false;


    public void ShowGuide(bool value)
    {
        if (value)
        {
            if (!isShowGuide)
            {
                print("show");
                UIManager.Instance.ShowHideCircle(true);
                isShowGuide = true;
            }
        }
        else
        {
            print("hide");
            UIManager.Instance.ShowHideCircle(false);
            isShowGuide = false;
        }
    }

}
