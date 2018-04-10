using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleFalling : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == Constants.PLAYER_TAG || collider.tag == Constants.ENEMY_TAG)
        {
            var rg = collider.GetComponent<Rigidbody>();
            print(rg.velocity.sqrMagnitude);
            if (rg.velocity.sqrMagnitude < 5.5f)
            {
                collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                //rg.velocity = new Vector3(0, 10, 0);
            }
            
        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == Constants.PLAYER_TAG || collider.tag == Constants.ENEMY_TAG)
        {
            //collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
}
