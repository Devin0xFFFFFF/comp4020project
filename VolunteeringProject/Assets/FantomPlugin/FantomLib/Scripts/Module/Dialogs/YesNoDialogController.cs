using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Yes/No Dialog Controller
    ///･The value of the callback is 'yesValue' when it is a 'Yes' button pressed, and becomes 'noValue' when it is a 'No' button pressed.
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class YesNoDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                  //Dialog title
        public string message = "Message";              //Dialog message
        public string yesButton = "OK";                 //Text of 'Yes' button.
        public string yesValue = "yes";                 //Callback value when 'Yes' pressed.
        public string noButton = "Cancel";              //Text of 'No' button.
        public string noValue = "no";                   //Callback value when 'No' pressed.

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Callbacks
        [Serializable] public class YesHandler : UnityEvent<string> { }     //yesValue
        public YesHandler OnYes;

        [Serializable] public class NoHandler : UnityEvent<string> { }      //noValue
        public YesHandler OnNo;


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Show dialog
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("YesNoDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowDialog(
                title,
                message,
                gameObject.name, "ReceiveResult",
                yesButton, yesValue,
                noButton, noValue,
                style);
#endif
        }


        //Returns value when button pressed.
        private void ReceiveResult(string result)
        {
            if (result == yesValue)
            {
                if (OnYes != null)
                    OnYes.Invoke(yesValue);
            }
            else if (result == noValue)
            {
                if (OnNo != null)
                    OnNo.Invoke(noValue);
            }
        }
    }
}