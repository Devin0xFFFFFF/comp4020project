using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Single Choice Dialog Controller
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///･Note: 'SaveValue' is better not to use it when dynamically changing items (SetItem(), Show(strnig[])). It becomes incompatible with saved value.
    ///･Note: When using value save (saveValue), it is better to give a specific save name (saveKey) individually.
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class SingleChoiceDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                  //Dialog title

        [Serializable]
        public class Item
        {
            public string text = "";                    //Text for each item
            public string value = "";                   //Value for each item

            public Item() { }
            public Item(string text, string value)
            {
                this.text = text;
                this.value = value;
            }

            public Item Clone()
            {
                return (Item)MemberwiseClone();
            }
        }
        [SerializeField] private Item[] items;          //Choices

        [SerializeField] private int selectedIndex = 0; //Default selected index

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
        [Serializable] public class ResultHandler : UnityEvent<string> { }      //text or value
        public ResultHandler OnResult;

        [Serializable] public class ResultIndexHandler : UnityEvent<int> { }    //index
        public ResultIndexHandler OnResultIndex;

        [Serializable] public class ValueChangedHandler : UnityEvent<string> { }  //text or value
        public ValueChangedHandler OnValueChanged;

        [Serializable] public class ValueIndexChangedHandler : UnityEvent<int> { }  //index
        public ValueIndexChangedHandler OnValueIndexChanged;

#region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveKey is empty)
        const string SELECTED_PREF = "_selected";   //For selected index

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_selected'")
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + SELECTED_PREF : saveKey; }
        }

        //Load local values manually.
        public void LoadPrefs()
        {
            selectedIndex = Mathf.Clamp(PlayerPrefs.GetInt(SaveKey, selectedIndex), 0, items.Length -1);
        }

        //Save local values manually.
        //･To be saved value is only selected index.
        public void SavePrefs()
        {
            PlayerPrefs.SetInt(SaveKey, selectedIndex);
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

        //The currently (default) selected index.
        //･If saveValue is true, it will be automatically saved.
        public int CurrentValue {
            get { return selectedIndex; }
            set {
                selectedIndex = Mathf.Clamp(value, 0, items.Length - 1);
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


        //Set items dynamically (current items will be overwritten)
        //･The selected index is 0. It can be changed by 'CurrentValue' property.
        //Note: When changed dynamically, it is inconsistent with the value saved in Playerprefs (better to use saveValue is false).
        //Note: When the resultType is 'Value', the value becomes the index of string type.
        //Note: Empty and duplication are not checked.
        public void SetItem(string[] texts)
        {
            if (texts == null)
                return;

            items = new Item[texts.Length];
            for (int i = 0; i < texts.Length; i++)
                items[i] = new Item(texts[i], i.ToString());  //value is empty -> index (string type)

            selectedIndex = 0;
        }

        //Set items dynamically (current items will be overwritten)
        //･The selected index is 0. It can be changed by 'CurrentValue' property.
        //Note: When changed dynamically, it is inconsistent with the value saved in PlayerPrefs (better to use saveValue is false).
        //Note: Empty and duplication are not checked.
        public void SetItem(Item[] items)
        {
            if (items == null)
                return;

            this.items = items;
            selectedIndex = 0;
        }


        //The value for reset.
        private Item[] initValue;
        private int initSelectIndex;

        //Store the values of the inspector.
        private void StoreInitValue()
        {
            initValue = (Item[])items.Clone();
            for (int i = 0; i < items.Length; i++)
                initValue[i] = items[i].Clone();

            initSelectIndex = selectedIndex;
        }

        //Restore the value of inspector and delete PlayerPrefs key.
        public void ResetValue()
        {
            items = (Item[])initValue.Clone();
            for (int i = 0; i < initValue.Length; i++)
                items[i] = initValue[i].Clone();

            selectedIndex = initSelectIndex;
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
            SingleChoiceDialogController[] objs = FindObjectsOfType<SingleChoiceDialogController>();
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
            selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Length - 1);
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
            Debug.Log("SingleChoiceDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowSingleChoiceDialog(
                title,
                Texts,
                Mathf.Clamp(selectedIndex, 0, items.Length - 1),
                gameObject.name, "ReceiveResult", "ReceiveChanged",
                true,   //For internal processing, only uses Index type.
                okButton, cancelButton,
                style);
#endif
        }


        //Set items dynamically and show dialog (current items will be overwritten)
        //Note: When the resultType is 'Value', the value becomes the index of string type.
        //Note: When changed dynamically, it is inconsistent with the value saved in Playerprefs (better to use saveValue is false).
        //Note: Empty and duplication are not checked.
        public void Show(string[] texts)
        {
            SetItem(texts);
            Show();
        }


        //Returns value when 'OK' pressed.
        private void ReceiveResult(string result)
        {
            int index;      //For internal processing, only uses Index type.
            if (!int.TryParse(result, out index))
                return;

            if (saveValue)
            {
                selectedIndex = index;
                SavePrefs();
            }

            switch (resultType)
            {
                case ResultType.Index:
                    if (OnResultIndex != null)
                        OnResultIndex.Invoke(index);
                    break;
                case ResultType.Text:
                    if (OnResult != null)
                        OnResult.Invoke(items[index].text);
                    break;
                case ResultType.Value:
                    if (OnResult != null)
                        OnResult.Invoke(items[index].value);
                    break;
            }
        }

        //Returns value when choice.
        public void ReceiveChanged(string result)
        {
            int index;      //For internal processing, only uses Index type.
            if (!int.TryParse(result, out index))
                return;

            switch (resultType)
            {
                case ResultType.Index:
                    if (OnValueIndexChanged != null)
                        OnValueIndexChanged.Invoke(index);
                    break;
                case ResultType.Text:
                    if (OnValueChanged != null)
                        OnValueChanged.Invoke(items[index].text);
                    break;
                case ResultType.Value:
                    if (OnValueChanged != null)
                        OnValueChanged.Invoke(items[index].value);
                    break;
            }
        }
    }
}
