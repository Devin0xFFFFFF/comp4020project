using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Speech Recognizer Controller (Without dialog)
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    /// </summary>
    public class SpeechRecognizerController : MonoBehaviour
    {

        //Callbacks
        public UnityEvent OnReady;              //Callback when microphone standby.
        public UnityEvent OnBegin;              //Callback when microphone begin speech recognization.

        [Serializable] public class ResultHandler : UnityEvent<string[]> { }    //recognization words
        public ResultHandler OnResult;          //Callback when recognization success

        [Serializable] public class ErrorHandler : UnityEvent<string> { }       //error state messate
        public ErrorHandler OnError;            //Callback when recognization fail

#region Properties and Local values Section

        //Properties
        private static bool isSupportedRecognizer = false;  //Cached supported Speech Recognizer (Because Recognizer shares one, it is static).
        private static bool isSupportedChecked = false;     //Already checked (Because Recognizer shares one, it is static).

        public bool IsSupportedRecognizer {
            get {
                if (!isSupportedChecked)
                {
#if UNITY_EDITOR
                    isSupportedRecognizer = true;       //For Editor
#elif UNITY_ANDROID
                    isSupportedRecognizer = AndroidPlugin.IsSupportedSpeechRecognizer();
#endif
                    isSupportedChecked = true;
                }
                return isSupportedRecognizer;
            }
        }

        //Local Values
        private bool canceled = false;  //Interrupted recognizer flag (With the time lag of message reception, prevents callback events from occurring)

#endregion

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Start Speech Recognizer
        public void StartRecognizer()
        {
            if (!IsSupportedRecognizer)
                return;

            canceled = false;
#if UNITY_EDITOR
            Debug.Log("SpeechRecognizerController.StartRecognizer called");
#elif UNITY_ANDROID
            AndroidPlugin.StartSpeechRecognizer(
                gameObject.name,
                "ReceiveResult",
                "ReceiveError",
                "ReceiveReady",
                "ReceiveBegin");
#endif
        }


        //Microphone standby state
        private void ReceiveReady(string message)
        {
            if (canceled)
                return;

            if (OnReady != null)
                OnReady.Invoke();
        }

        //Microphone begin speech recognization state
        private void ReceiveBegin(string message)
        {
            if (canceled)
                return;

            if (OnBegin != null)
                OnBegin.Invoke();
        }

        //Receive the result when speech recognition succeed.
        private void ReceiveResult(string message)
        {
            if (canceled)
                return;

            if (string.IsNullOrEmpty(message))
                return;

            if (OnResult != null)
                OnResult.Invoke(message.Split('\n'));
        }

        //Receive the error when speech recognition fail.
        private void ReceiveError(string message)
        {
            if (canceled)
                return;

            if (OnError != null)
                OnError.Invoke(message);
        }


        //Interrupt speech recognition
        public void StopRecognizer()
        {
            canceled = true;
#if UNITY_EDITOR
            Debug.Log("SpeechRecognizerController.StopRecognizer called");
#elif UNITY_ANDROID
            AndroidPlugin.ReleaseSpeechRecognizer();
#endif
        }
    }
}
