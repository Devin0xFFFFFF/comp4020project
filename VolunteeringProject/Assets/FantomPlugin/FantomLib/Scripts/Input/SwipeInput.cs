using System;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Get swipe direction and call back
    /// http://fantom1x.blog130.fc2.com/blog-entry-250.html
    /// </summary>
    public class SwipeInput : MonoBehaviour
    {
        //設定値
        public bool widthReference = true;  //Make the screen width (Screen.width) size the standard of the ratio (false: based on height (Screen.height))
        public float validWidth = 0.25f;    //Screen ratio of movement amount recognized as swipe [ratio to screen width] (0.0~1.0: recognize as swipe with a movement amount longer than this value)
        public float timeout = 0.5f;        //Time to recognize as a swipe (to recognize it as a swipe in less time)

        //Area on screen to recognize: 0.0~1.0 [(0,0):Bottom left of screen, (1,1):Upper right of screen]
        public Rect validArea = new Rect(0, 0, 1, 1);

        //Local Values
        Vector2 startPos;                   //Swipe start coordinates
        Vector2 endPos;                     //Swipe end coordinates
        float limitTime;                    //Swipe time limit (Do not recognize as swipe after this time)
        bool pressing;                      //Pressing flag (to obtain only a single finger)

        Vector2 swipeDir = Vector2.zero;    //The acquired swipe direction (for each frame) [zero, left, right, up, down direction]

        //Swipe direction acquisition property (for each frame)
        public Vector2 Direction {
            get { return swipeDir; }
        }

        //Swipe event callback
        [Serializable]
        public class SwipeHandler : UnityEvent<Vector2> { } //swipe direction
        public SwipeHandler OnSwipe;


        void OnEnable()
        {
            pressing = false;
        }

        // Update is called once per frame
        void Update()
        {
            swipeDir = Vector2.zero;    //Reset per frame

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)   //Only platforms you want to obtain with touch
            if (Input.touchCount == 1)
#endif
            {
                if (!pressing && Input.GetMouseButtonDown(0))
                {
                    startPos = Input.mousePosition;
                    if (validArea.xMin * Screen.width <= startPos.x && startPos.x <= validArea.xMax * Screen.width &&
                        validArea.yMin * Screen.height <= startPos.y && startPos.y <= validArea.yMax * Screen.height)
                    {
                        pressing = true;
                        limitTime = Time.time + timeout;
                    }
                }
                else if (pressing && Input.GetMouseButtonUp(0))
                {
                    pressing = false;

                    if (Time.time < limitTime)
                    {
                        endPos = Input.mousePosition;
                        Vector2 dist = endPos - startPos;
                        float dx = Mathf.Abs(dist.x);
                        float dy = Mathf.Abs(dist.y);
                        float requiredPx = widthReference ? Screen.width * validWidth : Screen.height * validWidth;

                        if (dy < dx)
                        {
                            if (requiredPx < dx)
                                swipeDir = Mathf.Sign(dist.x) < 0 ? Vector2.left : Vector2.right;
                        }
                        else
                        {
                            if (requiredPx < dy)
                                swipeDir = Mathf.Sign(dist.y) < 0 ? Vector2.down : Vector2.up;
                        }

                        if (swipeDir != Vector2.zero)
                        {
                            if (OnSwipe != null)
                                OnSwipe.Invoke(swipeDir);
                        }
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