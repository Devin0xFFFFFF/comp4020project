using FantomLib;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapAPI : MonoBehaviour {

    public const float WPG_LAT = 49.8951f;
    public const float WPG_LONG = -97.1384f;

    public RawImage MapImage;
    public PinchInput PinchInput;

    public string ApiKey;

    public int MinZoom = 8;
    public int MaxZoom = 14;

    public int Zoom = 14;
    public int MapWidth = 640;
    public int MapHeight = 640;

    private StringBuilder _urlBuilder;

    private void Start() {
        PinchInput.OnPinch.AddListener(ChangeZoom);
    }

    private void OnEnable() {
        Zoom = MaxZoom;
        StartCoroutine(_GetMap());
    }

    private void ChangeZoom(float width, float delta, float ratio) {
        Zoom = Mathf.RoundToInt(Mathf.Clamp(Zoom + delta, MinZoom, MaxZoom));
        StartCoroutine(_GetMap());
    }

    private IEnumerator _GetMap() {
        _urlBuilder = new StringBuilder();
        _urlBuilder.AppendFormat("https://maps.googleapis.com/maps/api/staticmap?center=");
        _urlBuilder.AppendFormat("{0},{1}", WPG_LAT, WPG_LONG);
        _urlBuilder.AppendFormat("&zoom={0}&size={1}x{2}&scale={3}", Zoom, MapWidth, MapHeight, 2);
        _urlBuilder.AppendFormat("&maptype={0}", 0);

        foreach (EventInfo evtInfo in AppData.Events)
        {
            _urlBuilder.AppendFormat("&markers=color:blue%7Clabel:{0}%7C{1}", WWW.EscapeURL(evtInfo.Name), WWW.EscapeURL(evtInfo.Location));
        }

        _urlBuilder.AppendFormat("&key={0}", ApiKey);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_urlBuilder.ToString());

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            MapImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //MapImage.SetNativeSize();
        }
    }
}