using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Bluetooth Setting Controller
    ///･Show the dialog that request to enable bluetooth.
    ///
    ///・Bluetooth の接続要求ダイアログを表示する。
    /// </summary>
    public class BluetoothSettingController : MonoBehaviour
    {
        //Callbacks
        //Callback when receive result.         //結果受信時のコールバック
        [Serializable] public class ResultHandler : UnityEvent<bool> { }    //isOn
        public ResultHandler OnResult;

        //Callback when error occurrence.       //エラー時のコールバック
        [Serializable] public class ErrorHandler : UnityEvent<string> { }   //error state messate
        public ErrorHandler OnError;


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Request to enable Bluetooth
        public void StartRequest()
        {
#if UNITY_EDITOR
            Debug.Log("BluetoothSettingController.StartRequest called");
#elif UNITY_ANDROID
            AndroidPlugin.StartBluetoothRequestEnable(gameObject.name, "ReceiveResult");
#endif
        }


        //Callback handler when receive result
        private void ReceiveResult(string result)
        {
            if (result.StartsWith("SUCCESS"))
            {
                if (OnResult != null)
                    OnResult.Invoke(true);
            }
            else if (result.StartsWith("CANCEL"))
            {
                if (OnResult != null)
                    OnResult.Invoke(false);
            }
            else
            {
                if (OnError != null)
                    OnError.Invoke(result);
            }
        }
    }
}
