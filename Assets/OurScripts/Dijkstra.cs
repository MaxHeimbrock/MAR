using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

public class Dijkstra {

    private GameObject[] allWaypoints;

    private Pathfinding pathfinding;

    Transform camPos;


    // Use this for initialization
    public Dijkstra () {
        allWaypoints = GameObject.FindGameObjectsWithTag("waypoint");
        camPos = GameObject.Find("Sphere").GetComponent<Transform>(); //----------------------------------------------------

        pathfinding = GameObject.Find("PathFinding").GetComponent<Pathfinding>();

    }


    public void FindPathToGoal(int goal)
    {
        int startWaypoint = FindClosestWaypoint();
        if(goal == startWaypoint)
        {
            Debug.Log("Start equals Goal");
            int[] path = { goal };
            pathfinding.SetPath(path);
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
                        pathfinding.SetPath(CreatePathArray(pathToNode[currentWaypoint] + goal));
                        return;
                    }
                    if (pathToNode[currentNeighbor] == null)
                    {
                        pathToNode[currentNeighbor] = pathToNode[currentWaypoint] + currentNeighbor;
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
        for (int i = 0; i < allWaypoints.Length; i++)
        {
            Vector2 waypointPos2D = new Vector2(allWaypoints[i].transform.position.x, allWaypoints[i].transform.position.z);
            float currentDistance = Vector2.Distance(camPos2D, waypointPos2D);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestWaypoint = i;
            }
        }
        return closestWaypoint;
    }

    int[] CreatePathArray(string stringPath)
    {
        int[] path = new int[stringPath.Length];
        for(int i = 0; i < path.Length; i++)
        {
            path[i] = Int32.Parse(stringPath[i].ToString());
        }
        return path;
    }
}
