using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Custom Dialog Controller
    /// http://fantom1x.blog130.fc2.com/blog-entry-290.html
    /// (Usage)
    ///･When 'Find Items In Child' is on, put Prefab of 'DialogItemDivisor', 'DialogItemText', 'DialogItemSlider', 'DialogItemSwitch', 'DialogItemToggles' in transform.child of GameObject to which CustomDialogController is attached.
    ///･When 'Find Items In Child' is off, register Prefab of 'DialogItemXXX'(XXX:widget type) to 'Items' in inspector.
    /// (Notes)
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///･Note: 'SaveValue' is better not to use it when dynamically changing items (SetItem()). It becomes incompatible with saved value.
    ///･Note: When using value save (saveValue), it is better to give a specific save name (saveKey) individually.
    /// (Theme[Style])
    /// https://developer.android.com/reference/android/R.style.html#Theme
    /// </summary>
    public class CustomDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string title = "Title";                          //Dialog title
        public string message = "";                             //Dialog message (It should be empty when overflowing)

        public bool findItemsInChild = true;                    //true: Find items in 'transform.GetChild()' / false: Inspector settings
        [SerializeField] private DialogItemParameter[] items;   //Use only in the inspector (When internal processing, It is converted to dialogItems and used).

        public string okButton = "OK";                          //Text of 'OK' button.
        public string cancelButton = "Cancel";                  //Text of 'Cancel' button.

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        //Save PlayerPrefs Settings
        public bool saveValue = false;                  //Whether to save the 'key=value' pair (Also local value is always overwritten). It is better not to use it when dynamically changing items (It becomes incompatible with saved value).
        [SerializeField] private string saveKey = "";   //When specifying the PlayerPrefs key.

        //Callbacks
        [Serializable] public class ResultHandler : UnityEvent<Dictionary<string, string>> { }  //key, value
        public ResultHandler OnResult;

        [Serializable] public class ValueChangedHandler : UnityEvent<string, string> { }    //key, value
        public ValueChangedHandler OnValueChanged;

#region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveKey is empty)
        const string VALUE_PREF = "_values";

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_values'")
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + VALUE_PREF : saveKey; }
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
            XPlayerPrefs.SetDictionary(SaveKey, GetValue());    //compatible 'Param' class
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

        //Convert the local value to Dictionary <item.key, value>
        //･If saveValue is true, it will be automatically saved.
        public Dictionary<string, string> CurrentValue {
            get { return GetValue(); }
            set {
                SetValue(value);
                if (saveValue)
                    SavePrefs();
            }
        }

        //Local value (dialogItems) -> Dictionary <string, string> (compatible 'Param' class)
        private Dictionary<string, string> GetValue()
        {
            Param param = new Param();
            if (dialogItems != null)
            {
                foreach (var elem in keyToIndex)
                {
                    int i = elem.Value;
                    string type = dialogItems[i].type;
                    if (type == "Switch")
                        param[elem.Key] = ((SwitchItem)dialogItems[i]).defChecked.ToString();
                    else if (type == "Slider")
                        param[elem.Key] = ((SliderItem)dialogItems[i]).value.ToString();
                    else if (type == "Toggle")
                        param[elem.Key] = ((ToggleItem)dialogItems[i]).defValue;
                }
            }
            return param;
        }

        //Dictionary <string, string> -> Local value (dialogItems) (compatible 'Param' class)
        //･Note: Nonexistent keys and type mismatch are ignored.
        private void SetValue(Dictionary<string, string> dic)
        {
            if (dic == null || dialogItems == null)
                return;

            foreach (var item in dic)
            {
                if (keyToIndex.ContainsKey(item.Key))
                {
                    int i = keyToIndex[item.Key];
                    try
                    {
                        string type = dialogItems[i].type;
                        if (type == "Switch")
                            ((SwitchItem)dialogItems[i]).defChecked = bool.Parse(item.Value);
                        else if (type == "Slider")
                            ((SliderItem)dialogItems[i]).value = float.Parse(item.Value);
                        else if (type == "Toggle")
                            ((ToggleItem)dialogItems[i]).defValue = item.Value;
                    }
                    catch (Exception) { }
                }
            }
        }


        //Set items(dialogItems) dynamically (current items will be overwritten)
        //Note: When changed dynamically, it is inconsistent with the value saved in PlayerPrefs (better to use saveValue is false).
        //Note: Empty and duplication are not checked.
        public void SetItem(DialogItem[] dialogItems)
        {
            if (dialogItems == null)
                return;

            this.dialogItems = dialogItems;

            keyToIndex.Clear();
            for (int i = 0; i < dialogItems.Length; i++)
            {
                if (!string.IsNullOrEmpty(dialogItems[i].key))
                    keyToIndex[dialogItems[i].key] = i;         //same as MakeKeyToIndexTable()

                var item = dialogItems[i];
                switch (item.type)
                {
                    case "Switch":
                        ((SwitchItem)dialogItems[i]).changeCallbackMethod = "ReceiveChanged";
                        break;
                    case "Slider":
                        ((SliderItem)dialogItems[i]).changeCallbackMethod = "ReceiveChanged";
                        break;
                    case "Toggle":
                        ((ToggleItem)dialogItems[i]).changeCallbackMethod = "ReceiveChanged";
                        break;
                    default:
                        //Divisor, Text
                        break;
                }
            }
        }


        //Convert items -> dialogItems (Local Values)
        private DialogItem[] dialogItems;

        //The values for reset.
        private DialogItem[] initValue;

        //Inspecter value (DialogItemParameter) -> Local value (DialogItem).
        //(*) When internal processing, it is used dialogItems.
        //･Make 'key -> index' table
        //･Store the values of the inspector (initValue).
        //･Check for key exist.
        private void ConvertToDialogItem()
        {
            if (items == null || items.Length == 0)     //from inspector
                return;

            dialogItems = new DialogItem[items.Length];
            initValue = new DialogItem[items.Length];
            keyToIndex.Clear();
            int keyCount = 0;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                switch (item.type)
                {
                    case DialogItemType.Divisor:
                        dialogItems[i] = new DivisorItem(item.lineHeight, item.lineColor);
                        initValue[i] = ((DivisorItem)dialogItems[i]).Clone();
                        break;
                    case DialogItemType.Text:
                        string align = item.align.ToString().ToLower();
                        dialogItems[i] = new TextItem(item.text, item.textColor, item.backgroundColor, align == "none" ? "" : align);
                        initValue[i] = ((TextItem)dialogItems[i]).Clone();
                        break;
                    case DialogItemType.Switch:
                        dialogItems[i] = new SwitchItem(item.text, item.key, item.defaultChecked, item.textColor, "ReceiveChanged");
                        initValue[i] = ((SwitchItem)dialogItems[i]).Clone();
                        if (!string.IsNullOrEmpty(item.key))
                            keyToIndex[item.key] = i;
                        keyCount++;
                        break;
                    case DialogItemType.Slider:
                        dialogItems[i] = new SliderItem(item.text, item.key, item.value, item.minValue, item.maxValue, item.digit, item.textColor, "ReceiveChanged");
                        initValue[i] = ((SliderItem)dialogItems[i]).Clone();
                        if (!string.IsNullOrEmpty(item.key))
                            keyToIndex[item.key] = i;
                        keyCount++;
                        break;
                    case DialogItemType.Toggle:
                        dialogItems[i] = new ToggleItem(item.TogglesTexts, item.key, item.TogglesValues, item.toggleItems[item.checkedIndex].value, item.textColor, "ReceiveChanged");
                        initValue[i] = ((ToggleItem)dialogItems[i]).Clone();
                        if (!string.IsNullOrEmpty(item.key))
                            keyToIndex[item.key] = i;
                        keyCount++;
                        break;
                }
            }
