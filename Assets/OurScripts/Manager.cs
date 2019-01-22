using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

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
    public enum State { FindAnchor, ReadyToStart, SectionSelection, PathNavigation, ProductSelection, ProductTracking, ProductFound};
    public enum Section { Init, Dairy, Snacks };
    public enum Product { Init, Corny, Chocolate, Milk, Butter };
    State currentState;
    Section currentSection;
    Product currentProduct;

    // Pathfinding
    // order is: [0] Dairy, [1] Snacks
    public Pathfinding[] paths;

    // TrackingObjects
    // order is: [0] Corny, [1] Milk ...
    public GameObject[] products;

    // Use this for initialization
    void Start()
    {
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

        SetText("Current state: " + currentState + "\nCurrent Section: " + currentSection + "\nCurrent Product. " + currentProduct);

        switch (currentState)
        {
            case State.FindAnchor:
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
        currentState = State.FindAnchor;
        currentSection = Section.Init;
        currentProduct = Product.Init;
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

            audioSource.clip = whichSectionAudioFile;
            audioSource.Play(0);
        }
    }

    #endregion 

    #region State Changes from VoiceCommand or Vuforia

    public void PosterFound()
    {
        if (currentState == State.FindAnchor)
        {
            GoToNextState();
        }
    }
    
    public void SetPath(Section section)
    {
        if (currentState == State.SectionSelection)
        {
            switch (section)
            {
                case Section.Dairy: 
                    
                    GoToNextState();
                    currentSection = Section.Dairy;
                    currentPathfinding = paths[0];
                    currentPathfinding.Activate();
                    break;

                case Section.Snacks:

                    GoToNextState();
                    currentSection = Section.Snacks;
                    currentPathfinding = paths[1];
                    currentPathfinding.Activate();
                    break;
            }
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

    public void SetProduct(Product product)
    {
        if (currentState == State.ProductSelection)
        {
            if (currentSection == Section.Dairy && product == Product.Milk)
            {
                GoToNextState();
                currentProduct = Product.Milk;
            }
            else if (currentSection == Section.Snacks && product == Product.Corny)
            {
                GoToNextState();
                currentProduct = Product.Corny;
            }
            else
            {
                // TODO: Voice output
                Debug.Log("Wrong Section");
            }
        }
    }

    public void ActivateProduct(Product product)
    {
        if (currentState == State.ProductTracking && product == currentProduct)
        {
            switch (product)
            {
                case Product.Corny:
                    products[0].GetComponent<AudioSource>().Play();
                    break;

                case Product.Milk:
                    break;
            }

            GoToNextState();
        }
    }

    #endregion

    private void ManualStep()
    {
        switch (currentState)
        {
            case State.FindAnchor:
                PosterFound();
                break;

            case State.ReadyToStart:
                StartDemo();
                break;

            case State.SectionSelection:
                SetPath(Section.Snacks);
                break;

            case State.PathNavigation:
                ArrivedAtSection();
                break;

            case State.ProductSelection:
                SetProduct(Product.Corny);
                break;

            case State.ProductTracking:
                break;
        }
    }
}
