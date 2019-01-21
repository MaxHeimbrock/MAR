using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {


    public GameObject billboard;
    enum State { BeforePathSet, BeforeProduct, LookingForProduct};
    State currentState;
    AudioSource audioSource;

    public bool gefunden = false;
    public bool arrivedAtDestination;

    public GameObject pathFindingMax;
    public GameObject pathFindingOther;

    private bool commandIsGiven = false;
    public VoiceCommand voiceCommand;

    int posterFounded = 0;
    public AudioClip whichSectionAudioFile;
    public AudioClip whichProductAudioFile;

    public GameObject cornyProduct;
    bool playProductSound = false;
    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        currentState = State.BeforePathSet;
	}
	
	// Update is called once per frame
	void Update () {

        if (currentState == State.BeforePathSet)
        {
            if (!commandIsGiven && gefunden)
            {
                AskForInput();
            }
            if (commandIsGiven && voiceCommand.pathIsSet)
            {
                if (voiceCommand.pathIsMax)
                {
                    pathFindingMax.SetActive(true);
                }
                else
                {
                    pathFindingOther.SetActive(true);
                }
                GoToNextState();
                commandIsGiven = false;
            }

            
        }

        if (currentState == State.BeforeProduct)
        {
            if (voiceCommand.pathIsMax)
            {
                arrivedAtDestination = pathFindingMax.GetComponent<Pathfinding>().arrivedAtDestination;
            }
            else
            {
                arrivedAtDestination = pathFindingOther.GetComponent<Pathfinding>().arrivedAtDestination;
            }

            if (!commandIsGiven && arrivedAtDestination)
            {
                AskForInput();
            }
            if (commandIsGiven && voiceCommand.prouctIsSet)
            {
                if (voiceCommand.productIsMilk)
                {
                    billboard.GetComponent<TextMesh>().text = "From Manager: Milk";
                    GoToNextState();
                }
                else
                {
                    billboard.GetComponent<TextMesh>().text = "From Manager: Corny";
                    //cornyProduct.SetActive(true);
                    playProductSound = true;
                    GoToNextState();
                    
                }
            }
        }

        if (playProductSound)
        {
            billboard.GetComponent<TextMesh>().text = "Play for Corny";
            cornyProduct.GetComponent<AudioSource>().Play(0);
        }

    }

    void AskForInput()
    {
        commandIsGiven = true;
        if (currentState == State.BeforePathSet)
        {
            audioSource.clip = whichSectionAudioFile;
            audioSource.Play(0);
        }
        else if (currentState == State.BeforeProduct)
        {
            audioSource.clip = whichProductAudioFile;
            audioSource.Play(0);
        }
    }

    void SelectPath()
    {

    }


    void GoToNextState()
    {
        if(currentState == State.BeforePathSet)
        {
            currentState = State.BeforeProduct;
        }
        else if (currentState == State.BeforeProduct)
        {
            currentState = State.LookingForProduct;
        }
    }

}
