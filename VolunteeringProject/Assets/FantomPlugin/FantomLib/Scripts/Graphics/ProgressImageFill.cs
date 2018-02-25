using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FantomLib
{
    /// <summary>
    /// Use Image.fillAmount as progress
    /// </summary>
    public class ProgressImageFill : MonoBehaviour
    {
        [Range(0, 1)] public float preAmount = 0;       //fillAmount on startup/reset
        [Range(0, 1)] public float startAmount = 0;     //fillAmount at start of progress
        [Range(0, 1)] public float completeAmount = 1;  //fillAmount at completion of progress
        public bool delayedResetOnComplete = true;      //Call delayed reset on completion
        public float resetDelay = 0.1f;                 //Delayed reset time

        public Image fillImage;                         //Image for operate fillAmount


        // Use this for initialization
        private void Start()
        {
            if (fillImage != null)
                fillImage.fillAmount = preAmount;
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        public void OnStart()
        {
            if (fillImage != null)
                fillImage.fillAmount = startAmount;
        }

        public void OnProgress(float amount)
        {
            if (fillImage != null)
                fillImage.fillAmount = amount;
        }

        public void OnComplete()
        {
            if (fillImage != null)
                fillImage.fillAmount = completeAmount;

            if (delayedResetOnComplete)
                Invoke("OnReset", resetDelay);
        }

        public void OnReset()
        {
            if (fillImage != null)
                fillImage.fillAmount = preAmount;
        }
    }
}