using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FantomLib
{
    /// <summary>
    /// Get long press and call back (Suitable for judgment on UI, Event System and Graphics Raycaster are required)
    /// http://fantom1x.blog130.fc2.com/blog-entry-251.html
    /// </summary>
    public class LongClickEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public float validTime = 1.0f;      //Time to recognize as long press (to recognize it as a long press with longer time)

        //Local Values
        float requiredTime;                 //Long press recognition time (recognize it as long press after this time)
        bool pressing = false;              //Pressing flag (also used for acquiring only one finger)

        //Long press event callback
        public UnityEvent OnLongClick;

        //Long press/progress start event callback
        public UnityEvent OnStart;

        //Progress event callback
        [Serializable] public class ProgressHandler : UnityEvent<float> { } //Amount at progress: 0~1f
        public ProgressHandler OnProgress;

        //Progress interrupted event callback
        public UnityEvent OnCancel;


        // Update is called once per frame
        void Update()
        {
            if (pressing)
            {
                if (requiredTime < Time.time)
                {
                    if (OnLongClick != null)
                        OnLongClick.Invoke();

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
        }

        //Press in the UI area etc
        public void OnPointerDown(PointerEventData data)
        {
            if (!pressing)
            {
                pressing = true;
                requiredTime = Time.time + validTime;

                if (OnStart != null)
                    OnStart.Invoke();
            }
            else
            {
                pressing = false;
            }
        }

        //(*)If it is a smartphone and it is transparent to the UI, it reacts even if you move your finger a little.
        public void OnPointerUp(PointerEventData data)
        {
            if (pressing)
            {
                if (OnCancel != null)
                    OnCancel.Invoke();

                pressing = false;
            }
        }

        //Disable it if it is outside the UI area
        public void OnPointerExit(PointerEventData data)
        {
            if (pressing)
            {
                if (OnCancel != null)
                    OnCancel.Invoke();

                pressing = false;
            }
        }
    }

}