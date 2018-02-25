using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Storage Save Text Controller
    /// (*)API 19 [Android4.4] or later
    /// 
    /// Write a text file with the Storage Access Framework (API 19 [Android4.4] or later).
    ///·Select a text file with something like a system explorer and return the loaded text.
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///･Note: When using value save (saveValue), it is better to give a specific save name (saveKey) individually.
    ///
    /// ストレージアクセスフレームワークでテキストファイルを読み込む (API 19 [Android4.4] 以上)
    ///・システムのエクスプローラのようなものでテキストファイルを選択し、ロードしたテキストを返す。
    ///※Android → Unity への結果コールバックは「GameObject.name」で受信されるため、ヒエラルキー上ではユニークが良い。
    ///※値の保存（saveValue）を利用するときは、特定の保存名（saveKey）を個々に与えた方が良い。
    /// </summary>
    public class StorageSaveTextController : MonoBehaviour
    {
        //Inspector settings
        public string fileName = DEFAULT_FILENAME;      //FileName to save (not include directory path). //保存するファイル名（ディレクトリパスは含まない）

        //Save PlayerPrefs Settings
        public bool saveValue = false;                  //Whether to save the last fileName (Also local value is always overwritten).
        [SerializeField] private string saveKey = "";   //When specifying the PlayerPrefs key for text.     //特定の保存名を付けるとき


        //Callbacks
        //Callback when loaded text (UTF-8).            //ロード成功時のコールバック
        [Serializable] public class ResultHandler : UnityEvent<string> { }    //saved file name (not include directory path)
        public ResultHandler OnResult;

        //Callback when error.                          //エラー時のコールバック
        [Serializable] public class ErrorHandler : UnityEvent<string> { }    //error message
        public ErrorHandler OnError;

#region PlayerPrefs Section

        //Defalut PlayerPrefs Key (It is used only when saveKey is empty)
        const string FILE_PREF = "_file";

        //Saved key in PlayerPrefs (Default key is "gameObject.name + '_file'")
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + FILE_PREF : saveKey; }
        }

        //Load local values manually.
        public void LoadPrefs()
        {
            fileName = PlayerPrefs.GetString(SaveKey, fileName);
        }

        //Save local values manually.
        public void SavePrefs()
        {
            PlayerPrefs.SetString(SaveKey, fileName);
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

        const string DEFAULT_FILENAME = "NewDocument.txt";

        //The currently (default) fileName.
        //･If saveValue is true, it will be automatically saved.
        public string CurrentValue {
            get { return fileName; }
            set {
                fileName = string.IsNullOrEmpty(value) ? DEFAULT_FILENAME : value;
                if (saveValue)
                    SavePrefs();
            }
        }

        //Alias for CurrentValue
        public string FileName {
            get { return CurrentValue; }
            set { CurrentValue = value; }
        }

#endregion

        // Use this for initialization
        private void Awake()
        {
            if (saveValue)
                LoadPrefs();
        }

        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        

        //Call the default storage access framework (like explorer).
        public void Show(string text)
        {
#if UNITY_EDITOR
            Debug.Log("StorageSaveTextController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.OpenStorageAndSaveText(fileName, text, gameObject.name, "ReceiveResult", "ReceiveError");
#endif
        }

        //Call the default storage access framework (like explorer).
        //Set fileName dynamically (current value will be overwritten).
        public void Show(string fileName, string text)
        {
            this.fileName = fileName;
#if UNITY_EDITOR
            Debug.Log("StorageSaveTextController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.OpenStorageAndSaveText(fileName, text, gameObject.name, "ReceiveResult", "ReceiveError");
#endif
        }


        //Callback handler when save text success
        private void ReceiveResult(string fileName)
        {
            if (saveValue)
            {
                this.fileName = fileName;
                SavePrefs();
            }

            if (OnResult != null)
                OnResult.Invoke(fileName);
        }

        //Callback handler when error
        private void ReceiveError(string message)
        {
            if (OnError != null)
                OnError.Invoke(message);
        }
    }
}
