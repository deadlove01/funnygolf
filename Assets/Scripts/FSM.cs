using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;
    public GolfBase golfBase;
    protected Transform playerTrans;

    protected Transform goalTrans;

    protected virtual void Init() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    protected Rigidbody rg;
    // Use this for initialization
    void Start ()
	{
	    playerTrans = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG).transform;
	    goalTrans = GameObject.FindGameObjectWithTag(Constants.GOAL_TAG).transform;
	    rg = GetComponent<Rigidbody>();
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
