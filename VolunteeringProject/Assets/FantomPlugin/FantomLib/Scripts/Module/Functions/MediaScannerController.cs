using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Media Scanner Controller
    /// 
    ///･Run MediaScanner for single or multiple paths.
    ///･If you save your own files without using the Android system, you need to run MediaScanner to recognize the file.
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///
    ///・単一または複数のパスについて、MediaScannerを実行します。
    ///・Androidシステムを使わずに、独自にファイルを保存したときなどには、MediaScannerを実行して、ファイルを認識させる必要があります。
    /// </summary>
    public class MediaScannerController : MonoBehaviour
    {

        //Callbacks
        [Serializable] public class CompleteHandler : UnityEvent<string> { }    //path
        public CompleteHandler OnComplete;


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Scan (update) a single file.
        public void StartScan(string path)
        {
#if UNITY_EDITOR
            Debug.Log("MediaScannerController.StartScan called");
#elif UNITY_ANDROID
            AndroidPlugin.StartMediaScanner(path, gameObject.name, "ReceiveComplete");
#endif
        }

        //Scan (update) multiple files.
        public void StartScan(string[] paths)
        {
#if UNITY_EDITOR
            Debug.Log("MediaScannerController.StartScan called");
#elif UNITY_ANDROID
            AndroidPlugin.StartMediaScanner(paths, gameObject.name, "ReceiveComplete");
#endif
        }


        //Callback handler when receive complete
        private void ReceiveComplete(string path)
        {
            if (OnComplete != null)
                OnComplete.Invoke(path);
        }
    }
}
