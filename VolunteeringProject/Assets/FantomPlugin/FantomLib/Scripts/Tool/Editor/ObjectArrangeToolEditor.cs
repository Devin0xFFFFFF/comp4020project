using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FantomLib
{
    /// <summary>
    /// Arrange objects at equal intervals (mainly for uGUI)
    /// </summary>
    [CustomEditor(typeof(ObjectArrangeTool))]
    public class ObjectArrangeToolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);

            if (GUILayout.Button("Arrage"))
            {
                var tool = target as ObjectArrangeTool;
                if (!Application.isPlaying && !tool.executing)
                {
                    tool.Arrange();
                }
            }
        }
    }
}
