using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingManager : MonoBehaviour
{
    public bool gefunden = false;
    public GameObject pathFinding;
    
    TextMesh textMesh;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {        


        if (gefunden)
            pathFinding.SetActive(true);


    }
}
