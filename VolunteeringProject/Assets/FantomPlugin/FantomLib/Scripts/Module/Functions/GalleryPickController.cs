using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Gallery Pick Controller
    /// 
    /// Open the default gallery application and get the image file information (path, width and height).
    ///･If there are two or more application, select with the launcher.
    ///･Note: Recommended since API 19(Android4.4). In the API Level before that, file information may not be get correctly according to the specification of the default folder or ContentProvider.
    ///･Note: Sometimes it can not be get correctly depending on the authority (security) or the folder in which it is placed.
    ///･Note: Depending on the saving condition of the image, it may not be possible to get width or height information (It becomes 0 when it can not be get).
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    ///
    /// デフォルトのギャラリーアプリを開いて、画像ファイル情報（パスと幅・高さ）を取得する。
    ///・アプリが複数ある場合、ランチャーで選択する。
    ///※API19(Android4.4)以降推奨。それより前のAPI Levelではデフォルトのフォルダやコンテンツプロバイダの仕様により、正しくファイル情報が取得できない可能性あり。
    ///※権限（セキュリティ）や配置しているフォルダなどによっても正しく取得できないことがある。
    ///※画像の保存状態によっては、幅や高さの情報が取得できないことがあるかもしれない（取得できなかったときは0になる）。
    /// </summary>
    public class GalleryPickController : MonoBehaviour
    {

        //Callbacks
        //Callback when get file information is successful.     //ファイル情報の取得が成功したときのコールバック
        [Serializable] public class ResultHandler : UnityEvent<string, int, int> { }    //path, width, height
        public ResultHandler OnResult;

        //Callback when get file information is fail.           //ファイル情報の取得が失敗したときのコールバック
        [Serializable] public class ErrorHandler : UnityEvent<string> { }       //error state messate
        public ErrorHandler OnError;            //Callback when fail to get path or other error



        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        

        //Call the default gallery application. If there are two or more, select with the launcher.
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("GalleryPickController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.OpenGallery(gameObject.name, "ReceiveResult", "ReceiveError");
#endif
        }


        //Callback handler when receive result
        private void ReceiveResult(string result)
        {
            if (result[0] == '{')   //When Json, success.  //Json のとき、取得成功
            {
                ImageInfo info = JsonUtility.FromJson<ImageInfo>(result);
                if (OnResult != null)
                    OnResult.Invoke(info.path, info.width, info.height);
            }
            else
                ReceiveError(result);
        }

        //Callback handler when receive error
        private void ReceiveError(string message)
        {
            if (OnError != null)
                OnError.Invoke(message);
        }
    }
}
