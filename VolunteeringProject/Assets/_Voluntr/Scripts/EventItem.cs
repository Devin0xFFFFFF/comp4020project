using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class EventItem : MonoBehaviour {
    public event Action<EventItem, bool> OnSelected;

    public Text NameText;
    public Text TimeText;
    public Text LocationText;
    public Text TagsText;
    public AccordionElement Element;

    [HideInInspector]
    public EventInfo EventInfo;
    public bool Selected { get { return Element.isOn; } set { Element.isOn = value; } }

    private void Awake() {
        Element.onValueChanged.AddListener(Select);
    }

    public void Set(EventInfo eventInfo) {
        EventInfo = eventInfo;

        NameText.text = eventInfo.Name;
        TimeText.text = eventInfo.Time.ToShortDateString();
        LocationText.text = eventInfo.LocationName;
        TagsText.text = eventInfo.Tags;
    }

    private void Select(bool state) {
        if(OnSelected != null)
        {
            OnSelected(this, state);
        }
    }
}
