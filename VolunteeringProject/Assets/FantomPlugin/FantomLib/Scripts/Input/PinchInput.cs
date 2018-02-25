using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Get pinch and call back 
    /// http://fantom1x.blog130.fc2.com/blog-entry-288.html
    /// </summary>
    public class PinchInput : MonoBehaviour
    {
        public bool isNormalized = true;        //Call back with the normalized value with screen width (or height) (false = returned in pixels)
        public bool widthReference = true;      //When isNormalized=true, based on the screen width (Screen.width) (when false based on height (Screen.height)) [unit becomes like px/Screen.width]

        //Area on screen to recognize: 0.0~1.0 [(0,0):Bottom left of screen, (1,1):Upper right of screen]
        public Rect validArea = new Rect(0, 0, 1, 1);

        //Pinching detection properties (For each frame acquisition)
        public bool IsPinching {
            get; private set;
        }

        //Pinch width (distance) property (For each frame acquisition)
        //(*)When isNormalized=true, it is normalized with the screen width, and when it is false, it becomes px unit.
        public float Width {
            get; private set;
        }

        //Difference property from just before pinch width (distance) (For each frame acquisition)
        //(*)When isNormalized=true, it is normalized with the screen width, and when it is false, it becomes px unit.
        public float Delta {
            get; private set;
        }

        //Change ratio property of pinching width (distance) property (For each frame acquisition)
        //(*)Open the finger -> 1.0 or more (1, 2, 3, ...[including decimal] / finger closing -> lower than 1.0 (1/2, 1/3, 1/4, ... [not negative]))
        //(*)Note that closing fingers is easier than opening fingers physically (good for scale operation etc).
        public float Ratio {
            get; private set;
        }


        //Pinch start callback
        [Serializable]
        public class PinchStartHandler : UnityEvent<float, Vector2> { } //Width, center
        public PinchStartHandler OnPinchStart;

        //Pinching callback (expansion ratio and difference)
        [Serializable]
        public class PinchHandler : UnityEvent<float, float, float> { } //Width, Delta, Ratio
        public PinchHandler OnPinch;


        //Local Values
        float startDistance;            //Finger distance at pinch start (px)
        float oldDistance;              //The previous expansion at contraction distance (px)


        void OnEnable()
        {
            IsPinching = false;
        }

        // Update is called once per frame
        void Update()
        {
            //Property reset for each frame
            Width = 0; Delta = 0; Ratio = 1;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)   //Only platforms you want to obtain with touch
            if (Input.touchCount == 2)
            {
                Touch touch = (Input.touches[1].fingerId == 1) ? Input.touches[1] : Input.touches[0];
                if (!IsPinching && touch.phase == TouchPhase.Began)
                {
                    Vector2 center = (Input.touches[0].position + Input.touches[1].position) / 2;
                    if (validArea.xMin * Screen.width <= center.x && center.x <= validArea.xMax * Screen.width && 
                        validArea.yMin * Screen.height <= center.y && center.y <= validArea.yMax * Screen.height)
                    {
                        IsPinching = true;

                        Width = startDistance = oldDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                        if (isNormalized)
                        {
                            float unit = widthReference ? Screen.width : Screen.height;
                            Width /= unit;
                            center /= unit;
                        }

                        if (OnPinchStart != null)
                            OnPinchStart.Invoke(Width, center);
                    }
                }
                else if (IsPinching)
                {
                    float endDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                    Width = endDistance;
                    Delta = endDistance - oldDistance;
                    Ratio = endDistance / startDistance;
                    oldDistance = endDistance;

                    if (isNormalized)
                    {
                        float unit = widthReference ? Screen.width : Screen.height;
                        Width /= unit;
                        Delta /= unit;
                    }

                    if (OnPinch != null)
                        OnPinch.Invoke(Width, Delta, Ratio);
                }
            }
            else
#endif
            {
                IsPinching = false;
            }
        }
    }
}
