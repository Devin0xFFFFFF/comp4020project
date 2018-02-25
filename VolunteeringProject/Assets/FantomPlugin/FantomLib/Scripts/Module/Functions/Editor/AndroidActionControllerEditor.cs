using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FantomLib
{
    [CustomEditor(typeof(AndroidActionController))]
    public class AndroidActionControllerEditor : Editor
    {
        SerializedProperty action;
        GUIContent actionLabel = new GUIContent("Action");
        SerializedProperty actionType;
        GUIContent actionTypeLabel = new GUIContent("Action Type");
        SerializedProperty title;
        GUIContent titleLabel = new GUIContent("Title");
        SerializedProperty uri;
        GUIContent uriLabel = new GUIContent("URI");
        SerializedProperty extra;
        GUIContent extraLabel = new GUIContent("Extra");
        SerializedProperty query;
        GUIContent queryLabel = new GUIContent("Query");
        SerializedProperty mimetype;
        GUIContent mimetypeLabel = new GUIContent("MIME Type");

        private void OnEnable()
        {
            action = serializedObject.FindProperty("action");
            actionType = serializedObject.FindProperty("actionType");
            title = serializedObject.FindProperty("title");
            uri = serializedObject.FindProperty("uri");
            extra = serializedObject.FindProperty("extra");
            query = serializedObject.FindProperty("query");
            mimetype = serializedObject.FindProperty("mimetype");
        }

        public override void OnInspectorGUI()
        {
            var obj = target as AndroidActionController;
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target) , typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            //obj.action = EditorGUILayout.TextField("Action", obj.action);
            EditorGUILayout.PropertyField(action, actionLabel, true);

            //obj.actionType = (AndroidActionController.ActionType)EditorGUILayout.EnumPopup("Action Type", obj.actionType);
            EditorGUILayout.PropertyField(actionType, actionTypeLabel, true);

            switch (obj.actionType)
            {
                case AndroidActionController.ActionType.URI:
                    //obj.uri = EditorGUILayout.TextField("URI", obj.uri);
                    EditorGUILayout.PropertyField(uri, uriLabel, true);
                    break;

                case AndroidActionController.ActionType.ExtraQuery:
                    //obj.extra = EditorGUILayout.TextField("Extra", obj.extra);
                    EditorGUILayout.PropertyField(extra, extraLabel, true);

                    //obj.query = EditorGUILayout.TextField("Query", obj.query);
                    EditorGUILayout.PropertyField(query, queryLabel, true);
                    break;

                case AndroidActionController.ActionType.CHOOSER:
                    //obj.extra = EditorGUILayout.TextField("Extra", obj.extra);
                    EditorGUILayout.PropertyField(extra, extraLabel, true);
                    
                    //obj.query = EditorGUILayout.TextField("Query", obj.query);
                    EditorGUILayout.PropertyField(query, queryLabel, true);
                    
                    //obj.mimetype = EditorGUILayout.TextField("MIME Type", obj.mimetype);
                    EditorGUILayout.PropertyField(mimetype, mimetypeLabel, true);
                    
                    //obj.title = EditorGUILayout.TextField("Chooser Title", obj.title);
                    EditorGUILayout.PropertyField(title, titleLabel, true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
