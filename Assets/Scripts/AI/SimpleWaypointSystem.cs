using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleWaypointSystem : MonoBehaviour
{
    [SerializeField] private float deadZone = 2f;
    private GameObject[] waypoints;
	// Use this for initialization
    private int waypointIndex = 0;

    private Rigidbody rg;

	void Start ()
	{
	    rg = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	   
	}


    public void UpdateWaypointPositions()
    {
        waypoints = GameObject.FindGameObjectsWithTag(Constants.WAYPOINT_TAG);
        if (waypoints != null && waypoints.Length > 0)
        {
            waypoints = waypoints.OrderBy(pos => Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(pos.transform.position.x, 0,
                    pos.transform.position.z))).ToArray();

           
        }
        waypointIndex = 0;

    }


    public void Play(float speed, bool isSmart = false)
    {
        var waypoint = waypoints[waypointIndex];
        float minDistance = 100f;
        for (int i = 0; i < waypoints.Length; i++)
        {
            var dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(waypoints[i].transform.position.x, 0,
                    waypoints[i].transform.position.z));
            if (AtDesiredPosition(waypoints[i].transform.position, transform.position, minDistance))
            {
                minDistance = dist;
                waypointIndex = i;
            }
        }

        if (waypointIndex + 1 >= waypoints.Length - 1)
        {
            waypoint = waypoints[waypointIndex];
        }
        else
        {
            waypoint = waypoints[waypointIndex + 1];
        }
       
        var direction = waypoint.transform.position - transform.position;

        //check our distance to the current waypoint, Are we near enough?
        if (direction.magnitude < deadZone)
        {
            if (waypointIndex < waypoints.Length - 1) //switch to the nex waypoint if exists
            {
                waypointIndex++;
                waypoint = waypoints[waypointIndex];
                direction = waypoint.transform.position - transform.position;

                direction = new Vector3(direction.x, 0, direction.z);
               
                print("bot direction: " + direction);

                if (!isSmart)
                {
                    direction.Normalize();
                    print("add force");
                    rg.AddForce(direction * speed, ForceMode.Impulse);
                }
                    
                else
                {
                    //rg.velocity = direction.normalized * speed;
                    direction = waypoint.transform.position - transform.position;
                    rg.AddForce(direction.normalized * speed, ForceMode.Impulse);
                    //AddForceSmart(direction.normalized * speed);
                }
            }
            else //begin from new if we are already on the last waypoint
            {
                //waypointIndex = 0;
                direction = waypoint.transform.position - transform.position;
                rg.AddForce(direction.normalized * speed, ForceMode.Impulse);
                //rg.velocity = direction.normalized;
                //rg.AddForce(direction.normalized);
            }
        }
        else
        {
            print("error");
        }
     
       
        //rg.velocity = new Vector3(direction.x * speed, 0, direction.z * speed);
    }


    private void AddForceSmart(Vector3 force)
    {
        print("AddForceAtPosition");
        //rg.AddForceAtPosition(force, transform.position);
    }
   

    private bool AtDesiredPosition(Vector3 pos1, Vector3 pos2, float checkDist)
    {
        var distance = Vector3.Distance(new Vector3(pos1.x, 0, pos1.z), new Vector3(pos2.x, 0, pos2.z));
        if (distance <= checkDist)
        {
            return true;
        }
        return false;
    }


}
