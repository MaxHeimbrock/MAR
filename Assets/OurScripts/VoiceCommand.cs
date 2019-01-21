﻿using System.Collections;
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

    private Manager manager;

    public bool pathIsSet = false;
    public bool pathIsMax = false;
    public bool prouctIsSet = false;
    public bool productIsMilk = false;
    //public GameObject drinkSection;
    //public GameObject foodSection;

    private void Awake()
    {
        //pathfinding = GetComponent<Pathfinding>();

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        if (manager != null)
            Debug.Log("manager found with tag by VoiceCommand");

        else
            Debug.Log("manager not found with tag by VoiceCommand");
    }

    void Start()
    {
        actions.Add("milk", SetMilkAsProduct);
        actions.Add("corny", SetCornyAsProduct);

        actions.Add("Start Demo", StartDemo);
        actions.Add("Dairy Products", SetPathToDairyProducts);
        actions.Add("Snacks", SetPathToSnacks);
        actions.Add("Reset Demo", ResetDemo);

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

    void SetMilkAsProduct()
    {
        manager.SetMilkAsProduct();
    }

    void SetCornyAsProduct()
    {
        billboard.GetComponent<TextMesh>().text = "Corny";
        prouctIsSet = true;
    }

    void StartDemo()
    {
        manager.StartDemo();
    }

    void SetPathToDairyProducts()
    {
        manager.SetPathToDairyProducts();
    }

    void SetPathToSnacks()
    {
        manager.SetPathToSnacks();
    }

    // Resets Manager and Pathfinding Objects with Tag
    void ResetDemo()
    {
        manager.ResetDemo();

        GameObject[] paths = GameObject.FindGameObjectsWithTag("Pathfinding");

        foreach (GameObject path in paths)
        {
            path.GetComponent<Pathfinding>().ResetPathfinding();
            Debug.Log("Path Reset");
        }
    }
}