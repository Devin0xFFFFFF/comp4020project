using System;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Get long press and call back (Suitable for judgment in whole screen area and partial area of screen)
    /// http://fantom1x.blog130.fc2.com/blog-entry-251.html
    /// </summary>
    public class LongClickInput : MonoBehaviour
    {
        //Setting value
        public float validTime = 1.0f;      //Time to recognize as long press (to recognize it as a long press with longer time)

        //Area on screen to recognize: 0.0~1.0 [(0,0):Bottom left of screen, (1,1):Upper right of screen]
        public Rect validArea = new Rect(0, 0, 1, 1);

        //Local Values
        Vector2 minPos = Vector2.zero;      //Long press recognition pixel minimum coordinate
        Vector2 maxPos = Vector2.one;       //Long press recognition pixel maximum coordinate
        float requiredTime;                 //Long press recognition time (recognize it as long press after this time)
        bool pressing;                      //Pressing flag (also used for acquiring only one finger)

        bool isValid = false;               //For each frame validity

        //Long press detection property (For each frame acquisition)
        public bool IsLongClick {
            get { return isValid; }
        }

        //Long press event callback
        public UnityEvent OnLongClick;

        //Long press/progress start event callback
        public UnityEvent OnStart;

        //Progress event callback
        [Serializable] public class ProgressHandler : UnityEvent<float> { } //Amount at progress: 0~1f
        public ProgressHandler OnProgress;

        //Progress interrupted event callback
        public UnityEvent OnCancel;


        void OnEnable()
        {
            pressing = false;
        }

        // Update is called once per frame
        void Update()
        {
            isValid = false;    //Reset per frame

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)   //Only platforms you want to obtain with touch
            if (Input.touchCount == 1)
#endif
            {
                if (!pressing && Input.GetMouseButtonDown(0))
                {
                    Vector2 pos = Input.mousePosition;
                    minPos.Set(validArea.xMin * Screen.width, validArea.yMin * Screen.height);
                    maxPos.Set(validArea.xMax * Screen.width, validArea.yMax * Screen.height);
                    if (minPos.x <= pos.x && pos.x <= maxPos.x && minPos.y <= pos.y && pos.y <= maxPos.y)
                    {
                        pressing = true;
                        requiredTime = Time.time + validTime;

                        if (OnStart != null)
                            OnStart.Invoke();
                    }
                }
                else if (pressing)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (requiredTime < Time.time)
                        {
                            Vector2 pos = Input.mousePosition;
                            if (minPos.x <= pos.x && pos.x <= maxPos.x && minPos.y <= pos.y && pos.y <= maxPos.y)
                            {
                                isValid = true;

                                if (OnLongClick != null)
                                    OnLongClick.Invoke();
                            }

                            pressing = false;
                        }
                        else
                        {
                            if (OnProgress != null)
                            {
                                float amount = Mathf.Clamp01(1f - (requiredTime - Time.time) / validTime);  //0~1f
                                OnProgress.Invoke(amount);
                            }
                        }
                    }
                    else  //MouseButtonUp, MouseButtonDown
                    {
                        if (pressing)
                        {
                            if (OnCancel != null)
                                OnCancel.Invoke();
                        }

                        pressing = false;
                    }
                }
            }
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)   //Only platforms you want to obtain with touch
            else
            {
                pressing = false;
            }
#endif
        }
    }

}