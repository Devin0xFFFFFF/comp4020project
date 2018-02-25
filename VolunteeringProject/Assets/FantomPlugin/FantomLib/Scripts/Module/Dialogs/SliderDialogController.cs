﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Slider Dialog Controller
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///･Note: 'SaveValue' is better not to use it when dynamically changing items (SetItem(), Show(strnig[])). It becomes incompatible with saved value.
    ///･Note: When using value save (saveValue), it is better to give a specific save name (saveKey) individually.
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class SliderDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                  //Dialog title
        public string message = "Message";              //Dialog message (It should be empty when overflowing)

        [Serializable]
        public class Item
        {
            public string text = "";                    //Text for each item
            public string key = "";                     //Identification key for each item
            public float value = 100;                   //Default (current) value
            public float minValue = 0;                  //Lower limit of value
            public float maxValue = 100;                //Upper limit of value
            [Range(0, 3)] public int digits = 0;        //The number of digits to the right of the decimal point (integer when 0).

            public Item() { }
            public Item(string text, string key, float value = 100, float minValue = 0, float maxValue = 100, int digits = 0)
            {
                this.text = text;
                this.key = key;
                this.value = Mathf.Clamp(value, minValue, maxValue);
                this.minValue = minValue;
                this.maxValue = maxValue;
                this.digits = Mathf.Clamp(digits, 0, 3);
            }

            public Item Clone()
            {
                return (Item)MemberwiseClone();
            }
        }
        [SerializeField] private Item[] items;          //All items

        public Color itemsTextColor = Color.black;      //Text color of all items

        public string okButton = "OK";                  //Text of 'OK' button.
        public string cancelButton = "Cancel";          //Text of 'Cancel' button.

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Save PlayerPrefs Settings
        public bool saveValue = false;                  //Whether to save the seleted (Also local value is always overwritten). It is better not to use it when dynamically changing items (It becomes incompatible with saved value).
        [SerializeField] private string saveKey = "";   //When specifying the PlayerPrefs key.

        //Callbacks
        [Serializable] public class ResultHandler : UnityEvent<Dictionary<string, float>> { }
        public ResultHandler OnResult;

        [Serializable] public class ValueChangedHandler : UnityEvent<string, float> { }
        public ValueChangedHandler OnValueChanged;

#region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveKey is empty)
        const string SLIDER_PREF = "_sliders";

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_sliders'")
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + SLIDER_PREF : saveKey; }
        }

        //Load local values manually.
        public void LoadPrefs()
        {
            Param param = Param.GetPlayerPrefs(SaveKey);
            SetValue(param);
        }

        //Save local values manually.
        //･To be saved value is only the 'key & value' pairs (Dictionary <string, string>).
        public void SavePrefs()
        {
            Dictionary<string, string> dic = items.ToDictionary(e => e.key, e => e.value.ToString());   //Duplicate key is not allowed.
            XPlayerPrefs.SetDictionary(SaveKey, dic);   //compatible 'Param' class
            PlayerPrefs.Save();
        }

        //Delete PlayerPrefs key.
        //Note: Local values are not initialized at this time.
        public void DeletePrefs()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }

        //Returns true if the saved value exists.
        public bool HasPrefs {
            get { return PlayerPrefs.HasKey(SaveKey); }
        }

#endregion

