using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public GameObject[] allWaypoints;
    private int[] path;
    int activeWaypoint;

    private GameObject cam;
    private Transform camPos;

    private Manager manager;

    private bool active;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camPos = cam.GetComponent<Transform>();

        //waypoints[activeWaypoint].GetComponent<soundScript>().ActivateSound();

        allWaypoints = GameObject.FindGameObjectsWithTag("waypoint");
        activeWaypoint = 0;
        path = new int[0];

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        if (manager != null)
            Debug.Log("manager found with tag by Pathfinding");

        else
            Debug.Log("manager not found with tag by Pathfinding");
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            CheckWaypoint();
    }

    // Changes activeWaypoint and changes AudioSource to play - calls manager if arrived at last Waypoint
    void WaypointReached()
    {
        Deactivate();

        if (activeWaypoint < path.Length - 1)
        {
            activeWaypoint++;
            Activate();
        }
        else
        {
            activeWaypoint = 0;
            manager.ArrivedAtSection();
        }
    }

    // Checks if Player (Camera) is close to Waypoint - Calls WaypointReached() if reached
    void CheckWaypoint()
    {
        Vector2 camPos2D = new Vector2(camPos.position.x, camPos.position.z);

        Vector2 waypoint2D = new Vector2(allWaypoints[activeWaypoint].transform.position.x, allWaypoints[activeWaypoint].transform.position.z);

        if (Vector2.Distance(camPos2D, waypoint2D) < 0.3)
        {
            WaypointReached();
        }
    }

    // Starts Sound and Pathfinding
    void Activate()
    {
        active = true;
        allWaypoints[activeWaypoint].SetActive(true);
        allWaypoints[activeWaypoint].GetComponent<AudioSource>().Play();
    }

    // Stops Sound and Pathfinding
    void Deactivate()
    {
        active = false;
        allWaypoints[activeWaypoint].SetActive(false);
        allWaypoints[activeWaypoint].GetComponent<AudioSource>().Stop();
    }

    // This method is called in Dijkstra. Only way to activate Pathfinding
    public void SetPath(int[] path)
    {
        this.path = path;
        Activate();
    }
}
