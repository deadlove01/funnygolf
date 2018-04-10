using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public Vector3 updateOffset;
    public float smoothRate = 2.5f;
    private Vector3 vel = Vector3.zero;
    
    [SerializeField]
    private bool lookRotation = false;

    private Transform holeTrans;

    [HideInInspector]
    public bool isStartFollow = false;

	// Use this for initialization
	void Start ()
	{
	    isStartFollow = false;
        holeTrans = GameObject.FindGameObjectWithTag("Hole").transform;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (target == null)
	        return;


	  
        if (lookRotation)
	    {
	        if (!isStartFollow)
	        {
	            var newPos = target.position - offset;
	            vel = Vector3.zero;


	            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref vel, smoothRate);
	            transform.LookAt(target);
	        }

            //transform.position = target.TransformPoint(offset);
            //transform.LookAt(target);
            //var angles = transform.eulerAngles;
            //transform.eulerAngles = new Vector3(angles.x, target.eulerAngles.y, angles.z);
            //   var newPos = holeTrans.position - target.position;
            //   //transform.position = Vector3.SmoothDamp(transform.position, newPos, ref vel, smoothRate);
            //   //   var dir = holeTrans.position -target.position ;
            //   //   dir.Normalize();
            //   var rot = transform.eulerAngles;
            //   var newRot = Quaternion.Euler(rot.x, target.rotation.y, rot.z);
            //   //transform.position = target.position + new Vector3(0, 0, -offset.z);
            ////transform.position = target.position - offset;
            //transform.position = target.TransformPoint(offset);
            //   //transform.rotation = Quaternion.LookRotation(holeTrans.position - transform.position, Vector3.up);
            //   //transform.LookAt(target);

            //transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            //t/*ransform.position = target.position + newRot * new Vector3(0,0, -offset.z);*/
            //   //transform.position = holeTrans.position - target.position;
            //var newPos = target.position - offset;
            //Vector3 direction = offset;
            //Quaternion rotation = Quaternion.Euler(target.rotation.x, target.rotation.y, 0);
            //transform.position = target.position + rotation * direction;
            //transform.LookAt(target);
            //transform.position = Vector3.SmoothDamp(transform.position, newPos, ref vel, smoothRate);
            //transform.LookAt(target);
            //transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            //transform.LookAt(dir, Vector3.up);
            //transform.position = Vector3.SmoothDamp(transform.position, target.position + (-dir *offset.magnitude), ref vel, smoothRate);
            //   Debug.DrawRay(target.position, dir * 10, Color.black);
            //var rotation = Quaternion.LookRotation(dir, Vector3.up);
            //var angles = transform.eulerAngles;
            //transform.eulerAngles = new Vector3(angles.x, rotation.y, angles.z);
        }
	    else
	    {
	        var newPos = target.position - offset;
	        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref vel, smoothRate);
        }
	  
    }

    public void UpdatePosition()
    {
        if (lookRotation)
        {
            if (isStartFollow)
            {
                isStartFollow = true;
                transform.position = target.TransformPoint(updateOffset);
                var angles = transform.eulerAngles;
                transform.eulerAngles = new Vector3(angles.x, target.eulerAngles.y, angles.z);
            }

        }

    }
}
