using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Application exit with key input (with twice push within the time limit)
    /// </summary>
    public class AppExitWithKey : MonoBehaviour
    {
        public bool enableKey = true;               //true = use key
        public KeyCode exitKey = KeyCode.Escape;    //Key code to finish

        public bool oneMoreConfirm = false;         //'Press again' mode (press twice to exit)
        public float oneMoreDuration = 3.0f;        //Time limit of 'Press again'
        public float exitDelay = 0.0f;              //Exit execution delay (Reasonable until 3.0 seconds)


        //Event callback press key
        public UnityEvent OnFirstPressed;           //First time press
        public UnityEvent OnSecondPressed;          //Second time press

        //Event callback before exit
        public UnityEvent OnBeforeDelay;            //Callback when just before waiting
        public UnityEvent OnBeforeExit;             //Callback when just before exit


        //Local Values
        protected bool pressed = false;             //First time press flag
        protected float limitTime;                  //Second pressing time limit
        protected bool done = false;                //Key input done



        // Use this for initialization
        protected void Start()
        {

        }


        // Update is called once per frame
        protected void Update()
        {
            if (enableKey && !done)
            {
                if (Input.GetKeyDown(exitKey))
                {
                    if (oneMoreConfirm)
                    {
                        if (!pressed) //First time press
                        {
                            pressed = true;
                            limitTime = Time.time + oneMoreDuration;

                            if (OnFirstPressed != null)
                                OnFirstPressed.Invoke();
                        }
                        else //Second time press
                        {
                            if (Time.time < limitTime)  //Valid if it is within the time limit
                            {
                                done = true;

                                if (OnSecondPressed != null)
                                    OnSecondPressed.Invoke();

                                OnExit();
                            }
                        }
                    }
                    else //When it exit only once
                    {
                        done = true;
                        OnExit();
                    }
                }

                if (limitTime <= Time.time)  //Reset after time limit
                    pressed = false;
            }
        }


        protected Coroutine coroutine = null;

        //For calling from outside
        public void OnExit()
        {
            if (coroutine == null)
                coroutine = StartCoroutine(WaitAndExit(exitDelay > 0 ? exitDelay : 0));
        }


        //Wait for the specified time and then exit (For calling "OnExit()")
        protected virtual IEnumerator WaitAndExit(float sec)
        {
            if (OnBeforeDelay != null)
                OnBeforeDelay.Invoke();

            yield return new WaitForSeconds(sec);

            if (OnBeforeExit != null)
                OnBeforeExit.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //Editor
#elif !UNITY_WEBGL
            Application.Quit();
#endif
            done = true;
        }
    }

}