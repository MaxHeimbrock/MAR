using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : MonoBehaviour {

    public GameObject cam;

    private Transform camTransform;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        camTransform = cam.GetComponent<Transform>();

        audioSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Physics.Raycast(camTransform.position, camTransform.rotation.eulerAngles, 1.0f))
        {
            audioSource.Play();

            Debug.Log("collision");
        }                
	}
}
