using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

public class Dijkstra {

    public GameObject[] allWaypoints;

    private Pathfinding pathfinding;

    Transform camPos;


    // Use this for initialization - Waypoints are found by their name - Number of Waypoints is determined by Tag
    public Dijkstra () {
        allWaypoints = new GameObject[GameObject.FindGameObjectsWithTag("waypoint").Length];
        for(int i = 0;  i < allWaypoints.Length; i++)
        {
            allWaypoints[i] = GameObject.Find("Waypoint (" + i + ")");
        }
        camPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        pathfinding = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pathfinding>();
    }

    public void FindPathToGoal(int goal)
    {
        int startWaypoint = FindClosestWaypoint();
        Debug.LogWarning("Ziel: " + goal + " Start: " + startWaypoint);
        if(goal == startWaypoint)
        {
            Debug.Log("Start equals Goal");
            int[] path = { goal };
            pathfinding.SetPath(path, allWaypoints);
        }
        else
        {
            Queue<int> waitingNodes = new Queue<int>();
            waitingNodes.Enqueue(startWaypoint);
            string[] pathToNode = new string[allWaypoints.Length];
            pathToNode[startWaypoint] = startWaypoint.ToString();

            while (waitingNodes.Count > 0)
            {
                int currentWaypoint = waitingNodes.Dequeue();
                int[] neighbors = allWaypoints[currentWaypoint].GetComponent<PathNode>().neighbors;
                for (int i = 0; i < neighbors.Length; i++)
                {
                    int currentNeighbor = neighbors[i];
                    if (currentNeighbor == goal)
                    {
                        Debug.Log("Dijkstra found shortest path.");
                        pathfinding.SetPath(CreatePathArray(pathToNode[currentWaypoint] + "," + goal), allWaypoints);
                        return;
                    }
                    if (pathToNode[currentNeighbor] == null)
                    {
                        pathToNode[currentNeighbor] = pathToNode[currentWaypoint] + "," + currentNeighbor;
                        waitingNodes.Enqueue(currentNeighbor);
                    }
                }
            }
            Debug.LogError("Goal not found: " + goal);
        }
    }

    int FindClosestWaypoint()
    {
        Vector2 camPos2D = new Vector2(camPos.position.x, camPos.position.z);
        float minDistance = float.MaxValue;
        int closestWaypoint = -1;
        for (int i = 1; i < allWaypoints.Length; i++)
        {
            Vector2 waypointPos2D = new Vector2(allWaypoints[i].transform.position.x, allWaypoints[i].transform.position.z);
            float currentDistance = Vector2.Distance(camPos2D, waypointPos2D);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestWaypoint = i;
            }
            Debug.Log("Closest waypoint: " + closestWaypoint + " Distance: " + minDistance);
        }
        Debug.Log(camPos2D);
        Debug.Log(minDistance);
        Debug.Log(allWaypoints[closestWaypoint].transform.position.x + " " + allWaypoints[closestWaypoint].transform.position.y + " " + allWaypoints[closestWaypoint].transform.position.z);
        return closestWaypoint;
    }

    int[] CreatePathArray(string stringPath)
    {
        Debug.Log("Test: " + stringPath);
        String[] pathArray = stringPath.Split(',');
        int[] path = new int[pathArray.Length];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = Int32.Parse(pathArray[i]);
        }
        Debug.Log("Final Array: " + path);
        return path;
    }
}
