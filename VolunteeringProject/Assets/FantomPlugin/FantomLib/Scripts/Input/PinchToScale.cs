using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Change the scale with a pinch (local scale)
    /// (*) use PinchInput callback
    /// http://fantom1x.blog130.fc2.com/blog-entry-288.html
    /// </summary>
    public class PinchToScale : MonoBehaviour
    {
        public Transform target;    //Object to be changed in scale

        //Local Values
        Vector3 startScale;         //Scale at pinch start
        Vector3 initScale;          //Initial scale (for reset)


        // Use this for initialization
        private void Start()
        {
            if (target == null)
                target = gameObject.transform;

            initScale = target.localScale;
        }

        // Update is called once per frame
        //private void Update () {

        //}


        //width: distance of two fingers of pinch
        //center: The coordinates of the center of two fingers of pinch
        public void OnPinchStart(float width, Vector2 center)
        {
            if (target != null)
                startScale = target.localScale;
        }

        //width: distance of two fingers of pinch
        //delta: The difference in pinch width just before
        //ratio: Stretch ratio from the start of pinch width (1:At the start of pinch, Expand by 1 or more, lower than 1 (1/2, 1/3, ...)
        public void OnPinch(float width, float delta, float ratio)
        {
            if (target != null)
                target.localScale = startScale * ratio;
        }

        //Restore the initial scale
        public void ResetScale()
        {
            if (target != null)
                target.localScale = initScale;
        }
    }
}