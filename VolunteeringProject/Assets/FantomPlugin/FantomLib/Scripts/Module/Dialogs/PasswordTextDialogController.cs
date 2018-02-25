using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Password Text Dialog Controller
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class PasswordTextDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                  //Dialog title
        public string message = "Message";              //Dialog message
        public int maxLength = 0;                       //Limit on number of characters (When it is 0 there is no limit).
        public bool numberOnly = false;                 //Only number characters can be entered.

        public string okButton = "OK";                  //Text of 'OK' button.
        public string cancelButton = "Cancel";          //Text of 'Cancel' button.

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Callbacks
        [Serializable] public class ResultHandler : UnityEvent<string> { }  //text (*)number -> string type
        public ResultHandler OnResult;


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Show dialog
        public void Show(string password)
        {
#if UNITY_EDITOR
            Debug.Log("PasswordTextDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowPasswordTextDialog(
                title,
                message,
                password,
                maxLength,
                numberOnly,
                gameObject.name, "ReceiveResult",
                okButton, cancelButton,
                style);
#endif
        }


        //Returns value when 'OK' pressed.
        private void ReceiveResult(string result)
        {
            if (OnResult != null)
                OnResult.Invoke(result);    //Note: It is not encrypted (plane text).
        }
    }
}
