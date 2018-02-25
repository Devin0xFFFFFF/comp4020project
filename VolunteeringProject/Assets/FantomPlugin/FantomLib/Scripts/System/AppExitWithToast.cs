using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Application exit with key input (with twice push within the time limit), Android Toast displays the pressed state
    /// </summary>
    public class AppExitWithToast : AppExitWithKey
    {

        public bool showOneMoreMessage = true;
        public LocalizeString oneMoreMessage = new LocalizeString(
            new List<LocalizeString.Data>()
            {
                new LocalizeString.Data(SystemLanguage.English, "Press again to exit."),
                new LocalizeString.Data(SystemLanguage.Japanese, "もう一度押すと終了します。"),
            });


        public bool showExitMessage = false;
        public LocalizeString exitMessage = new LocalizeString(
            new List<LocalizeString.Data>()
            {
                new LocalizeString.Data(SystemLanguage.English, "Exit the application."),
                new LocalizeString.Data(SystemLanguage.Japanese, "アプリケーションを終了します。"),
            });



        // Use this for initialization
        protected new void Start()
        {
            base.Start();

            oneMoreMessage.Initialize();    //Apply inspector registration.
            exitMessage.Initialize();       //Apply inspector registration.

            //Register itself when it is empty
            if (showOneMoreMessage && OnFirstPressed.GetPersistentEventCount() == 0)
            {
#if UNITY_EDITOR
                Debug.Log("OnFirstPressed added ShowOneMoreToast (auto)");
#endif
                OnFirstPressed.AddListener(ShowOneMoreToast);
            }

            if (showExitMessage && OnSecondPressed.GetPersistentEventCount() == 0)
            {
#if UNITY_EDITOR
                Debug.Log("OnSecondPressed added ShowExitToast (auto)");
#endif
                OnSecondPressed.AddListener(ShowExitToast);
            }
        }


        //When "Press again to exit." Toast
        public void ShowOneMoreToast()
        {
            if (!showOneMoreMessage)
                return;

#if UNITY_EDITOR
            Debug.Log("ShowOneMoreToast called");
#elif UNITY_ANDROID
            string text = oneMoreMessage.Text;
            if (!string.IsNullOrEmpty(text))
                AndroidPlugin.ShowToast(text);
#endif
        }


        //When "Exit the application." Toast (*) When using it you better put a time to display a bit with exitDelay
        public void ShowExitToast()
        {
            if (!showExitMessage)
                return;

#if UNITY_EDITOR
            Debug.Log("ShowExitToast called");
#elif UNITY_ANDROID
            string text = exitMessage.Text;
            if (!string.IsNullOrEmpty(text))
                AndroidPlugin.ShowToast(text);
#endif
        }


        //Wait for the specified time and then exit. (For calling "OnExit()")
        protected override IEnumerator WaitAndExit(float sec)
        {
            if (OnBeforeDelay != null)
                OnBeforeDelay.Invoke();

            yield return new WaitForSeconds(sec);

            if (OnBeforeExit != null)
                OnBeforeExit.Invoke();

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidPlugin.CancelToast();    //(*) Since the Toast tends to remain long on the screen, it disappears here.
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //Editor
#elif !UNITY_WEBGL && !UNITY_WEBPLAYER
            Application.Quit();
#endif
            done = true;
        }
    }

}