#if UNITY_EDITOR
            //Check for errors (Editor only).
            if (keyCount != keyToIndex.Count)
                Debug.LogError("[" + gameObject.name + "] There is empty or duplicate 'Key'.");
#endif
        }

        //Restore the values of the inspector and delete PlayerPrefs key.
        public void ResetValue()
        {
            dialogItems = (DialogItem[])initValue.Clone();

            for (int i = 0; i < initValue.Length; i++)
            {
                string type = initValue[i].type;
                if (type == "Divisor")
                    dialogItems[i] = ((DivisorItem)initValue[i]).Clone();
                else if (type == "Text")
                    dialogItems[i] = ((TextItem)initValue[i]).Clone();
                else if (type == "Switch")
                    dialogItems[i] = ((SwitchItem)initValue[i]).Clone();
                else if (type == "Slider")
                    dialogItems[i] = ((SliderItem)initValue[i]).Clone();
                else if (type == "Toggle")
                    dialogItems[i] = ((ToggleItem)initValue[i]).Clone();
            }

            MakeKeyToIndexTable();
            DeletePrefs();
        }


        //Detects 'DialogItemParameter' from its 'transform.GetChild()' element (not search for grandchildren.).
        private void FindItemsInChild()
        {
            int len = transform.childCount;
            if (len == 0)
                return;

            List<DialogItemParameter> list = new List<DialogItemParameter>(len);
            for (int i = 0; i < len; i++)
            {
                DialogItemParameter dlgParam = transform.GetChild(i).GetComponent<DialogItemParameter>();
                if (dlgParam != null)
                    list.Add(dlgParam);
            }

            if (list.Count > 0)
                items = list.ToArray();     //overwrite inspector value
        }


        //'key -> index' convert table (Divisor and Text do not have a key)
        private Dictionary<string, int> keyToIndex = new Dictionary<string, int>();

        //Make 'key -> index' table
        private void MakeKeyToIndexTable()
        {
            keyToIndex.Clear();
            for (int i = 0; i < dialogItems.Length; i++)
            {
                if (!string.IsNullOrEmpty(dialogItems[i].key))
                    keyToIndex[dialogItems[i].key] = i;
            }
        }


        //Check empty or duplication from item elements.
        private void CheckForErrors()
        {
            if (dialogItems == null || dialogItems.Length == 0)
            {
                Debug.LogWarning("[" + gameObject.name + "] 'Items' is empty.");
            }

            //Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy.
            //Note: Search only within the same type.
            CustomDialogController[] objs = FindObjectsOfType<CustomDialogController>();
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
            if (findItemsInChild)
                FindItemsInChild();

            ConvertToDialogItem();

            if (saveValue)
                LoadPrefs();
        }

        private void Start()
        {
#if UNITY_EDITOR
            CheckForErrors();   //Check for items(dialogItems) (Editor only).
#endif
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        
        //Show dialog
        public void Show()
        {
            if (dialogItems == null || dialogItems.Length == 0)
                return;
#if UNITY_EDITOR
            Debug.Log("CustomDialogController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowCustomDialog(
                title,
                message,
                dialogItems,
                gameObject.name, "ReceiveResult",
                false,          //"key=value" format
                okButton, cancelButton,
                style);
#endif
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
                OnResult.Invoke(param);     //Param and Dictionary are compatible.
        }

        //Returns value when slider moving.
        public void ReceiveChanged(string result)
        {
            if (OnValueChanged != null)
            {
                string[] arr = result.Split('=');
                OnValueChanged.Invoke(arr[0], arr[1]);
            }
        }
    }
}
