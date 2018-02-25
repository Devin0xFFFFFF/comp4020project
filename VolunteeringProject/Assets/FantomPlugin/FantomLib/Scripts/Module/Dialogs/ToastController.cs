using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Toast Controller
    /// </summary>
    public class ToastController : MonoBehaviour
    {
        //Inspector Settings
        public string message = "Message";      //Message to be displayed on Toast.
        public bool longDuration = false;       //Display time is long.


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Show Toast with local message
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("ToastController.Show called : " + message);
#elif UNITY_ANDROID
            AndroidPlugin.ShowToast(message, longDuration);
#endif
        }

        //Set message dynamically and show dialog (current message will be overwritten)
        public void Show(string message)
        {
            this.message = message;
            Show();
        }

        //Set message and longDuration dynamically, and show dialog (current message will be overwritten)
        public void Show(string message, bool longDuration)
        {
            this.message = message;
            this.longDuration = longDuration;
            Show();
        }


        //Force close Toast
        public void Cancel()
        {
#if UNITY_EDITOR
            Debug.Log("ToastController.Cancel called");
#elif UNITY_ANDROID
            AndroidPlugin.CancelToast();
#endif
        }
    }
}
