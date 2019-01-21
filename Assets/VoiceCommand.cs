using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommand : MonoBehaviour
{

    public GameObject billboard;
    Pathfinding pathfinding;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public bool pathIsSet = false;
    public bool pathIsMax = false;
    public bool prouctIsSet = false;
    public bool productIsMilk = false;
    //public GameObject drinkSection;
    //public GameObject foodSection;

    private void Awake()
    {
        //pathfinding = GetComponent<Pathfinding>();
    }

    void Start()
    {
        actions.Add("Max", SetMaxAsPath);
        actions.Add("Other", SetOtherAsPath);
        actions.Add("milk", SetMilkAsProduct);
        actions.Add("corny", SetCornyAsProduct);
        

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizeSpeech;
        keywordRecognizer.Start();
    }

    void RecognizeSpeech(PhraseRecognizedEventArgs speech)
    {
        print(speech.text);
        actions[speech.text].Invoke();
    }

    void SetMaxAsPath()
    {
        billboard.GetComponent<TextMesh>().text = "Max";
        pathIsSet = true;
        pathIsMax = true;
    }
    void SetOtherAsPath()
    {
        billboard.GetComponent<TextMesh>().text = "Other";
        pathIsSet = true;
    }

    void SetMilkAsProduct()
    {
        billboard.GetComponent<TextMesh>().text = "Milk";
        prouctIsSet = true;
        productIsMilk = true;
    }

    void SetCornyAsProduct()
    {
        billboard.GetComponent<TextMesh>().text = "Corny";
        prouctIsSet = true;
    }


}