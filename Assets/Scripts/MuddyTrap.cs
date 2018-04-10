using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyTrap : MonoBehaviour
{

    public float dragValue = 10f;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" || collider.tag == "Enemy")
        {
            collider.GetComponent<Rigidbody>().drag = dragValue;
        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player" || collider.tag == "Enemy")
        {
            collider.GetComponent<Rigidbody>().drag = 0;
        }
    }
}
