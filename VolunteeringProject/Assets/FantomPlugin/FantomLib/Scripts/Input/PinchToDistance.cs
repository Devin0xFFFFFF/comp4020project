using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Pinch to operate distance
    /// (*) use PinchInput callback
    /// http://fantom1x.blog130.fc2.com/blog-entry-288.html
    /// </summary>
    public class PinchToDistance : MonoBehaviour
    {
        public Transform target;            //Object to be a viewpoint
        public float speed = 2f;            //Rate of change
        public float minDistance = 1.0f;    //Minimum distance to approach
        public bool lookAt = true;          //Look at the object

        //LocalValues
        float initDistance;                 //Initial distance (for reset)


        // Use this for initialization
        private void Start()
        {
            if (target != null)
            {
                Vector3 dir = target.position - transform.position;
                initDistance = dir.magnitude;
                if (lookAt)
                    transform.LookAt(target.position);
            }
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //width: distance of two fingers of pinch
        //center: The coordinates of the center of two fingers of pinch
        public void OnPinchStart(float width, Vector2 center)
        {
        }

        //width: distance of two fingers of pinch
        //delta: The difference in pinch width just before
        //ratio: Stretch ratio from the start of pinch width (1:At the start of pinch, Expand by 1 or more, lower than 1 (1/2, 1/3, ...)
        public void OnPinch(float width, float delta, float ratio)
        {
            if (target == null)
                return;

            Vector3 dir = target.position - transform.position;
            float distance = Math.Max(minDistance, dir.magnitude - delta * speed);
            Vector3 pos = target.position - dir.normalized * distance;
            transform.position = pos;
            if (lookAt)
                transform.LookAt(target.position);
        }

        //Restore the initial distance
        public void ResetDistance()
        {
            if (target == null)
                return;

            Vector3 dir = target.position - transform.position;
            Vector3 pos = target.position - dir.normalized * initDistance;
            transform.position = pos;
            if (lookAt)
                transform.LookAt(target.position);
        }
    }
}