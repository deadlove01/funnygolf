using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBallFalling : MonoBehaviour {

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == Constants.ENEMY_TAG)
        {
            collider.GetComponent<SimpleFSM>().MoveToLastPosition();
            print("enemy out");
        }
        else if (collider.tag == Constants.PLAYER_TAG)
        {
            collider.GetComponent<GolfController>().MoveToLastPosition();
            print("player out");
        }
    }
}
