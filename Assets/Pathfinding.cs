using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public GameObject[] waypoints;

    public GameObject cam;
    private Transform camPos;

    int activeWaypoint = 0;
    public bool arrivedAtDestination = false;

	// Use this for initialization
	void Start () {
        camPos = cam.GetComponent<Transform>();
        waypoints[activeWaypoint].SetActive(true);
        waypoints[activeWaypoint].GetComponent<soundScript>().ActivateSound();
    }
	
	// Update is called once per frame
	void Update () {
        CheckWaypoint();
	}

    void WaypointReached()
    {
        waypoints[activeWaypoint].GetComponent<soundScript>().DeativateSound();
        waypoints[activeWaypoint].SetActive(false);

        if (activeWaypoint < waypoints.Length - 1)
        {
            activeWaypoint++;
            waypoints[activeWaypoint].GetComponent<soundScript>().ActivateSound();
            waypoints[activeWaypoint].SetActive(true);
        }
        else
        {
            arrivedAtDestination = true;
        }
        
    }

    void CheckWaypoint()
    {
        Vector2 camPos2D = new Vector2(camPos.position.x, camPos.position.z);

        Vector2 waypoint2D = new Vector2(waypoints[activeWaypoint].transform.position.x, waypoints[activeWaypoint].transform.position.z);

        if (Vector2.Distance(camPos2D, waypoint2D) < 0.3)
            WaypointReached();
    }       
}
