﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommand : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private Manager manager;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();
    }

    void Start()
    {
        #region dairy products
        actions.Add("Dairy products", SetPathToDairyProducts);
        actions.Add("I need milk", SetPathToDairyProducts);
        actions.Add("I want milk", SetPathToDairyProducts);
        actions.Add("buy milk", SetPathToDairyProducts);
        actions.Add("I want to buy milk", SetPathToDairyProducts);
        actions.Add("I am looking for milk", SetPathToDairyProducts);
        #endregion

        #region snacks
        actions.Add("I need snacks", SetPathToSnacks);
        actions.Add("Snacks", SetPathToSnacks);
        actions.Add("I want snacks", SetPathToSnacks);
        actions.Add("I want to buy snacks", SetPathToSnacks);
        actions.Add("I want to buy corny", SetPathToSnacks);
        actions.Add("I want to buy cookies", SetPathToSnacks);
        #endregion
        
        actions.Add("blue milk", SetMilkBlueAsProduct);
        actions.Add("Unser Land Milch", SetMilkBlueAsProduct);
        actions.Add("milk blue", SetMilkBlueAsProduct);
        actions.Add("green milk", SetMilkGreenAsProduct);
        actions.Add("milk green", SetMilkGreenAsProduct);
        actions.Add("corny", SetCornyAsProduct);
        actions.Add("cookies", SetCookieAsProduct);

        actions.Add("Start Demo", manager.StartDemo);

        actions.Add("Reset Demo", manager.ResetDemo);
        actions.Add("Restart Demo", manager.ResetDemo);

        actions.Add("manual step", manager.ManualStep);
        actions.Add("step", manager.ManualStep);
        actions.Add("forward", manager.ManualStep);
        actions.Add("next", manager.ManualStep);

        actions.Add("yes", manager.GoToCashierWithYes);
        actions.Add("no", manager.ContinueShopping);

        actions.Add("Go to Cashier", manager.GoToCashier);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizeSpeech;
        keywordRecognizer.Start();
    }

    void RecognizeSpeech(PhraseRecognizedEventArgs speech)
    {
        print(speech.text);
        actions[speech.text].Invoke();
    }

    void SetMilkBlueAsProduct()
    {
        manager.SetProduct(Manager.Product.MilkBlue);
    }

    void SetMilkGreenAsProduct()
    {
        manager.SetProduct(Manager.Product.MilkGreen);
    }

    void SetCornyAsProduct()
    {
        manager.SetProduct(Manager.Product.Corny);
    }

    void SetCookieAsProduct()
    {
        manager.SetProduct(Manager.Product.Cookie);
    }

    void SetPathToDairyProducts()
    {
        manager.SetPath(Manager.Section.Dairy);
    }

    void SetPathToSnacks()
    {
        manager.SetPath(Manager.Section.Snacks);
    }
}