using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleChecker : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == Constants.ENEMY_TAG)
        {
            GameManager.Instance.enemyWin = true;
            GameManager.Instance.GameOver = true;
            GameManager.Instance.EndGame(false);
            print("enemy win");
        }else if (collider.tag == Constants.PLAYER_TAG)
        {
            GameManager.Instance.playerWin = true;
            GameManager.Instance.GameOver = true;
            GameManager.Instance.EndGame(true);
            print("player win");
        }
    }
}