#region Properties and Local values Section

        //Convert the local values to Dictionary <item.key, item.value>
        //･If saveValue is true, it will be automatically saved.
        public Dictionary<string, float> CurrentValue {
            get { return items.ToDictionary(e => e.key, e => e.value); }    //Duplicate key is not allowed.
            set {
                SetValue(value);
                if (saveValue)
                    SavePrefs();
            }
        }

        //Propeties
        public string[] Texts {
            get { return items.Select(e => e.text).ToArray(); }
        }

        public string[] Keys {
            get { return items.Select(e => e.key).ToArray(); }
        }

        public float[] Values {
            get { return items.Select(e => e.value).ToArray(); }
        }

        public float[] MinValues {
            get { return items.Select(e => e.minValue).ToArray(); }
        }

        public float[] MaxValues {
            get { return items.Select(e => e.maxValue).ToArray(); }
        }

        public int[] Digits {
            get { return items.Select(e => e.digits).ToArray(); }
        }

        //Create arrays to be arguments of dialogs at once.
        private void GetItemArrays(out string[] texts, out string[] keys, 
            out float[] values, out float[] minValues, out float[] maxValues, out int[] digits)
        {
            texts = new string[items.Length];
            keys = new string[items.Length];
            values = new float[items.Length];
            minValues = new float[items.Length];
            maxValues = new float[items.Length];
            digits = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                texts[i] = items[i].text;
                keys[i] = items[i].key;
                values[i] = items[i].value;
                minValues[i] = items[i].minValue;
                maxValues[i] = items[i].maxValue;
                digits[i] = items[i].digits;
            }
        }

        //Param <item.key, (string)item.value> -> local values
        //･Note: Nonexistent keys and type mismatch are ignored.
        private void SetValue(Param param)
        {
            if (param == null)
                return;

            foreach (var item in param)
            {
                if (keyToIndex.ContainsKey(item.Key))
                {
                    float value;
                    if (float.TryParse(item.Value, out value))
                        items[keyToIndex[item.Key]].value = value;
                }
            }
        }

        //Dictionary <item.key, item.value> -> local values
        //･Note: Nonexistent keys are ignored.
        private void SetValue(Dictionary<string, float> dic)
        {
            if (dic == null)
                return;

            foreach (var item in dic)
            {
                if (keyToIndex.ContainsKey(item.Key))
                    items[keyToIndex[item.Key]].value = item.Value;
            }
        }


        //Set items dynamically (current items will be overwritten)
        //Note: When changed dynamically, it is inconsistent with the value saved in Playerprefs (better to use saveValue is false).
        //Note: The key becomes the index of string type.
        //Note: Empty and duplication are not checked.
        public void SetItem(string[] texts, float value)
        {
            if (texts == null)
                return;

            items = new Item[texts.Length];
            for (int i = 0; i < texts.Length; i++)
                items[i] = new Item(texts[i], i.ToString(), value);  //key is empty -> index (string type)

            MakeKeyToIndexTable();
        }

        //･overload (For callback registration in the inspector)
        public void SetItem(string[] texts)
        {
            SetItem(texts, 100);  //all values are 100
        }

        //Set items dynamically (current items will be overwritten)
        //Note: When changed dynamically, it is inconsistent with the value saved in PlayerPrefs (better to use saveValue is false).
        //Note: Empty and duplication are not checked.
        public void SetItem(Item[] items)
        {
            if (items == null)
                return;

            this.items = items;
            MakeKeyToIndexTable();
        }


        //The values for reset.
        private Item[] initValue;

        //Store the values of the inspector.
        private void StoreInitValue()
        {
            initValue = (Item[])items.Clone();
            for (int i = 0; i < items.Length; i++)
                initValue[i] = items[i].Clone();
        }

        //Restore the values of the inspector and delete PlayerPrefs key.
        public void ResetValue()
        {
            items = (Item[])initValue.Clone();
            for (int i = 0; i < initValue.Length; i++)
                items[i] = initValue[i].Clone();

            MakeKeyToIndexTable();
            DeletePrefs();
        }
        

        //'key -> index' convert table
        private Dictionary<string, int> keyToIndex = new Dictionary<string, int>();

        //Make 'key -> index' table
        private void MakeKeyToIndexTable()
        {
            keyToIndex.Clear();
            for (int i = 0; i < items.Length; i++)
            {
                if (!string.IsNullOrEmpty(items[i].key))
                    keyToIndex[items[i].key] = i;
            }
        }


        //Check empty or duplication from item elements.
        private void CheckForErrors()
        {
            if (items.Length == 0)
            {
                Debug.LogWarning("[" + gameObject.name + "] 'Items' is empty.");
            }
            else
            {
                if (keyToIndex.Count != items.Length)
                    Debug.LogError("[" + gameObject.name + "] There is empty or duplicate 'Key'.");

                HashSet<string> set = new HashSet<string>();
                foreach (var item in items)
                {
                    if (!string.IsNullOrEmpty(item.text))
                        set.Add(item.text);
                }
                if (set.Count != items.Length)
                    Debug.LogWarning("[" + gameObject.name + "] There is empty or duplicate 'Text'.");
            }

            //Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy.
            //Note: Search only within the same type.
            SliderDialogController[] objs = FindObjectsOfType<SliderDialogController>();
            if (objs.Length > 1)
            {
                HashSet<string> set = new HashSet<string>(objs.Select(e => e.gameObject.name).ToArray());
                if (set.Count != objs.Length)
                    Debug.LogError("[" + gameObject.name + "] There is duplicate 'gameObject.name'.");
            }
        }

#endregion

        // Use this for initialization
        private void Awake()
        {
            MakeKeyToIndexTable();
            StoreInitValue();

            if (saveValue)
                LoadPrefs();
        }

        private void Start()
        {
#if UNITY_EDITOR
            CheckForErrors();    //Check for items (Editor only).
#endif
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Show dialog
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("SliderDialogController.Show called");
#elif UNITY_ANDROID
            string[] texts; string[] keys; float[] values;
            float[] minValues; float[] maxValues; int[] digits;
            GetItemArrays(out texts, out keys, out values,
                out minValues, out maxValues, out digits);

            AndroidPlugin.ShowSliderDialog(
                title,
                message,
                texts,
                keys,
                values,
                minValues,
                maxValues,
                digits,
                itemsTextColor,
                gameObject.name, "ReceiveResult", "ReceiveChanged",
                okButton, cancelButton,
                style);
#endif
        }

        //Set items dynamically and show dialog (current items will be overwritten).
        //Note: When changed dynamically, it is inconsistent with the value saved in Playerprefs (better to use saveValue is false).
        //Note: Empty and duplication are not checked.
        public void Show(string[] texts)
        {
            SetItem(texts, 100);  //all values are 100
            Show();
        }


        //Returns value when 'OK' pressed.
        private void ReceiveResult(string result)
        {
            Param param = Param.Parse(result);  //Parse: "key1=value1\nkey2=value2" -> same as Dictionary<key, value> (Param)
            if (param == null)
                return;

            if (saveValue)
            {
                SetValue(param);
                Param.SetPlayerPrefs(SaveKey, param);
                PlayerPrefs.Save();
            }

            if (OnResult != null)
            {
                try {
                    OnResult.Invoke(param.Select(e => new { k = e.Key, v = float.Parse(e.Value) }).ToDictionary(a => a.k, a => a.v));
                }
                catch (Exception) { }
            }
        }

        //Returns value when slider moving.
        public void ReceiveChanged(string result)
        {
            if (OnValueChanged != null)
            {
                string[] arr = result.Split('=');
                float value;
                if (float.TryParse(arr[1], out value))
                    OnValueChanged.Invoke(arr[0], value);
            }
        }
    }
}
