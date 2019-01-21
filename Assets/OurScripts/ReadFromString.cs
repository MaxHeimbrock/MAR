using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadFromString : MonoBehaviour {

    public bool read = false;
    TextToSpeech textToSpeech;

    void Start()
    {
        textToSpeech = gameObject.GetComponent<TextToSpeech>();
        textToSpeech.Voice = TextToSpeechVoice.Mark;
        //textToSpeech.SpeakText("Welcome to the Holographic App ! You can use Gaze, Gesture and Voice Command to interact with it!");
    }

    // Update is called once per frame
    void Update()
    {
        if(read)
        {
            textToSpeech.SpeakText("yoloooooo");
        }
    }
}
