using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float smoothRate = 2.5f;
    private Vector3 vel = Vector3.zero;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (target == null)
	        return;

	    var newPos = target.position - offset;
	    transform.position = Vector3.SmoothDamp(transform.position, newPos, ref vel,  smoothRate);
	}
}
