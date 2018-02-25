using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Android Action Controller
    ///･Call the action to Android Native.
    /// (Action)
    /// https://developer.android.com/reference/android/content/Intent.html
    /// </summary>
    public class AndroidActionController : MonoBehaviour
    {
        //Inspector Settings
        public string action = "android.intent.action.VIEW";

        [Serializable]
        public enum ActionType
        {
            URI,                //Action to URI
            ExtraQuery,         //Use Extra and Query to action. 
            CHOOSER,            //Start action with Chooser
        }
        public ActionType actionType = ActionType.URI;

        //Parameters to give to the action etc.
        public string title = "";
        public string uri = "";
        public string extra = "query";
        public string query = "keyword";
        public string mimetype = "text/plain";

#region Properties and Local values Section

        //Check empty etc.
        private void CheckForErrors()
        {
            if (string.IsNullOrEmpty(action))
                Debug.LogError("Action is empty.");

            switch (actionType)
            {
                case ActionType.URI:
                    if (string.IsNullOrEmpty(uri))
                        Debug.LogError("Uri is empty.");
                    break;
                case ActionType.ExtraQuery:
                    if (string.IsNullOrEmpty(extra))
                        Debug.LogError("Extra is empty.");
                    break;
                case ActionType.CHOOSER:
                    if (string.IsNullOrEmpty(mimetype))
                        Debug.LogError("MIME Type is empty.");
                    break;
            }
        }

#endregion

        // Use this for initialization
        private void Start()
        {
#if UNITY_EDITOR
            CheckForErrors();   //Check for fatal errors (Editor only).
#endif
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Start the action to Android
        public void StartAction()
        {
#if UNITY_EDITOR
            Debug.Log("AndroidActionControlloer.StartAction called");
#elif UNITY_ANDROID
            switch (actionType)
            {
                case ActionType.URI:
                    AndroidPlugin.StartActionURI(action, uri);
                    break;
                case ActionType.ExtraQuery:
                    AndroidPlugin.StartAction(action, extra, query);
                    break;
                case ActionType.CHOOSER:
                    AndroidPlugin.StartActionWithChooser(action, extra, query, mimetype, title);
                    break;
            }
#endif
        }

        //Start the action to URI (current value will be overwritten)
        public void StartActionURI(string uri)
        {
            if (actionType != ActionType.URI)
                return;

            this.uri = uri;
            StartAction();
        }

        //Start the action to URI with Chooser (current value will be overwritten)
        public void StartActionWithChooser(string query)
        {
            if (actionType != ActionType.CHOOSER)
                return;

            this.query = query;
            StartAction();
        }
    }
}
