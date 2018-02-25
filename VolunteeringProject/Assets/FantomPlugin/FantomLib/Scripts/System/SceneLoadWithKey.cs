using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FantomLib
{
    /// <summary>
    /// Load Scene with key input
    /// </summary>
    public class SceneLoadWithKey : MonoBehaviour
    {
        public int sceneBuildIndex = 0;             //Index of 'Scenes in Build'
        public bool useName = false;                //true = use "sceneName" / false = use "sceneBuildIndex"
        public string sceneName = "";               //Scene Name
        public bool isAdditive = false;             //Additional Load

        public bool enableKey = true;               //enable key operation
        public KeyCode loadKey = KeyCode.Escape;    //Key code to load scene

        public float loadDelay = 0.0f;              //Load execution delay (Reasonable until 3.0 seconds)


        //Event callback press key
        public UnityEvent OnKeyPressed;             //Callback when press load key

        //Event callback before load
        public UnityEvent OnBeforeDelay;            //Callback when just before waiting
        public UnityEvent OnBeforeLoad;             //Callback when just before load


        //Local Values
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
                if (Input.GetKeyDown(loadKey))
                {
                    done = true;

                    if (OnKeyPressed != null)
                        OnKeyPressed.Invoke();

                    OnSceneLoad();
                }
            }
        }


        protected Coroutine coroutine = null;

        //For calling from outside
        public void OnSceneLoad()
        {
            if (coroutine == null)
                coroutine = StartCoroutine(WaitAndLoad(loadDelay > 0 ? loadDelay : 0));
        }


        //Wait for the specified time and then load the scene (For calling "OnSceneLoad()")
        protected virtual IEnumerator WaitAndLoad(float sec)
        {
            if (OnBeforeDelay != null)
                OnBeforeDelay.Invoke();

            yield return new WaitForSeconds(sec);

            if (OnBeforeLoad != null)
                OnBeforeLoad.Invoke();

            if (useName)
            {
                if (!string.IsNullOrEmpty(sceneName))
                {
                    SceneManager.LoadScene(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning("sceneName is empty.");
#endif
                    done = false;
                    coroutine = null;
                    yield break;
                }
            }
            else
            {
                SceneManager.LoadScene(sceneBuildIndex, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            }

            //For additional loading, reset it.
            if (isAdditive)
            {
                done = false;
                coroutine = null;
            }
            else
            {
                done = true;
            }
        }
    }

}