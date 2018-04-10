using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyTrap : MonoBehaviour
{

    public float dragValue = 10f;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == Constants.PLAYER_TAG || collider.tag == Constants.ENEMY_TAG)
        {
            collider.GetComponent<Rigidbody>().drag = dragValue;
        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == Constants.PLAYER_TAG || collider.tag == Constants.ENEMY_TAG)
        {
            collider.GetComponent<Rigidbody>().drag = 0;
        }
    }
}
