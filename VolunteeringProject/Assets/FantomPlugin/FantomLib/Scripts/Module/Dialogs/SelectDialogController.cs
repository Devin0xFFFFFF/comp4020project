using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Select Dialog Controller
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class SelectDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";              //Dialog title

        [Serializable]
        public class Item
        {
            public string text = "";                //Text for each item
            public string value = "";               //Value for each item

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
        [SerializeField] private Item[] items;      //Choices

        //Callback value type
        [Serializable] public enum ResultType { Index, Text, Value }
        public ResultType resultType = ResultType.Value;

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Callbacks
        [Serializable] public class ResultHandler : UnityEvent<string> { }      //text or value
        public ResultHandler OnResult;

        [Serializable] public class ResultIndexHandler : UnityEvent<int> { }    //index
        public ResultIndexHandler OnResultIndex;

#region Properties and Local values Section

        //Propeties
        public string[] Texts {
            get { return items.Select(e => e.text).ToArray(); }
        }

        public string[] Values {
            get { return items.Select(e => e.value).ToArray(); }
        }


        //Set items dynamically (current items will be overwritten)
        //Note: When the resultType is 'Value', the value becomes the index of string type.
        //Note: Empty and duplication are not checked.
        public void SetItem(string[] texts)
        {
            if (texts == null)
                return;

            items = new Item[texts.Length];
            for (int i = 0; i < texts.Length; i++)
                items[i] = new Item(texts[i], i.ToString());  //value is empty -> index (string type)
        }

        //Set items dynamically (current items will be overwritten)
        //Note: Empty and duplication are not checked.
        public void SetItem(Item[] items)
        {
            if (items == null)
                return;

            this.items = items;
        }


        //The value for reset.
        private Item[] initValue;

        //Store the values of the inspector.
        private void StoreInitValue()
        {
            initValue = (Item[])items.Clone();
            for (int i = 0; i < items.Length; i++)
                initValue[i] = items[i].Clone();
        }

        //Restore the value of inspector.
        public void ResetValue()
        {
            items = (Item[])initValue.Clone();
            for (int i = 0; i < initValue.Length; i++)
                items[i] = initValue[i].Clone();
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
            SelectDialogController[] objs = FindObjectsOfType<SelectDialogController>();
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

        
        //Show dialog with local values
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("SelectDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowSelectDialog(
                title,
                Texts,
                gameObject.name,
                "ReceiveResult",
                true,   //For internal processing, only uses Index type.
                style);
#endif
        }

        //Set items dynamically and show dialog (current items will be overwritten)
        //Note: When the resultType is 'Value', the value becomes the index of string type.
        //Note: Empty and duplication are not checked.
        public void Show(string[] texts)
        {
            SetItem(texts);
            Show();
        }


        //Returns value when choiced item.
        private void ReceiveResult(string result)
        {
            int index;      //For internal processing, only uses Index type.
            if (!int.TryParse(result, out index))
                return;

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
    }
}
