using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventList : MonoBehaviour {
    public EventItem ItemPrefab;
    public Transform Accordion;
    public int MaxItems = 10;

    private void Start() {
        for(int i = 0; i < MaxItems; i++)
        {
            AddItem("Item" + i, DateTime.Now.ToShortDateString(), "UofM", "tag");
        }
    }

    public void AddItem(string name, string time, string location, string tags) {
        EventItem item = Instantiate(ItemPrefab, Accordion);
        item.transform.SetAsFirstSibling();
        item.Set(name, time, location, tags);
    }
}
