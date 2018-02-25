using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FantomLib
{
    [CustomEditor(typeof(DialogItemParameter))]
    public class DialogItemParameterEditor : Editor
    {
        SerializedProperty type;
        GUIContent typeLabel = new GUIContent("Item Type");
        SerializedProperty lineHeight;
        GUIContent lineHeightLabel = new GUIContent("Line Height");
        SerializedProperty lineColor;
        GUIContent lineColorLabel = new GUIContent("Line Color");
        SerializedProperty text;
        GUIContent textLabel = new GUIContent("Text");
        SerializedProperty textColor;
        GUIContent textColorLabel = new GUIContent("Text Color");
        SerializedProperty backgroundColor;
        GUIContent backgroundColorLabel = new GUIContent("Background Color");
        SerializedProperty align;
        GUIContent alignLabel = new GUIContent("Align");
        SerializedProperty key;
        GUIContent keyLabel = new GUIContent("Key");
        SerializedProperty defaultChecked;
        GUIContent defaultCheckedLabel = new GUIContent("isOn");
        SerializedProperty value;
        GUIContent valueLabel = new GUIContent("Value");
        SerializedProperty minValue;
        GUIContent minValueLabel = new GUIContent("Min Value");
        SerializedProperty maxValue;
        GUIContent maxValueLabel = new GUIContent("Max Value");
        SerializedProperty digit;
        GUIContent digitLabel = new GUIContent("Digit");
        SerializedProperty toggleItems;
        GUIContent toggleItemsLabel = new GUIContent("Toggle Items");
        SerializedProperty checkedIndex;
        GUIContent checkedIndexLabel = new GUIContent("Selected Index");

        private void OnEnable()
        {
            type = serializedObject.FindProperty("type");
            lineHeight = serializedObject.FindProperty("lineHeight");
            lineColor = serializedObject.FindProperty("lineColor");
            text = serializedObject.FindProperty("text");
            textColor = serializedObject.FindProperty("textColor");
            backgroundColor = serializedObject.FindProperty("backgroundColor");
            align = serializedObject.FindProperty("align");
            key = serializedObject.FindProperty("key");
            defaultChecked = serializedObject.FindProperty("defaultChecked");
            value = serializedObject.FindProperty("value");
            minValue = serializedObject.FindProperty("minValue");
            maxValue = serializedObject.FindProperty("maxValue");
            digit = serializedObject.FindProperty("digit");
            toggleItems = serializedObject.FindProperty("toggleItems");
            checkedIndex = serializedObject.FindProperty("checkedIndex");
        }

        public override void OnInspectorGUI()
        {
            var obj = target as DialogItemParameter;
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            //obj.type = (DialogItemType)EditorGUILayout.EnumPopup("Item Type", obj.type);
            EditorGUILayout.PropertyField(type, typeLabel, true);

            switch (obj.type)
            {
                case DialogItemType.Divisor:
                    //obj.lineHeight = EditorGUILayout.FloatField("Line Height", obj.lineHeight);
                    EditorGUILayout.PropertyField(lineHeight, lineHeightLabel, true);

                    //obj.lineColor = EditorGUILayout.ColorField("Line Color", obj.lineColor);
                    EditorGUILayout.PropertyField(lineColor, lineColorLabel, true);
                    break;

                case DialogItemType.Text:
                    //obj.text = EditorGUILayout.TextField("Text", obj.text);
                    EditorGUILayout.PropertyField(text, textLabel, true);

                    //obj.textColor = EditorGUILayout.ColorField("Text Color", obj.textColor);
                    EditorGUILayout.PropertyField(textColor, textColorLabel, true);

                    //obj.backgroundColor = EditorGUILayout.ColorField("Background Color", obj.backgroundColor);
                    EditorGUILayout.PropertyField(backgroundColor, backgroundColorLabel, true);

                    //obj.align = (DialogItemParameter.TextAlign)EditorGUILayout.EnumPopup("Align", obj.align);
                    EditorGUILayout.PropertyField(align, alignLabel, true);
                    break;

                case DialogItemType.Switch:
                    //obj.key = EditorGUILayout.TextField("Key", obj.key);
                    EditorGUILayout.PropertyField(key, keyLabel, true);

                    //obj.text = EditorGUILayout.TextField("Text", obj.text);
                    EditorGUILayout.PropertyField(text, textLabel, true);

                    //obj.defaultChecked = EditorGUILayout.Toggle("isOn", obj.defaultChecked);
                    EditorGUILayout.PropertyField(defaultChecked, defaultCheckedLabel, true);

                    //obj.textColor = EditorGUILayout.ColorField("Text Color", obj.textColor);
                    EditorGUILayout.PropertyField(textColor, textColorLabel, true);
                    break;

                case DialogItemType.Slider:
                    //obj.key = EditorGUILayout.TextField("Key", obj.key);
                    EditorGUILayout.PropertyField(key, keyLabel, true);

                    //obj.text = EditorGUILayout.TextField("Text", obj.text);
                    EditorGUILayout.PropertyField(text, textLabel, true);

                    //obj.value = EditorGUILayout.FloatField("Value", obj.value);
                    EditorGUILayout.PropertyField(value, valueLabel, true);

                    //obj.minValue = EditorGUILayout.FloatField("Min Value", obj.minValue);
                    EditorGUILayout.PropertyField(minValue, minValueLabel, true);

                    //obj.maxValue = EditorGUILayout.FloatField("Max Value", obj.maxValue);
                    EditorGUILayout.PropertyField(maxValue, maxValueLabel, true);

                    //obj.digit = EditorGUILayout.IntField("Digit", obj.digit);
                    EditorGUILayout.PropertyField(digit, digitLabel, true);

                    //obj.textColor = EditorGUILayout.ColorField("Text Color", obj.textColor);
                    EditorGUILayout.PropertyField(textColor, textColorLabel, true);
                    break;

                case DialogItemType.Toggle:
                    //obj.key = EditorGUILayout.TextField("Key", obj.key);
                    EditorGUILayout.PropertyField(key, keyLabel, true);

                    EditorGUILayout.PropertyField(toggleItems, toggleItemsLabel, true);

                    //obj.checkedIndex = EditorGUILayout.IntField("Selected Index", obj.checkedIndex);
                    EditorGUILayout.PropertyField(checkedIndex, checkedIndexLabel, true);

                    //obj.textColor = EditorGUILayout.ColorField("Text Color", obj.textColor);
                    EditorGUILayout.PropertyField(textColor, textColorLabel, true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
