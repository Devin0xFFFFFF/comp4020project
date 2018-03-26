using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=3ExhMNLmQtc
public class MapScreen : MonoBehaviour {

    public const float WPG_LAT = 49.8951f;
    public const float WPG_LNG = -97.1384f;

    public Button EventButton;
    public RawImage MapImage;
    public Button MapMarkerButton;

    private Button _selectedButton;

    private void OnEnable() {
        EventButton.gameObject.SetActive(false);
    }

    public IEnumerator _BuildMap() {
        EventButton.onClick.AddListener(GoToSelection);
        EventButton.gameObject.SetActive(false);
        yield return _GetMap();
        LinkMarkers();
    }

    //Link up hard coded markers
    public void LinkMarkers() {
        for(int i = 0; i < MapImage.transform.childCount; i++)
        {
            Button markerButton = MapImage.transform.GetChild(i).GetComponent<Button>();
            markerButton.onClick.AddListener(delegate () 
            {
                EventButton.gameObject.SetActive(true);
                Text text = EventButton.GetComponentInChildren<Text>();
                text.text = AppData.Events[markerButton.transform.GetSiblingIndex()].Name;
                ConsoleScreen.Log("SELECT", AppData.Events[markerButton.transform.GetSiblingIndex()].Name);
                _selectedButton = markerButton;
            });
        }
    }

    private void GoToSelection() {
        if(_selectedButton != null)
        {
            ConsoleScreen.Log("GO TO", AppData.Events[_selectedButton.transform.GetSiblingIndex()].Name);
            AppManager.SwitchToEventScreen(AppData.Events[_selectedButton.transform.GetSiblingIndex()]);
        }
    }

    private IEnumerator _GetMap() {
        StringBuilder urlBuilder = new StringBuilder();
        urlBuilder.AppendFormat("https://maps.googleapis.com/maps/api/staticmap?center={0},{1}", WPG_LAT, WPG_LNG);
        urlBuilder.AppendFormat("&zoom={0}&size={1}x{2}&scale={3}&maptype={4}", 13, 640, 640, 2, 0);

        foreach (EventInfo evtInfo in AppData.Events)
        {
            urlBuilder.AppendFormat("&markers=color:blue%7Clabel:{0}%7C{1},{2}", WWW.EscapeURL(evtInfo.Name.Substring(0, 1).ToUpper()), evtInfo.Latitude, evtInfo.Longitude);
        }

        urlBuilder.AppendFormat("&key={0}", AppData.MAPS_API_KEY);

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
        }
    }
}