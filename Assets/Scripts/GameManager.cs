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
    public bool enemyWin = false;
    public bool playerWin = false;
    public GolfBase[] gamers;

    public Camera GetCurrentCamera()
    {
        if(currentCamera == null)
            Debug.LogError("Current Camera is null, something goes wrong!");
        return currentCamera;
    }

    public Camera[] cameras;

    private int camIndex = 0;
    private bool isShowGuide = false;

    private Camera currentCamera;
    void Start()
    {
        if (cameras == null || cameras.Length == 0)
        {
            Debug.LogError("Cameras array need to declare!");
        }
        camIndex = 0;
        currentCamera = cameras[camIndex++];

        if (gamers == null || gamers.Length == 0)
        {
            Debug.LogError("Gamers array need to declare!");
        }
    }

    public void SwitchCamera()
    {
        if (cameras.Length == 1)
            return;
        if (camIndex == cameras.Length - 1)
            camIndex = 0;
        currentCamera = cameras[camIndex++];
    }

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
