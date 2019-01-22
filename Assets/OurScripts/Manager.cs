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
    public AudioClip distanceSound;

    // States
    public enum State { FindAnchor, ReadyToStart, SectionSelection, PathNavigation, ProductSelection, ProductTracking, ProductFound, ProductTouched};
    public enum Section { Init, Dairy, Snacks };
    public enum Product { Init, Corny, Cookie, MilkBlue, MilkGreen };
    State currentState;
    Section currentSection;
    Product currentProduct;

    // Pathfinding
    // order is as in enum Section: [0] Dairy, [1] Snacks
    public Pathfinding[] paths;

    // TrackingObjects
    // order is as in enum Product: [0] Corny, [1] Keks, [2] MilkBlue, [3] MilkGreen ...
    public GameObject[] products;

    private Vector3 handPos;
    private float distanceToProduct;

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

        SetText(currentState + "\n" + currentSection + "\n" + currentProduct + "\n" + distanceToProduct);

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

            case State.ProductFound:
                distanceToProduct = HandProductDistance();
                break;

            case State.ProductTouched:
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
        if (currentState != State.FindAnchor)
            currentState = State.ReadyToStart;

        currentSection = Section.Init;
        currentProduct = Product.Init;
        currentPathfinding = null;

        // resets all paths
        for (int i = 0; i < paths.Length; i++)
            paths[i].ResetPathfinding();

        handPos = new Vector3(0, 0, 0);
        distanceToProduct = 0.0f;

        if (currentState == State.ProductTracking)
        {
            products[(int)currentProduct - 1].GetComponent<AudioSource>().Stop();
        }
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

    public void SetHandPos(Vector3 handPos)
    {
        this.handPos = handPos;
    }

    public float HandProductDistance()
    {
        float result = 0;
        
        if (currentProduct != Product.Init && currentState == State.ProductFound)
        {
            result = Vector3.Distance(handPos, products[(int)currentProduct - 1].transform.position);            
        }

        if (result < 0.09f)
        {
            ProductTouched();
        }

        return result;
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
            if (currentSection == Section.Dairy && (product == Product.MilkBlue || product == Product.MilkGreen))
            {
                GoToNextState();
                currentProduct = product;
            }
            else if (currentSection == Section.Snacks && (product == Product.Cookie || product == Product.Corny))
            {
                GoToNextState();
                currentProduct = product;
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
            products[(int)currentProduct - 1].GetComponent<AudioSource>().Play();
            
            GoToNextState();
        }
    }    

    public void ProductTouched()
    {
        // TODO

        GoToNextState();
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
                ActivateProduct(Product.Corny);
                break;

            case State.ProductTouched:
                ProductTouched();
                break;
        }
    }
}
