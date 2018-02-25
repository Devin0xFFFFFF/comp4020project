using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FantomLib
{
    [CustomEditor(typeof(SingleChoiceDialogController))]
    public class SingleChoiceDialogControllerEditor : Editor
    {
        SerializedProperty title;
        GUIContent titleLabel = new GUIContent("Title");
        SerializedProperty items;
        GUIContent itemsLabel = new GUIContent("Items");
        SerializedProperty selectedIndex;
        GUIContent selectedIndexLabel = new GUIContent("Selected Index");
        SerializedProperty resultType;
        GUIContent resultTypeLabel = new GUIContent("Result Type");
        SerializedProperty okButton;
        GUIContent okButtonLabel = new GUIContent("Ok Button");
        SerializedProperty cancelButton;
        GUIContent cancelButtonLabel = new GUIContent("Cancel Button");
        SerializedProperty style;
        GUIContent styleLabel = new GUIContent("Style");
        SerializedProperty saveValue;
        GUIContent saveValueLabel = new GUIContent("Save Value");
        SerializedProperty saveKey;
        GUIContent saveKeyLabel = new GUIContent("Save Key");

        SerializedProperty OnResult;
        SerializedProperty OnResultIndex;
        SerializedProperty OnValueChanged;
        SerializedProperty OnValueIndexChanged;

        private void OnEnable()
        {
            title = serializedObject.FindProperty("title");
            items = serializedObject.FindProperty("items");
            selectedIndex = serializedObject.FindProperty("selectedIndex");
            resultType = serializedObject.FindProperty("resultType");
            okButton = serializedObject.FindProperty("okButton");
            cancelButton = serializedObject.FindProperty("cancelButton");
            style = serializedObject.FindProperty("style");
            saveValue = serializedObject.FindProperty("saveValue");
            saveKey = serializedObject.FindProperty("saveKey");
            OnResult = serializedObject.FindProperty("OnResult");
            OnResultIndex = serializedObject.FindProperty("OnResultIndex");
            OnValueChanged = serializedObject.FindProperty("OnValueChanged");
            OnValueIndexChanged = serializedObject.FindProperty("OnValueIndexChanged");
        }

        public override void OnInspectorGUI()
        {
            var obj = target as SingleChoiceDialogController;
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target) , typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            //obj.title = EditorGUILayout.TextField("Title", obj.title);
            EditorGUILayout.PropertyField(title, titleLabel, true);

            EditorGUILayout.PropertyField(items, itemsLabel, true);

            EditorGUILayout.PropertyField(selectedIndex, selectedIndexLabel, true);

            //obj.resultType = (SingleChoiceDialogController.ResultType)EditorGUILayout.EnumPopup("Result Type", obj.resultType);
            EditorGUILayout.PropertyField(resultType, resultTypeLabel, true);

            //obj.okButton = EditorGUILayout.TextField("Ok Button", obj.okButton);
            EditorGUILayout.PropertyField(okButton, okButtonLabel, true);

            //obj.cancelButton = EditorGUILayout.TextField("Cancel Button", obj.cancelButton);
            EditorGUILayout.PropertyField(cancelButton, cancelButtonLabel, true);

            //obj.style = EditorGUILayout.TextField("Style", obj.style);
            EditorGUILayout.PropertyField(style, styleLabel, true);

            //obj.saveValue = EditorGUILayout.Toggle("Save Value", obj.saveValue);
            EditorGUILayout.PropertyField(saveValue, saveValueLabel, true);

            EditorGUILayout.PropertyField(saveKey, saveKeyLabel, true);

            switch (obj.resultType)
            {
                case SingleChoiceDialogController.ResultType.Index:
                    EditorGUILayout.PropertyField(OnResultIndex, true);
                    EditorGUILayout.PropertyField(OnValueIndexChanged, true);
                    break;
                case SingleChoiceDialogController.ResultType.Value:
                case SingleChoiceDialogController.ResultType.Text:
                    EditorGUILayout.PropertyField(OnResult, true);
                    EditorGUILayout.PropertyField(OnValueChanged, true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
