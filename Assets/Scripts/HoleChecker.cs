using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleChecker : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            GameManager.Instance.enemyWin = true;
        }else if (collider.tag == "Player")
        {
            GameManager.Instance.playerWin = true;
        }
    }
}
