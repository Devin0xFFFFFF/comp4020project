using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Multi Choice Dialog Controller
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///･Note: 'SaveValue' is better not to use it when dynamically changing items (SetItem(), Show(strnig[])). It becomes incompatible with saved value.
    ///･Note: When using value save (saveValue), it is better to give a specific save name (saveKey) individually.
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class MultiChoiceDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                  //Dialog title

        [Serializable]
        public class Item
        {
            public string text = "";                    //Text for each item
            public string value = "";                   //Value for each item
            public bool isOn = false;                   //Selected item

            public Item() { }
            public Item(string text, string value, bool isOn = false)
            {
                this.text = text;
                this.value = value;
                this.isOn = isOn;
            }

            public Item Clone()
            {
                return (Item)MemberwiseClone();
            }
        }
        [SerializeField] private Item[] items;          //Choices

        //Callback value type
        [Serializable] public enum ResultType { Index, Text, Value }
        public ResultType resultType = ResultType.Value;

        public string okButton = "OK";                  //Text of 'OK' button.
        public string cancelButton = "Cancel";          //Text of 'Cancel' button.

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Save PlayerPrefs Settings
        public bool saveValue = false;                  //Whether to save the seleted index (Also local value is always overwritten). It is better not to use it when dynamically changing items (It becomes incompatible with saved value).
        [SerializeField] private string saveKey = "";   //When specifying the PlayerPrefs key.

        //Callbacks
        [Serializable] public class ResultHandler : UnityEvent<string[]> { }    //texts or values
        public ResultHandler OnResult;

        [Serializable] public class ResultIndexHandler : UnityEvent<int[]> { }  //indexes
        public ResultIndexHandler OnResultIndex;

        [Serializable] public class ValueChangedHandler : UnityEvent<string[]> { }  //texts or values
        public ValueChangedHandler OnValueChanged;

        [Serializable] public class ValueIndexChangedHandler : UnityEvent<int[]> { }  //indexes
        public ValueIndexChangedHandler OnValueIndexChanged;

