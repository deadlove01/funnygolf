using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject[] gamers;

    public Camera GetCurrentCamera()
    {
        if(currentCamera == null)
            Debug.LogError("Current Camera is null, something goes wrong!");
        return currentCamera;
    }

    public GameObject[] cameras;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Vector3[] positions;

    private int camIndex = 0;
    private bool isShowGuide = false;

    private Camera currentCamera;
    private int gameIndex = 0;
    private bool isGameStarted = false;

    private MapGenerator mapGenerator;

   
   
    void Start()
    {
        if (cameras == null || cameras.Length == 0)
        {
            Debug.LogError("Cameras array need to declare!");
        }
        camIndex = 0;
        currentCamera = cameras[camIndex].GetComponent<Camera>();

       
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap2();
        positions = new Vector3[2]
        {
            new Vector3(0.5f, 0.62f, 0),
            new Vector3(-0.5f, 0.62f, 0),
        };
        //var camFollow = currentCamera.GetComponent<CameraFollow>();
        //camFollow.target = GameObject.FindGameObjectWithTag(Constants.MAP_TAG).transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }
    }

    public void StartGame()
    {
        if (gamers == null || gamers.Length == 0)
        {
            Debug.LogError("Gamers array need to declare!");
        }
        gamers = new GameObject[2];
        gamers[0] = Instantiate(playerPrefab, positions[0], Quaternion.identity);
        gamers[0].name = Constants.PLAYER_TAG;
        gamers[1] = Instantiate(enemyPrefab, positions[1], Quaternion.identity);
        gamers[1].name = Constants.ENEMY_TAG;

        for (int i = 0; i < gamers.Length; i++)
        {
            var go = gamers[i];
            if (i == gameIndex)
            {
                if (go.tag == Constants.PLAYER_TAG)
                {
                    var golfScript = go.GetComponent<GolfController>();
                    golfScript.golfBase.isMyTurn = true;
                    golfScript.canShoot = true;
                }
                else if (go.tag == Constants.ENEMY_TAG)
                {
                    var golfScript = go.GetComponent<SimpleFSM>();
                    golfScript.golfBase.isMyTurn = true;
                    golfScript.canShoot = true;
                }
                print(gamers[gameIndex].tag + " turn!");
                UIManager.Instance.UpdateTurnText(go.name);
            }
            else
            {
                if (go.tag == Constants.PLAYER_TAG)
                {
                    gamers[i].GetComponent<GolfController>().golfBase.isMyTurn = false;
                }
                else if (go.tag == Constants.ENEMY_TAG)
                {
                    gamers[i].GetComponent<SimpleFSM>().golfBase.isMyTurn = false;
                }

            }
        }

      
        isGameStarted = true;
        UIManager.Instance.UpdateLevelText(LevelTracking.Instance.currentLevel);
    }

    public void SwitchCamera()
    {
        if (GameOver)
            return;
        print("camera length: "+cameras.Length);
        if (cameras.Length == 1)
            return;


        print("switch camera!");
        cameras[camIndex].SetActive(false);
        if (camIndex == cameras.Length - 1)
            camIndex = 0;
        else
            camIndex++;
        cameras[camIndex].SetActive(true);
        currentCamera = cameras[camIndex].GetComponent<Camera>();
        
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

    public void SwitchTurn()
    {
        if (!isGameStarted)
            return;
        var go = gamers[gameIndex];
        if(go.tag == Constants.PLAYER_TAG)
            gamers[gameIndex].GetComponent<GolfController>().golfBase.isMyTurn = false;
        else if (go.tag == Constants.ENEMY_TAG)
            go.GetComponent<SimpleFSM>().golfBase.isMyTurn = false;
        if (gameIndex == gamers.Length - 1)
        {
            gameIndex = 0;
        }
        else
        {
            gameIndex++;
        }
        print("game index: "+gameIndex);
        go = gamers[gameIndex];
        if (go.tag == Constants.PLAYER_TAG)
        {
            var golfScript = go.GetComponent<GolfController>();
            golfScript.golfBase.isMyTurn = true;
            golfScript.canShoot = true;
        }

        else if (go.tag == Constants.ENEMY_TAG)
        {
            var golfScript = go.GetComponent<SimpleFSM>();
            golfScript.golfBase.isMyTurn = true;
            golfScript.canShoot = true;
            print("enemy set turn: "+golfScript.golfBase.isMyTurn);
        }
        print(gamers[gameIndex].tag + " turn!");
        var camFollow = currentCamera.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.target = go.transform;
        }

        UIManager.Instance.UpdateTurnText(go.name);
    }


    public void EndGame(bool isPlayerWon)
    {
        UIManager.Instance.ShowGameOverPanel(true, isPlayerWon);
    }

    public void NextLevel()
    {
        LevelTracking.Instance.currentLevel++;
        SceneManager.LoadScene(0);
    }

}
