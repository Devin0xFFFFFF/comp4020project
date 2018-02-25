using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// OK with CheckBox Dialog Controller
    ///･The value of the callback is always 'resultValue'.
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///･Note: When using value save (saveChecked), it is better to give a specific save name (saveCheckedKey) individually.
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class OKWithCheckBoxDialogController : SavedCheckedBehaviour
    {
        //Inspector Settings
        public string title = "Title";                          //Dialog title
        public string message = "Message";                      //Dialog message
        public string okButton = "OK";                          //Text of 'OK' button.
        public string resultValue = "closed";                   //Callback value when closed dialog.

        //CheckBox
        [SerializeField] private bool defaultChecked = false;   //Default state of CheckBox (If saved, it will be overwritten).
        public string checkBoxText = "Remember me";             //Text of CheckBox
        public Color checkBoxTextColor = Color.black;           //Text color of CheckBox

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Save PlayerPrefs Settings
        public bool saveChecked = true;                         //Whether to save the CheckBox value (Also local value is always overwritten).
        [SerializeField] private string saveCheckedKey = "";    //When specifying the PlayerPrefs key for CheckBox.

        //Callbacks
        [Serializable] public class CloseHandler : UnityEvent<string, bool> { }     //resultValue, checked
        public CloseHandler OnClose;

#region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveCheckedKey is empty)
        const string CHECKED_PREF = "_checked";

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_checked'")
        public string SaveCheckedKey {
            get { return string.IsNullOrEmpty(saveCheckedKey) ? gameObject.name + CHECKED_PREF : saveCheckedKey; }
        }

        //Load local values manually.
        public void LoadPrefs()
        {
            defaultChecked = XPlayerPrefs.GetBool(SaveCheckedKey, defaultChecked);
        }

        //Save local values manually.
        //･To be saved value is only checked state.
        public void SavePrefs()
        {
            XPlayerPrefs.SetBool(SaveCheckedKey, defaultChecked);
            PlayerPrefs.Save();
        }

        //Delete PlayerPrefs key.
        //Note: Local values are not initialized at this time.
        public void DeletePrefs()
        {
            PlayerPrefs.DeleteKey(SaveCheckedKey);
        }

        //Returns true if the saved value exists.
        public bool HasPrefs {
            get { return PlayerPrefs.HasKey(SaveCheckedKey); }
        }

        //Checked already saved state. When first time (before saving) always false.
        public override bool SavedChecked {
            get { return XPlayerPrefs.GetBool(SaveCheckedKey, false); }
        }

#endregion

#region Properties and Local values Section

        //Initial state of CheckBox.
        //･If saveChecked is true, it will be automatically saved.
        public bool DefaultChecked {
            get { return defaultChecked; }
            set {
                defaultChecked = value;
                if (saveChecked)
                    SavePrefs();
            }
        }

        //The value for reset.
        private bool initChecked;

        //Store the value of the inspector.
        private void StoreInitChecked()
        {
            initChecked = defaultChecked;
        }

        //Restore the value of the inspector and delete PlayerPrefs key.
        public void ResetChecked()
        {
            defaultChecked = initChecked;
            DeletePrefs();
        }

#endregion

        // Use this for initialization
        private void Awake()
        {
            StoreInitChecked();

            if (saveChecked)
                LoadPrefs();
        }

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
            Debug.Log("OKWithCheckBoxDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowDialogWithCheckBox(
                title,
                message,
                checkBoxText, checkBoxTextColor,
                defaultChecked, 
                gameObject.name, "ReceiveResult",
                okButton, resultValue,
                style);
#endif
        }


        //The callback value (resultValue) always returns.
        private void ReceiveResult(string result)
        {
            bool check = result.EndsWith(", CHECKED_TRUE");
            if (saveChecked)
            {
                defaultChecked = check;
                SavePrefs();
            }

            if (result.StartsWith(resultValue))
            {
                if (OnClose != null)
                    OnClose.Invoke(resultValue, check);
            }
        }
    }
}