#region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveKey is empty)
        const string SELECTED_PREF = "_selected";   //For selected state array

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_selected'")
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + SELECTED_PREF : saveKey; }
        }

        //Load local values manually.
        public void LoadPrefs()
        {
            bool[] selected = XPlayerPrefs.GetArray(SaveKey, IsOn);
            SetOn(selected);
        }

        //Save local values manually.
        //･To be saved value is only the 'isOn' state array.
        public void SavePrefs()
        {
            XPlayerPrefs.SetArray(SaveKey, IsOn);   //bool[]
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

        //Convert the local selected state (Item.isOn) to an array.
        //･If saveValue is true, it will be automatically saved.
        public bool[] CurrentValue {
            get { return IsOn; }
            set {
                SetOn(value);
                if (saveValue)
                    SavePrefs();
            }
        }

        //Propeties
        public string[] Texts {
            get { return items.Select(e => e.text).ToArray(); }
        }

        public string[] Values {
            get { return items.Select(e => e.value).ToArray(); }
        }

        public bool[] IsOn {
            get { return items.Select(e => e.isOn).ToArray(); }
        }

        //Create arrays to be arguments of dialogs at once.
        private void GetItemArrays(out string[] texts, out string[] values, out bool[] checkedItems)
        {
            texts = new string[items.Length];
            values = new string[items.Length];
            checkedItems = new bool[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                texts[i] = items[i].text;
                values[i] = items[i].value;
                checkedItems[i] = items[i].isOn;
            }
        }

        //Set to local values by selected array
        //･It is substituted in order from the top (Those less than the length are ignored).
        private void SetOn(params bool[] selected)
        {
            if (selected == null)
                return;

            int len = Mathf.Min(selected.Length, items.Length);
            for (int i = 0; i < len; i++)
                items[i].isOn = selected[i];
        }

        //Set to local values by selected index array (callback value directly).
        //･Turn on elements specified by index and turn off others.
        private void SetOn(params int[] indexes)
        {
            if (indexes == null)
                indexes = new int[0];   //Length=0 -> all false

            HashSet<int> set = new HashSet<int>(indexes);
            for (int i = 0; i < items.Length; i++)
                items[i].isOn = set.Contains(i);
        }


        //Set items dynamically (current items will be overwritten)
        //Note: When changed dynamically, it is inconsistent with the value saved in Playerprefs (better to use saveValue is false).
        //Note: When the resultType is 'Value', the value becomes the index of string type.
        //Note: Empty and duplication are not checked.
        public void SetItem(string[] texts, bool isOn)
        {
            if (texts == null)
                return;

            items = new Item[texts.Length];
            for (int i = 0; i < texts.Length; i++)
                items[i] = new Item(texts[i], i.ToString(), isOn);  //value is empty -> index (string type)
        }

        //･overload (for callback on inspecter)
        public void SetItem(string[] texts)
        {
            SetItem(texts, false);  //all off
        }

        //Set items dynamically (current items will be overwritten)
        //Note: When changed dynamically, it is inconsistent with the value saved in PlayerPrefs.
        //Note: Empty and duplication are not checked.
        public void SetItem(Item[] items)
        {
            if (items == null)
                return;

            this.items = items;
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
            
            DeletePrefs();
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
                if (resultType == ResultType.Value)
                {
                    HashSet<string> set = new HashSet<string>();
                    foreach (var item in items)
                    {
                        if (!string.IsNullOrEmpty(item.value))
                            set.Add(item.value);
                    }
                    if (set.Count != items.Length)
                        Debug.LogWarning("[" + gameObject.name + "] There is empty or duplicate 'Value'.");
                }
                else if (resultType == ResultType.Text)
                {
                    HashSet<string> set = new HashSet<string>();
                    foreach (var item in items)
                    {
                        if (!string.IsNullOrEmpty(item.text))
                            set.Add(item.text);
                    }
                    if (set.Count != items.Length)
                        Debug.LogWarning("[" + gameObject.name + "] There is empty or duplicate 'Text'.");
                }
            }

            //Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy.
            //Note: Search only within the same type.
            MultiChoiceDialogController[] objs = FindObjectsOfType<MultiChoiceDialogController>();
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
            Debug.Log("MultiChoiceDialogController.Show called");
#elif UNITY_ANDROID
            string[] texts; string[] values; bool[] checkedItems;
            GetItemArrays(out texts, out values, out checkedItems);

            AndroidPlugin.ShowMultiChoiceDialog(
                title,
                texts,
                checkedItems,
                gameObject.name, "ReceiveResult", "ReceiveChanged",
                true,   //For internal processing, only uses Index type.
                okButton, cancelButton,
                style);
#endif
        }

        //Set items dynamically and show dialog (current items will be overwritten).
        //Note: When the resultType is 'Value', the value becomes the index of string type.
        //Note: When changed dynamically, it is inconsistent with the value saved in Playerprefs (better to use saveValue is false).
        //Note: Empty and duplication are not checked.
        public void Show(string[] texts)
        {
            SetItem(texts, false);  //all off
            Show();
        }


        //Returns value when 'OK' pressed.
        private void ReceiveResult(string result)
        {
            int[] indexes;
            if (!string.IsNullOrEmpty(result))
            {
                try {
                    indexes = result.Split('\n').Select(e => int.Parse(e)).ToArray(); //For internal processing, only uses Index type.
                }
                catch (Exception) {
                    return;
                }
            }
            else
                indexes = new int[0];       //Necessary for SetOn() -> all off

            if (saveValue)
            {
                SetOn(indexes);
                SavePrefs();
            }

            switch (resultType)
            {
                case ResultType.Index:
                    if (OnResultIndex != null)
                        OnResultIndex.Invoke(indexes);
                    break;
                case ResultType.Text:
                    if (OnResult != null)
                        OnResult.Invoke(indexes.Select(i => items[i].text).ToArray());
                    break;
                case ResultType.Value:
                    if (OnResult != null)
                        OnResult.Invoke(indexes.Select(i => items[i].value).ToArray());
                    break;
            }
        }

        //Returns value when choice.
        public void ReceiveChanged(string result)
        {
            int[] indexes;
            if (!string.IsNullOrEmpty(result))
            {
                try {
                    indexes = result.Split('\n').Select(e => int.Parse(e)).ToArray(); //For internal processing, only uses Index type.
                }
                catch (Exception) {
                    return;
                }
            }
            else
                indexes = new int[0];       //Necessary for all empty

            switch (resultType)
            {
                case ResultType.Index:
                    if (OnValueIndexChanged != null)
                        OnValueIndexChanged.Invoke(indexes);
                    break;
                case ResultType.Text:
                    if (OnValueChanged != null)
                        OnValueChanged.Invoke(indexes.Select(i => items[i].text).ToArray());
                    break;
                case ResultType.Value:
                    if (OnValueChanged != null)
                        OnValueChanged.Invoke(indexes.Select(i => items[i].value).ToArray());
                    break;
            }
        }
    }
}
