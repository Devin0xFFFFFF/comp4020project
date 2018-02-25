using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FantomLib;

//Text To Speech demo using controllers
public class TextToSpeechTest2 : MonoBehaviour {

    public Text displayText;
    public Text statusText;
    public Animator statusAnimator;

    public Text speedText;
    public Text pitchText;

    //Running message
    public string startMessage = "Speaking";    //Message when speech start.        //発声中
    public string doneMessage = "Finished";     //Message when speech finished.     //発声終了
    public string stopMessage = "Interrupted";  //Message when speech interrupted.  //発声中断

    //Initialization/error message
    public string ttsAvailableMessage = "Text To Speech is available.";             //Message when TTS available             //テキスト読み上げが利用できます。
    public string ttsInitializationError = "Failed to initialize Text To Speech.";  //Message when TTS initialization error  //テキスト読み上げの初期化に失敗しました
    public string ttsLocaleError = "It is a language that can not be used.";        //Message when TTS locale error          //利用できない言語です。

    //Register 'TextToSpeechController.StartSpeech' in the inspector.
    [Serializable] public class TTSStartHandler : UnityEvent<string> { }
    public TTSStartHandler OnTTSStart;

    //Register 'MultiLineTextDialogController.Show' in the inspector.
    [Serializable] public class TextEditHandler : UnityEvent<string> { }
    public TextEditHandler OnTextEdit;


    // Use this for initialization
    private void Start () {

    }
    
    // Update is called once per frame
    //private void Update () {
        
    //}


    //==========================================================
    //Display and edit text string
    
    //Display text string (and for reading)
    public void DisplayText(string message, bool add = false)
    {
        if (displayText != null)
        {
            if (add)
                displayText.text += message;
            else
                displayText.text = message;
        }
    }

    //Display status message
    public void DisplayStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    //Display speech speed
    public void DisplaySpeed(float speed)
    {
        if (speedText != null)
            speedText.text = string.Format("Speed : {0:F2}", speed);
    }

    //Display voice pitch
    public void DisplayPitch(float pitch)
    {
        if (pitchText != null)
            pitchText.text = string.Format("Pitch : {0:F2}", pitch);
    }

    //Call the text(reading) edit dialog
    public void EditText()
    {
        if (OnTextEdit != null && !string.IsNullOrEmpty(displayText.text))
            OnTextEdit.Invoke(displayText.text);
    }

    //Callback handler for text edit dialog result
    public void OnEditText(string text)
    {
        DisplayText(text.Trim());
    }


    //==========================================================
    //Example Text To Speech (Callback handlers)
    
    //TextToSpeechController.StartSpeech call
    public void StartTTS()
    {
        if (OnTTSStart != null)
            OnTTSStart.Invoke(displayText.text);
    }


    //Receive status message from callback
    public void OnStatus(string message)
    {
        DisplayStatus(message);

        if (message.StartsWith("SUCCESS_INIT"))
            DisplayText("\n" + ttsAvailableMessage, true);
        else if (message.StartsWith("ERROR_LOCALE_NOT_AVAILABLE"))
            DisplayText("\n" + ttsInitializationError + "\n" + ttsLocaleError, true);
        else if (message.StartsWith("ERROR_INIT"))
            DisplayText("\n" + ttsInitializationError, true);
        else
            DisplayText("\n" + message, true);

        TextToSpeechController ttsController = FindObjectOfType<TextToSpeechController>();
        if (ttsController != null)
        {
            DisplayText("\nInitializeStatus = " + ttsController.InitializeStatus, true);
            DisplayText("\nIsInitializeSuccess = " + ttsController.IsInitializeSuccess, true);
            //DisplayText("\n" + ttsController.SaveKey + " : " + PlayerPrefs.GetString(ttsController.SaveKey), true);    //json
        }
    }

    //Callback handler for start speaking
    public void OnStart()
    {
        if (statusAnimator != null)
            statusAnimator.SetTrigger("blink");

        DisplayStatus(startMessage);
    }

    //Callback handler for finish speaking
    public void OnDone()
    {
        if (statusAnimator != null)
            statusAnimator.SetTrigger("stop");

        DisplayStatus(doneMessage);
    }

    //Callback handler for interrupt speaking
    public void OnStop(string message)
    {
        if (statusAnimator != null)
            statusAnimator.SetTrigger("stop");

        DisplayStatus(stopMessage);
    }

}
