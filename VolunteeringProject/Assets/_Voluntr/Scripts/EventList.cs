using System;
using UnityEngine;

public class EventList : MonoBehaviour {
    public event Action<EventItem> OnItemSelected;

    public EventItem ItemPrefab;
    public Transform Accordion;
    public int MaxItems = 10;

    private EventItem _selected;
    private EventItem _lastSelected;

    public void AddItem(EventInfo eventInfo) {
        EventItem item = Instantiate(ItemPrefab, Accordion);
        item.transform.SetAsFirstSibling();
        item.Set(eventInfo);

        item.OnSelected += Item_OnSelected;
    }

    public void Clear() {
        for(int i = Accordion.childCount - 1; i >= 0; i--)
        {
            EventItem item = Accordion.GetChild(i).GetComponent<EventItem>();
            if(item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void Item_OnSelected(EventItem item, bool state) {

        if(item == _lastSelected)
        {
            _lastSelected = null;
        }
        else if(_selected != item)
        {
            _lastSelected = _selected;
            _selected = item;

            if (_lastSelected != null)
            {
                _lastSelected.Selected = false;
            }
        }
        else if(_selected == item && OnItemSelected != null)
        {
            OnItemSelected(_selected);
        }
    }
}
