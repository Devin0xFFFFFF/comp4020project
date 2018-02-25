using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FantomLib;

//Open Wifi setting, Request Bluetooth enable
public class AndroidSettingsTest : MonoBehaviour {

    //Messages
    public string notReachableMessage = "Internet not reachable";                                           //ネットワークに到達できない
    public string reachableViaCarrierDataNetworkMessage = "Internet reachable via Carrier Data Network";    //キャリアデータネットワーク経由で到達できる
    public string reachableViaLocalAreaNetworkMessage = "Internet reachable via Local Area Network";        //Wifiまたはケーブル経由で到達できる


    // Use this for initialization
    private void Start () {
        CheckReachableNetwork();
    }
    
    // Update is called once per frame
    //private void Update () {
        
    //}


    //Check for Internet connection.    //インターネット接続の確認
    public void CheckReachableNetwork()
    {
        switch (Application.internetReachability) {
        case NetworkReachability.NotReachable:
            XDebug.Log(notReachableMessage);
            break;
        case NetworkReachability.ReachableViaCarrierDataNetwork:
            XDebug.Log(reachableViaCarrierDataNetworkMessage);
            break;
        case NetworkReachability.ReachableViaLocalAreaNetwork:
            XDebug.Log(reachableViaLocalAreaNetworkMessage);
            break;
        }
    }

    //Wifi settings
    public void OpenWifiSettings()
    {
        XDebug.Log(DateTime.Now.ToString("HH:mm:ss") + " OpenWifiSettings called");
#if UNITY_EDITOR
        Debug.Log("OpenWifiSettings called");
#elif UNITY_ANDROID
        AndroidPlugin.OpenWifiSettings(gameObject.name, "ReceiveWifiResult");
        //AndroidPlugin.OpenWifiSettings();
#endif
    }

    //Callback handler when Wifi Settings closed
    public void ReceiveWifiResult(string result)
    {
        XDebug.Log(DateTime.Now.ToString("HH:mm:ss") + " ReceiveWifiResult : result = " + result);
        CheckReachableNetwork();
    }

    //Callback handler when an error occurs in WifiS ettings.
    public void ReceiveWifiError(string message)
    {
        XDebug.Log("ReceiveWifiError : " + message);
    }



    //Bluetooth request enable
    public void StartBluetoothRequestEnable()
    {
#if UNITY_EDITOR
        Debug.Log("StartBluetoothRequestEnable called");
#elif UNITY_ANDROID
        AndroidPlugin.StartBluetoothRequestEnable(gameObject.name, "ReceiveBluetoothResult");
        //AndroidPlugin.StartBluetoothRequestEnable();
#endif
    }

    //Callback handler when Bluetooth request enable finished.
    private void ReceiveBluetoothResult(string result)
    {
        XDebug.Log("ReceiveBluetoothResult : result = " + result);
    }

    //Callback handler when Bluetooth request enable finished (yes/no).
    public void ReceiveBluetoothResult(bool isOn)
    {
        XDebug.Log("ReceiveBluetoothResult : isOn = " + isOn);
    }

    //Callback handler when an error occurs in Bluetooth request enable.
    public void ReceiveBluetoothError(string message)
    {
        XDebug.Log("ReceiveBluetoothError : " + message);
    }

}
