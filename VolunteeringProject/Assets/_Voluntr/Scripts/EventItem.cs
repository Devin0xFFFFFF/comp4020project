using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class EventItem : MonoBehaviour{
    public Text NameText;
    public Text TimeText;
    public Text LocationText;
    public Text TagsText;

    public void Set(string name, string time, string location, string tags) {
        NameText.text = name;
        TimeText.text = time;
        LocationText.text = location;
        TagsText.text = tags;
    }

    //public override OnValueChanged(bool state) {

    //}
}
