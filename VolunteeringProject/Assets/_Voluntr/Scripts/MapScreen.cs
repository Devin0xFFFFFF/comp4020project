using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=3ExhMNLmQtc
public class MapScreen : MonoBehaviour {

    public const float WPG_LAT = 49.8951f;
    public const float WPG_LONG = -97.1384f;

    public RawImage MapImage;

    public IEnumerator _GetMap() {
        StringBuilder urlBuilder = new StringBuilder();
        urlBuilder.AppendFormat("https://maps.googleapis.com/maps/api/staticmap?center=");
        urlBuilder.AppendFormat("{0},{1}", WPG_LAT, WPG_LONG);
        urlBuilder.AppendFormat("&zoom={0}&size={1}x{2}&scale={3}", 13, 640, 640, 2);
        urlBuilder.AppendFormat("&maptype={0}", 0);

        foreach (EventInfo evtInfo in AppData.Events)
        {
            urlBuilder.AppendFormat("&markers=color:blue%7Clabel:{0}%7C{1}", WWW.EscapeURL(evtInfo.Name), WWW.EscapeURL(evtInfo.Location));
        }

        urlBuilder.AppendFormat("&key={0}", "AIzaSyDtOGHPkTIRv18aUot0G-GeQ0lgrpqvOMo");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlBuilder.ToString());

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            MapImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            MapImage.SetNativeSize();
            MapImage.transform.localScale.Set(1, 1, 1);
        }
    }
}