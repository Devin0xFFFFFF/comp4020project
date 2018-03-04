using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventList : MonoBehaviour {
    public event Action<EventItem> OnItemSelected;

    public EventItem ItemPrefab;
    public Transform Accordion;
    public ScrollRect ScrollRect;

    private EventItem _selected;
    private EventItem _lastSelected;

    private float scrollPos;
    private bool selectionChanged;

    public void AddItem(EventInfo eventInfo) {
        EventItem item = Instantiate(ItemPrefab, Accordion);
        item.transform.SetAsFirstSibling();
        item.Set(eventInfo);

        ScrollRect.onValueChanged.AddListener(OnScroll);

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

    private void OnScroll(Vector2 scroll) {

        StopAllCoroutines();
        scrollPos = scroll.y;
        StartCoroutine(_MaintainScroll());
    }

    private IEnumerator _MaintainScroll() {
        yield return new WaitForSeconds(0.01f);
        ScrollRect.verticalNormalizedPosition = scrollPos;

        if(_selected != null && selectionChanged)
        {
            selectionChanged = false;
            for (int i = 0; i < 60; i++)
            {
                yield return null;
                ScrollRect.verticalNormalizedPosition = Mathf.Lerp(ScrollRect.verticalNormalizedPosition,
                    Mathf.Clamp01(_selected.transform.GetSiblingIndex() / Accordion.childCount), Time.deltaTime);
            }
        }
    }

    private void Item_OnSelected(EventItem item, bool state) {
        if (item == _lastSelected)
        {
            _lastSelected = null;
        }
        else if(_selected != item)
        {
            _lastSelected = _selected;
            _selected = item;
            selectionChanged = true;

            if (_lastSelected != null)
            {
                _lastSelected.Selected = false;
            }

            StartCoroutine(_MaintainScroll());
        }
        else if(_selected == item && OnItemSelected != null)
        {
            OnItemSelected(_selected);
        }
    }
}
