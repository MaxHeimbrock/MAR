using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundScript : MonoBehaviour {

    AudioSource audioSource;

    private bool play = false;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (play)
            audioSource.Play(0);
    }

    public void ActivateSound()
    {
        play = true;
    }

    public void DeativateSound()
    {
        play = false;
    }
}
