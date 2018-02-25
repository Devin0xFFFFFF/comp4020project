using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Arrange objects at equal intervals (mainly for uGUI)
    ///･Place it step by step based on objects[0].
    /// </summary>
    [ExecuteInEditMode]
    public class ObjectArrangeTool : MonoBehaviour
    {
#if UNITY_EDITOR
        [Serializable]
        public enum Axis {
            X, Y, Z
        }

        public Axis axis = Axis.Y;      //Axis to be arranged
        public float step = -100;       //Alignment interval
        public GameObject[] objects;    //Objects to be arranged

        //Running Flag
        public bool executing {
            get; private set;
        }


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Arrange objects at equal intervals
        public void Arrange()
        {
            if (objects.Length < 2)  //not nessesary
                return;

            executing = true;

            float start;
            if (axis == Axis.Y)
                start = objects[0].transform.localPosition.y;
            else if (axis == Axis.X)
                start = objects[0].transform.localPosition.x;
            else
                start = objects[0].transform.localPosition.z;

            for (int i = 1; i < objects.Length; i++)
            {
                Vector3 pos = objects[i].transform.localPosition;
                if (axis == Axis.Y)
                    pos.y = start + step * i;
                else if (axis == Axis.X)
                    pos.x = start + step * i;
                else
                    pos.z = start + step * i;

                objects[i].transform.localPosition = pos;
            }

            executing = false;
        }
#endif
    }
}
