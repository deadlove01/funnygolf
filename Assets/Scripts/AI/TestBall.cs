using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{

    private Rigidbody rg;

    private bool speedUp = false;
	// Use this for initialization
	void Start ()
	{
	    rg = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (rg.velocity.magnitude < 1 && rg.velocity.magnitude > 0.01f)
        {
            rg.velocity *= 0.9f;
        }
        if (rg.velocity.sqrMagnitude > 1f && !speedUp)
        {
            speedUp = true;
        }
        if (rg.velocity.magnitude <= 0.1f && rg.angularVelocity.magnitude <= 0.1f && speedUp
        )
        {
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;

            rg.constraints = RigidbodyConstraints.None;
            rg.freezeRotation = true;
            speedUp = false;

        }
    }
}
