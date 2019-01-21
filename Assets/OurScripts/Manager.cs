using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    // For text output
    public GameObject billboard;
    private TextMesh text;

    // For Audio I/O
    AudioSource audioSource;
    public VoiceCommand voiceCommand;
    private Pathfinding currentPathfinding;
    public AudioClip whichSectionAudioFile;
    public AudioClip whichProductAudioFile;

    // States
    enum State { Init, ReadyToStart, SectionSelection, PathNavigation, ProductSelection, ProductTracking};
    enum Section { Init, Dairy, Snacks};
    enum Product { Init, Corny, Chocolate, Milk, Butter};
    State currentState;
    Section currentSection;
    Product currentProduct;
    
    // Pathfinding
    // order is: [0] dairy, [1] snacks
    public Pathfinding[] paths;

    public GameObject cornyProduct;

    // Use this for initialization
    void Start () {

        audioSource = gameObject.GetComponent<AudioSource>();

        text = billboard.GetComponent<TextMesh>();

        ResetDemo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ManualStep();
            Debug.Log("Manual step");
        }

        if (Input.GetKeyDown("r"))
        {
            ResetDemo();
            Debug.Log("Reset");
        }

        switch (currentState)
        {
            case State.Init:
                break;

            case State.ReadyToStart:
                break;

            case State.SectionSelection:
                break;

            case State.PathNavigation:
                break;

            case State.ProductSelection:
                break;

            case State.ProductTracking:
                break;
        }
    }

    #region Helping functions

    void GoToNextState()
    {
        currentState++;
    }

    private void SetText(string s)
    {
        text.text = s;
    }

    public void ResetDemo()
    {
        currentState = State.Init;
        currentSection = Section.Init;
        currentProduct = Product.Init;

        SetText("Go to anchor first");

        currentPathfinding = null;

        // resets all paths
        for (int i = 0; i < paths.Length; i++)
            paths[i].ResetPathfinding();
    }

    public void StartDemo()
    {
        if (currentState == State.ReadyToStart)
        {
            GoToNextState();

            SetText("Demo Started");

            audioSource.clip = whichSectionAudioFile;
            audioSource.Play(0);
        }
    }

    #endregion 

    #region State Changes from VoiceCommand or Vuforia

    public void PosterGefunden()
    {
        if (currentState == State.Init)
        {
            GoToNextState();

            SetText("Anchor found\nSay: >>Start Demo<<");
        }
    }

    public void SetPathToDairyProducts()
    {
        if (currentState == State.SectionSelection)
        {
            GoToNextState();

            currentSection = Section.Dairy;

            currentPathfinding = paths[0];
            currentPathfinding.Activate();

            SetText("Navigation to Dairy Section");
        }
    }

    public void SetPathToSnacks()
    {
        if (currentState == State.SectionSelection)
        {
            GoToNextState();

            currentSection = Section.Snacks;

            currentPathfinding = paths[1];
            currentPathfinding.Activate();

            SetText("Navigation to Snack Section");
        }
    }


    public void ArrivedAtSection()
    {
        if (currentState == State.PathNavigation)
        {
            GoToNextState();
            currentPathfinding.ResetPathfinding();

            audioSource.clip = whichProductAudioFile;
            audioSource.Play(0);
        }
    }

    public void SetMilkAsProduct()
    {
        if (currentState == State.ProductSelection && currentSection == Section.Dairy)
        {
            GoToNextState();
            currentProduct = Product.Milk;
        }
    }

    public void SetCornyAsProduct()
    {
        if (currentState == State.ProductSelection && currentSection == Section.Snacks)
        {
            GoToNextState();
            currentProduct = Product.Corny;
        }
    }

    #endregion

    private void ManualStep()
    {
        switch (currentState)
        {
            case State.Init:
                PosterGefunden();
                break;

            case State.ReadyToStart:
                StartDemo();
                break;

            case State.SectionSelection:
                SetPathToSnacks();
                break;

            case State.PathNavigation:
                ArrivedAtSection();
                break;

            case State.ProductSelection:
                break;

            case State.ProductTracking:
                break;
        }
    }
}
