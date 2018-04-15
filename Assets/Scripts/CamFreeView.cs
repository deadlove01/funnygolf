using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFreeView : MonoBehaviour
{


    public Transform target;
    public float minDistance = 3f;
    public float maxDistance = 7f;
    public float distance = 5f;
    public float cameraSmooth = 1f;
    public float distanceSmooth = 1f;

    public float xMouseSensitivity = 5f;
    public float yMouseSensitivity = 5f;
    public float scrollMouseSensitivity = 5f;

    public float yMinLimit = -40;
    public float yMaxLimit = 40;

    private Vector3 camDesiredPos = Vector3.zero;

    private float startDistance = 0f;

    private float desiredDistance = 0f;
    private float camDistance = 0f;
    private float mouseX = 0;
    private float mouseY = 0;
    private float vel;

    void Awake()
    {
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        startDistance = camDistance = desiredDistance = distance;
        mouseX = 0;
        mouseY = 10f;
    }

    void OnEnable()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG).transform;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    UserInput();

	}


    void LateUpdate()
    {
        if (target == null)
            return;

        CalculateCameraPosition();

        UpdateCameraPosition();
    }


    void UserInput()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * xMouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * yMouseSensitivity;
        }

        mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);
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
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref vel, distanceSmooth);

        var direction = new Vector3(0,0, -distance);
        var rot = Quaternion.Euler(new Vector3(mouseY, mouseX, 0));
        camDesiredPos = target.transform.position + rot * direction;
    }

    void UpdateCameraPosition()
    {
        var newPos = Vector3.Lerp(transform.position, camDesiredPos, cameraSmooth);
        transform.position = newPos;
        transform.LookAt(target);
    }

    float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle > 360)
                angle -= 360;
            else if (angle < -360)
                angle += 360;


        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);
    }

}
