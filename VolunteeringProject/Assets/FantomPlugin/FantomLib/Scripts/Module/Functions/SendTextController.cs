using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FantomLib
{
    /// <summary>
    /// Send Text Controller
    /// 
    ///･Send text with Chooser (application selection widget).
    ///･If you send it to Twitter application etc., you can share the text.
    /// 
    ///・Chooser（アプリ選択ウィジェット）でテキストの送信をする。
    ///・Twitterなどに送れば、テキストをシェアできる。
    /// </summary>
    public class SendTextController : MonoBehaviour
    {

        public Text targetText;         //UI Text (When using 'Send ()')
        public string chooserTitle = "Select the application to share this text.";  //Title of chooser.


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        
        //Send the text of targetText.
        public void Send()
        {
            if (targetText == null || string.IsNullOrEmpty(targetText.text))
                return;
#if UNITY_EDITOR
            Debug.Log("SendTextController.Send : " + targetText.text);
#elif UNITY_ANDROID
            AndroidPlugin.StartActionWithChooser("android.intent.action.SEND", "android.intent.extra.TEXT", targetText.text, "text/plain", chooserTitle);
#endif
        }

        //Send text dynamically (It does not affect 'UI-Text').
        public void Send(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
#if UNITY_EDITOR
            Debug.Log("SendTextController.Send : " + text);
#elif UNITY_ANDROID
            AndroidPlugin.StartActionWithChooser("android.intent.action.SEND", "android.intent.extra.TEXT", text, "text/plain", chooserTitle);
#endif
        }
    }
}
