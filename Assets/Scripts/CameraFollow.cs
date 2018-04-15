using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;

    public float smoothRate = 2.5f;

    public float minDistance = 3f;
    public float maxDistance = 20f;
    public float distance = 10f;
    public float scrollMouseSensitivity = 30f;
    public float distanceSmooth = .1f;

    private Vector3 vel;
    private float velDist;

    private float desiredDistance = 0f;
    private float startDistance = 0f;
    private Vector3 camDesiredPos = Vector3.zero;
    void Start ()
	{
       
	}

    void Update()
    {
        UserInput();
    }
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (target == null || GameManager.Instance.GameOver)
	        return;


        CalculateCameraPosition();
        UpdateCameraPosition();
        
    }
    

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void UserInput()
    {
        var deadZone = 0.01f;
        var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel < -deadZone || mouseScrollWheel > deadZone)
        {
            desiredDistance = Mathf.Clamp(distance - mouseScrollWheel * scrollMouseSensitivity, minDistance,
                maxDistance);
        }
    }

    void CalculateCameraPosition()
    {
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref velDist, distanceSmooth);
        
        camDesiredPos = target.transform.position + new Vector3(offset.x, offset.y + distance, offset.z);
    }

    void UpdateCameraPosition()
    {
        var newPos = Vector3.Lerp(transform.position, camDesiredPos, smoothRate);
        transform.position = newPos;
    }

}
