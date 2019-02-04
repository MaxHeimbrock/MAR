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

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();
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

        Vector2 waypoint2D = new Vector2(allWaypoints[path[activeWaypoint]].transform.position.x, allWaypoints[path[activeWaypoint]].transform.position.z);

        if (Vector2.Distance(camPos2D, waypoint2D) < 10)
        {
            WaypointReached();
        }
    }

    // Starts Sound and Pathfinding
    void Activate()
    {
        active = true;
        allWaypoints[path[activeWaypoint]].SetActive(true);
        MeshRenderer[] renderers =  allWaypoints[path[activeWaypoint]].GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in renderers)
            mr.enabled = true;
        allWaypoints[path[activeWaypoint]].GetComponent<AudioSource>().Play();
    }

    // Stops Sound and Pathfinding
    public void Deactivate()
    {
        active = false;
        allWaypoints[path[activeWaypoint]].SetActive(false);
        MeshRenderer[] renderers = allWaypoints[path[activeWaypoint]].GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in renderers)
            mr.enabled = false;
        allWaypoints[path[activeWaypoint]].GetComponent<AudioSource>().Stop();
    }

    // This method is called in Dijkstra. Only way to activate Pathfinding
    public void SetPath(int[] path, GameObject[] waypoints)
    {
        this.path = path;
        allWaypoints = waypoints;
        activeWaypoint = 0;
        for(int i = 0; i < path.Length; i++)
        {
            Debug.Log(path[i]);
        }
        Activate();
    }
}
