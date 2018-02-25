using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Hardware Volume When operated, Android Toast displays the current volume
    /// </summary>
    public class HardVolumeControllerWithToast : HardVolumeController
    {

        public bool enableToast = true;     //Display Android Toast on/off


        protected void Awake()
        {
            //Register itself when it is empty
            if (OnVolumeCalled.GetPersistentEventCount() == 0)
            {
#if UNITY_EDITOR
                Debug.Log("OnVolumeCalled added DisplayVolume (auto)");
#endif
                OnVolumeCalled.AddListener(DisplayVolume);
            }
        }


        //Display Android Toast
        public void ShowToast(string message)
        {
            if (!enableToast)
                return;

#if UNITY_EDITOR
            Debug.Log("ShowToast : " + message);
#elif UNITY_ANDROID
            if (!string.IsNullOrEmpty(message))
                AndroidPlugin.ShowToast(message);
#endif
        }


        //Format to a string for display Android Toast
        public void DisplayVolume(int value)
        {
            ShowToast("Volume : " + value + " / " + maxVolume);
        }

    }

}