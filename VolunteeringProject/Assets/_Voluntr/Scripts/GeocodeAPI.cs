using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GeocodeAPI {
    private class location {
        public double lat;
        public double lng;
    }

    public static IEnumerator _GetCoordinates(string address, Action<Vector2> onLocate) {
        StringBuilder urlBuilder = new StringBuilder();
        urlBuilder.AppendFormat("https://maps.googleapis.com/maps/api/geocode/json?address={0}", WWW.EscapeURL(address));
        urlBuilder.AppendFormat("&key={0}", AppData.GEOCODE_API_KEY);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlBuilder.ToString());

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JObject googleSearch = JObject.Parse(www.downloadHandler.text);

            // get JSON result objects into a list
            IList<JToken> results = googleSearch["results"].Children().ToList();

            location loc = results[0]["geometry"]["location"].ToObject<location>();

            onLocate(new Vector2((float)loc.lat, (float)loc.lng));
        }
    }
}
