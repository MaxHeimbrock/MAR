using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : MonoBehaviour {

    public GameObject cam;

    private Transform camTransform;
    AudioSource audioSource;

    private RaycastHit hit;

    // Use this for initialization
    void Start () {
        camTransform = cam.GetComponent<Transform>();

        audioSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
        //if(Physics.Raycast(camTransform.position, camTransform.rotation.eulerAngles, out hit, Mathf.Infinity))
        //{
        //    if(hit.distance < 1.0)
        //    {
        //        audioSource.Play();
        //        Debug.DrawRay(camTransform.position, camTransform.rotation.eulerAngles * hit.distance, Color.yellow);
        //        Debug.Log("Close Hit with " + hit.collider.gameObject.name);
        //
        //    } else
        //    {
        //        Debug.DrawRay(camTransform.position, camTransform.rotation.eulerAngles * hit.distance, Color.white);
        //        Debug.Log("Far Hit with " + hit.collider.gameObject.name);
        //    }
        //
        //    
        //}

        //if (Physics.Raycast(camTransform.position, camTransform.rotation.eulerAngles, 1.0f))
        //{
        //    audioSource.Play();

        //    Debug.Log("collision");
        //}
    }
}
