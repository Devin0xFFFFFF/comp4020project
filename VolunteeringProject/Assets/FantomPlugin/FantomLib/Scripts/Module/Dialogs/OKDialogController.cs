using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// OK Dialog Controller
    ///･The value of the callback is always 'resultValue'.
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class OKDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                  //Dialog title
        public string message = "Message";              //Dialog message
        public string okButton = "OK";                  //Text of 'OK' button.
        public string resultValue = "ok";               //Callback value when 'OK' pressed.

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Callbacks
        [Serializable] public class CloseHandler : UnityEvent<string> { }   //resultValue
        public CloseHandler OnClose;


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
            Debug.Log("OKDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowDialog(
                title,
                message,
                gameObject.name, "ReceiveResult",
                okButton, resultValue,
                style);
#endif
        }


        //Returns value when closed dialog.
        private void ReceiveResult(string result)
        {
            if (result == resultValue)
            {
                if (OnClose != null)
                    OnClose.Invoke(resultValue);
            }
        }
    }
}
