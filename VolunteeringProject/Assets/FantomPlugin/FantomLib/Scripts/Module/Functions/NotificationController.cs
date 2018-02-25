using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Notification Controller
    /// </summary>
    public class NotificationController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "";                       //When it is empty, the application name is used (PlayerSettings > Product Name).
        public string message = "Message";              //Message to be displayed on Notification.

        [Serializable]
        public enum TapAction
        {
            BackToApplication,          //Tap the Notification to return to the application.
            OpenURL,                    //Tap the Notification to open the URL.
        }
        public TapAction tapAction = TapAction.BackToApplication;

        public string url = "";                         //It is opened in the default browser.
        public string iconName = "app_icon";            //Unity defaults to "app_icon" (Unless rewrite it with a 'AndroidManifest.xml' file).
        public string idTag = "tag";                    //Identification tag (When notified consecutively, same tag is displayed by overwriting).
        public bool showTimestamp = true;               //Display notification time.

        [Serializable]
        public enum VibratorType {
            OneShot, Pattern
        }
        public VibratorType vibratorType = VibratorType.OneShot;

        //Because it can not be saved in the inspector if it is a long type, it is converted from an int type.
        //That is, the value will be in the int range (however, a long range is not necessary).
        [SerializeField] private int vibratorDuration;      //Vibration duration when SimpleOneShot.
        [SerializeField] private int[] vibratorPattern;     //Vibration pattern. Specify the duration in milliseconds in the order of off, on, off, on, ....

#region Properties and Local values Section

        //Local values
        //Because it can not be saved in the inspector if it is a long type, it is converted from an int type.
        //That is, the value will be in the int range (however, a long range is not necessary).
        private long mVibratorDuration;
        private long[] mVibratorPattern;

        //Properties
        public long[] VibratorPattern {
            get { return mVibratorPattern; }
            set { mVibratorPattern = value; }
        }

        public long VibratorDuration {
            get { return mVibratorDuration; }
            set { mVibratorDuration = value; }
        }

        //Check empty etc.
        private void CheckForErrors()
        {
            if (tapAction == TapAction.OpenURL && string.IsNullOrEmpty(url))
                Debug.LogWarning("URL is empty.");
        }

#endregion

        // Use this for initialization
        private void Awake()
        {
            //It mainly converts from int type to long type (also inspector -> local value).
            mVibratorDuration = vibratorDuration;
            if (vibratorPattern != null && vibratorPattern.Length > 0)
                mVibratorPattern = vibratorPattern.Select(e => (long)e).ToArray();
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(title))
                title = Application.productName;    //When empty (PlayerSettings > Product Name)

#if UNITY_EDITOR
            CheckForErrors();   //Check for fatal errors (Editor only).
#endif
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Show Notification
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("NotificationController.Show called");
#elif UNITY_ANDROID
            switch (tapAction)
            {
                case TapAction.BackToApplication:
                    if (vibratorType == VibratorType.OneShot)
                    {
                        AndroidPlugin.ShowNotification(
                            title, 
                            message,
                            string.IsNullOrEmpty(iconName) ? "app_icon" : iconName,
                            idTag,
                            showTimestamp,
                            mVibratorDuration);      //Converted to a long type.
                    }
                    else
                    {
                        AndroidPlugin.ShowNotification(
                            title, 
                            message,
                            string.IsNullOrEmpty(iconName) ? "app_icon" : iconName,
                            idTag,
                            showTimestamp,
                            mVibratorPattern);      //Converted to a long type.
                    }
                    break;

                case TapAction.OpenURL:
                    if (string.IsNullOrEmpty(url))
                        return;

                    if (vibratorType == VibratorType.OneShot)
                    {
                        AndroidPlugin.ShowNotificationToActionURI(
                            title,
                            message,
                            string.IsNullOrEmpty(iconName) ? "app_icon" : iconName,
                            idTag,
                            "android.intent.action.VIEW",
                            url,
                            showTimestamp,
                            mVibratorDuration);      //Converted to a long type.
                    }
                    else
                    {
                        AndroidPlugin.ShowNotificationToActionURI(
                            title,
                            message,
                            string.IsNullOrEmpty(iconName) ? "app_icon" : iconName,
                            idTag,
                            "android.intent.action.VIEW",
                            url,
                            showTimestamp,
                            mVibratorPattern);      //Converted to a long type.
                    }
                    break;
            }
#endif
        }
    }
}
