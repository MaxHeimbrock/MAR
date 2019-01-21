using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public GameObject[] waypoints;
    int activeWaypoint;

    private GameObject cam;
    private Transform camPos;

    private Manager manager;

    private bool active = false;

    public bool arrivedAtDestination = false;

	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camPos = cam.GetComponent<Transform>();

        //waypoints[activeWaypoint].GetComponent<soundScript>().ActivateSound();

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        if (manager != null)
            Debug.Log("manager found with tag by Pathfinding");

        else
            Debug.Log("manager not found with tag by Pathfinding");

        ResetPathfinding();
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
            CheckWaypoint();
	}

    // Changes activeWaypoint and changes AudioSource to play - calls manager if arrived at last Waypoint
    void WaypointReached()
    {
        waypoints[activeWaypoint].GetComponent<AudioSource>().Stop();
        waypoints[activeWaypoint].SetActive(false);

        if (activeWaypoint < waypoints.Length - 1)
        {
            activeWaypoint++;
            waypoints[activeWaypoint].GetComponent<AudioSource>().Stop();
            waypoints[activeWaypoint].SetActive(true);
        }
        else
        {
            manager.ArrivedAtSection();
        }        
    }

    // Checks if Player (Camera) is close to Waypoint - Calls WaypointReached() if reached
    void CheckWaypoint()
    {
        Vector2 camPos2D = new Vector2(camPos.position.x, camPos.position.z);

        Vector2 waypoint2D = new Vector2(waypoints[activeWaypoint].transform.position.x, waypoints[activeWaypoint].transform.position.z);

        if (Vector2.Distance(camPos2D, waypoint2D) < 0.3)
        {
            WaypointReached();
        }
    }
    
    // Deactivates and 
    public void ResetPathfinding()
    {
        Deactivate();

        activeWaypoint = 0;
    }

    // Starts Sound and Pathfinding
    public void Activate()
    {
        active = true;
        waypoints[activeWaypoint].SetActive(true);
        waypoints[activeWaypoint].GetComponent<AudioSource>().Play();
    }

    // Stops Sound and Pathfinding
    public void Deactivate()
    {
        active = false;
        waypoints[activeWaypoint].SetActive(false);
        waypoints[activeWaypoint].GetComponent<AudioSource>().Stop();
    }
}
