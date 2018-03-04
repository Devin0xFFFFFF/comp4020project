using UnityEngine;

public class HomeScreen : MonoBehaviour {
    public EventList Feed;

    private void OnEnable() {
        Feed.Clear();

        for (int i = 0; i < AppData.Events.Length; i++)
        {
            Feed.AddItem(AppData.Events[i]);
        }

        Feed.OnItemSelected += Feed_OnItemSelected;
    }

    private void Feed_OnItemSelected(EventItem item) {
        AppManager.SwitchToEventScreen(item.EventInfo);
    }
}
