using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfController : MonoBehaviour
{
    public GolfBase golfBase;
    public float ballSpeed = 10f;
    public float speedLoss = 0.98f;


    public float maxPower = 30f;
    public float minPower = 1f;
    public float holdTime = 5f;

    private float stopThreshold = 0.7f;

    private Rigidbody rg;



    private bool isTouched = false;

    private bool canSwipe = false;
    private bool setStartPosUI = false;
    private float currentHoldTime;
    private float currentSpeed = 0;
    private bool speedUp = false;


    private Transform holeTrans;
    private Vector3 startPos = Vector3.zero;

    [HideInInspector]
    public bool canShoot = false;

    [SerializeField] private Vector3 linePos = Vector3.zero;
    void Awake()
    {
        rg = GetComponent<Rigidbody>();
        rg.freezeRotation = true;
    }

	// Use this for initialization
	void Start ()
	{
	    GameManager.Instance.ballIsStopped = true;
	    holeTrans = GameObject.FindGameObjectWithTag("Goal").transform;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (!golfBase.isMyTurn || GameManager.Instance.GameOver || !canShoot)
	        return;
    
        if (Input.GetMouseButton(0))
	    {
	        canSwipe = true;
	    }

	    if (canSwipe
            //&& !setStartPosUI
            )
	    {
	        setStartPosUI = true;
	        IndicatorLine.Instance.SetActiveLine(true);
            //UIManager.Instance.DrawArrowDirection(Camera.main.WorldToScreenPoint(transform.position), 
            //    Input.mousePosition);

            //print("current hold time: "+currentHoldTime);
            if (currentHoldTime < holdTime)
	        {
	            currentHoldTime += Time.deltaTime;
            }else if (currentHoldTime > holdTime)
	            currentHoldTime = holdTime;

	        currentSpeed = GetSpeed();
	        var percent = GetPowerPercent(currentSpeed);
            UIManager.Instance.UpdatePowerUI(percent);
	        var dir = GetScreenDirection();
	        dir.Normalize();

	        var pos = new Vector3(transform.position.x + linePos.x, transform.position.y + linePos.y, 
                transform.position.z + linePos.z);
	        IndicatorLine.Instance.DrawLine(pos, new Vector3(dir.x, 0, dir.y));

        }

	    if (canSwipe && Input.GetMouseButtonUp(0))
	    {
	        canShoot = false;
	        GameManager.Instance.ShowGuide(false);
            IndicatorLine.Instance.SetActiveLine(false);
            var camLook = GameManager.Instance.GetCurrentCamera().GetComponent<CameraFollow>();
            camLook.isStartFollow = false;
            
	        var direction = GetScreenDirection();
            direction.Normalize();
	        rg.freezeRotation = false;
            //rg.constraints = RigidbodyConstraints.FreezePositionY;
            var newForce = new Vector3(direction.x, 0, direction.y) * currentSpeed;
            rg.AddForce(newForce, ForceMode.Impulse);
	        GameManager.Instance.ballIsStopped = false;
            Reset();

	    }

	    if (GameManager.Instance.ballIsStopped)
	    {
	        GameManager.Instance.ShowGuide(true);
        }
	    

    }

    void FixedUpdate()
    {
        if (rg.velocity.magnitude < 1 && rg.velocity.magnitude > 0.1f)
        {
            rg.velocity *= speedLoss;
        }

        if (rg.velocity.sqrMagnitude > minPower && !speedUp)
        {
            speedUp = true;
        }
        if (rg.velocity.magnitude <= stopThreshold && rg.angularVelocity.magnitude <= stopThreshold
            && speedUp)
        {
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
            GameManager.Instance.ballIsStopped = true;
           
            transform.rotation = Quaternion.LookRotation(holeTrans.position - transform.position);
            rg.constraints = RigidbodyConstraints.None;
            rg.freezeRotation = true;

            if (golfBase.isMyTurn)
            {
                var camLook = GameManager.Instance.GetCurrentCamera().GetComponent<CameraFollow>();
                camLook.isStartFollow = true;
                camLook.UpdatePosition();
                if (speedUp)
                {
                    golfBase.strokes++;
                    if (gameObject.tag == Constants.PLAYER_TAG)
                    {
                        UIManager.Instance.UpdateTextUI(golfBase.strokes);
                    }
                    GameManager.Instance.SwitchTurn();
                }
            }
           
            
      
            speedUp = false;
        }
        

    }



    //void HitBall(float speed)
    //{
    //    rg.AddForce(new Vector3(0, 0, ballSpeed), ForceMode.Impulse);
    //}



    Vector3 GetScreenDirection()
    {
        Vector3 screenPoint = GameManager.Instance.GetCurrentCamera().WorldToScreenPoint(transform.position);
        Vector2 direction = (Vector2)(screenPoint - Input.mousePosition);
        
        //var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, this.transform.position.z));
        //print("world pos: " + direction);
        return direction;
    }

    Vector3 GetWorldDirection()
    {
        var mousePoint = GameManager.Instance.GetCurrentCamera().ScreenToWorldPoint(Input.mousePosition);
        return transform.position- mousePoint ;
    }

    float GetSpeed()
    {
        var speed = currentHoldTime * maxPower / holdTime;
        speed = Mathf.Clamp(speed, minPower, maxPower);
        return speed;
    }

    float GetPowerPercent(float speed)
    {
        var rs = speed / maxPower;
        return rs > 1 ? 1 : rs;
    }



    void Reset()
    {
        canSwipe = false;
        setStartPosUI = false;
        currentHoldTime = 0;
        UIManager.Instance.UpdatePowerUI(0);
        currentSpeed = 0;
    }
}
