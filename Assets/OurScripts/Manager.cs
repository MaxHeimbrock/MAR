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
    private VoiceCommand voiceCommand;
    public AudioClip whichSectionAudioFile;
    public AudioClip whichProductAudioFile;
    public AudioClip touchedSound;
    public AudioClip startDemo;
    public AudioClip smallBell;

    // States
    public enum State { FindAnchor, ReadyToStart, SectionSelection, PathNavigation, ProductSelection, ProductTracking, ProductFound, ProductTouched};
    public enum Section { Init, Dairy = 14, Snacks = 7 }; 
    public enum Product { Init, Corny, Cookie, MilkBlue, MilkGreen };
    State currentState;
    Section currentSection;
    Product currentProduct;

    // Pathfinding
    private Dijkstra dijkstra;

    // Sending to Watch
    private SendToWatchTCP sendToWatch;

    // TrackingObjects
    // order is as in enum Product: [0] Corny, [1] Keks, [2] MilkBlue, [3] MilkGreen ...
    public GameObject[] products;

    private Vector3 handPos;
    private float distanceToProduct;

    // Use this for initialization
    void Start()
    {
        sendToWatch = GetComponent<SendToWatchTCP>();

        // ToDo: Hier auskommentiert zum Debug
        //sendToWatch.StartConnection();

        voiceCommand = GetComponent<VoiceCommand>();

        dijkstra = new Dijkstra();

        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i].SetActive(false);
        }

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
        sendToWatch.SetFrequency(-1.0f);

        if (currentState == State.PathNavigation)
            GetComponent<Pathfinding>().Deactivate();

        if (currentState != State.FindAnchor)
        {
            currentState = State.FindAnchor;
            PosterFound();
        }

        currentSection = Section.Init;
        currentProduct = Product.Init;

        handPos = new Vector3(0, 0, 0);
        distanceToProduct = 0.0f;

        if (currentProduct != Product.Init)
            products[(int)currentProduct - 1].GetComponent<AudioSource>().Stop();
    }

    public void StartDemo()
    {
        if (currentState == State.ReadyToStart)
        {
            audioSource.loop = false;
            audioSource.Stop();

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
            sendToWatch.SetFrequency(distanceToProduct);
        }

        if (result < 0.07f)
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
            audioSource.loop = true;
            audioSource.clip = startDemo;
            audioSource.Play();
            GoToNextState();
        }
    }
    
    public void SetPath(Section section)
    {
        if (currentState == State.SectionSelection)
        {
            GoToNextState();
            currentSection = section;
            dijkstra.FindPathToGoal((int)currentSection);
        }
    }

    public void ArrivedAtSection()
    {
        if (currentState == State.PathNavigation)
        {
            GoToNextState();

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
                audioSource.clip = smallBell;
                audioSource.Play();
            }
            else if (currentSection == Section.Snacks && (product == Product.Cookie || product == Product.Corny))
            {
                GoToNextState();
                currentProduct = product;
                audioSource.clip = smallBell;
                audioSource.Play();
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
        products[(int)currentProduct - 1].GetComponent<AudioSource>().Stop();
        
        audioSource.clip = touchedSound;
        audioSource.Play(0);

        sendToWatch.SetFrequency(-1.0f);

        GoToNextState();
    }

    #endregion

    public void ManualStep()
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
                GetComponent<Pathfinding>().Deactivate();
                ArrivedAtSection();
                break;

            case State.ProductSelection:
                SetProduct(Product.Corny);
                break;

            case State.ProductTracking:
                ActivateProduct(Product.Corny);
                break;

            case State.ProductFound:
                ProductTouched();
                break;
        }
    }
}
