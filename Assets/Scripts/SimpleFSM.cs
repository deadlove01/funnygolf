using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleFSM : FSM {

    public enum FSMState
    {
        None,
        Attack,
        Goal,
        SmartGoal
    }

    public FSMState currentState;
    public float attackDistance = 5f;
    public bool smart = false;
    public bool canShoot = false;

    public float speedLoss = 0.9f;
    private bool speedUp = false;
    private float stopThreshold = 0.7f;
    [SerializeField]private float speed = 10f;
    private System.Random rand = new System.Random(DateTime.Now.Millisecond);

    protected override void Init()
    {
        base.Init();
        if (smart)
            currentState = FSMState.SmartGoal;
        else
            currentState = FSMState.Goal;
    }

    protected override void FSMUpdate()
    {
        if (!golfBase.isMyTurn || !canShoot)
            return;
        base.FSMUpdate();
        
        if (smart)
        {
            var goalDistance = Vector3.Distance(transform.position, goalTrans.position);
            var playerDistance = Vector3.Distance(transform.position, playerTrans.position);
            if (goalDistance > playerDistance)
            {
                currentState = FSMState.Attack;
            }
            else
            {
                currentState = FSMState.SmartGoal;
            }
        }
        switch (currentState)
        {
            case FSMState.Goal: MoveToGoal();
                break;
            case FSMState.SmartGoal: SmartMoveToGoal();
                break;
            case FSMState.Attack: AttackPlayer();
                break;
        }
    }


    protected override void FSMFixedUpdate()
    {
        base.FSMFixedUpdate();

        if (rg.velocity.magnitude < 1 && rg.velocity.magnitude > 0.1f)
        {
            rg.velocity *= speedLoss;
        }

        if (rg.velocity.sqrMagnitude > golfBase.minPower && !speedUp)
        {
            speedUp = true;
        }
        if (rg.velocity.magnitude <= stopThreshold && rg.angularVelocity.magnitude <= stopThreshold
            && speedUp)
        {
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
            GameManager.Instance.ballIsStopped = true;

            transform.rotation = Quaternion.LookRotation(goalTrans.position - transform.position);
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


    void MoveToGoal()
    {
        print("MoveToGoal");
        canShoot = false;
        StartCoroutine(IMove());
        //var goalPos = goalTrans.position;
        //goalPos = new Vector3(goalPos.x, transform.position.y, goalPos.z);
        //var goalDistance = Vector3.Distance(transform.position, goalPos);
        //var direction = goalPos - transform.position;

        //rg.freezeRotation = false;
        ////rg.constraints = RigidbodyConstraints.FreezePositionY;
        //var newForce = new Vector3(direction.x, 0, direction.z) * speed;
        //rg.AddForce(newForce, ForceMode.Impulse);

        //direction.Normalize();
    }

    IEnumerator IMove()
    {
        yield return new WaitForSeconds(rand.Next((int)minWaitTime, (int)maxWaitTime+1));
        var goalPos = goalTrans.position;
        goalPos = new Vector3(goalPos.x, transform.position.y, goalPos.z);
        var direction = goalPos - transform.position;

        rg.freezeRotation = false;
        //rg.constraints = RigidbodyConstraints.FreezePositionY;
        var newForce = new Vector3(direction.x, 0, direction.z) * speed;
        rg.AddForce(newForce, ForceMode.Impulse);
    }

    void AttackPlayer()
    {
        print("AttackPlayer");
    }

    void SmartMoveToGoal()
    {
        print("SmartMoveToGoal");
    }
}
