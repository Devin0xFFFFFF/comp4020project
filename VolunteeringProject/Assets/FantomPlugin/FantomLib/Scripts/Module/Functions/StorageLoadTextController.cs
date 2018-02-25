using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Storage Load Text Controller
    /// (*)API 19 [Android4.4] or later
    /// 
    /// Read a text file with the Storage Access Framework (API 19 [Android4.4] or later).
    ///·Select a text file with something like a system explorer and return the loaded text.
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///
    /// ストレージアクセスフレームワークでテキストファイルを読み込む (API 19 [Android4.4] 以上)
    ///・システムのエクスプローラのようなものでテキストファイルを選択し、ロードしたテキストを返す。
    /// </summary>
    public class StorageLoadTextController : MonoBehaviour
    {
        //Callbacks
        //Callback when loaded text (UTF-8).    //ロード成功時のコールバック
        [Serializable] public class ResultHandler : UnityEvent<string> { }    //loaded text
        public ResultHandler OnResult;

        //Callback when error.                  //エラー時のコールバック
        [Serializable] public class ErrorHandler : UnityEvent<string> { }    //error message
        public ErrorHandler OnError;



        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}



        //Call the default storage access framework (like explorer).
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("StorageLoadTextController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.OpenStorageAndLoadText(gameObject.name, "ReceiveResult", "ReceiveError");
#endif
        }


        //Callback handler when load text success
        private void ReceiveResult(string text)
        {
            if (OnResult != null)
                OnResult.Invoke(text);
        }

        //Callback handler when error
        private void ReceiveError(string message)
        {
            if (OnError != null)
                OnError.Invoke(message);
        }
    }
}
