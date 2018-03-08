using UnityEngine;

public class HomeScreen : MonoBehaviour {
    public EventList Feed;

    private void OnEnable() {
        Feed.Clear();
        int userID = AppManager.CurrentUser.ID;

        for (int i = 0; i < AppData.Events.Count; i++)
        {
            EventInfo evtInfo = AppData.Events[i];
            if (evtInfo.VolunteerIDs.Contains(userID) || evtInfo.OrganizerIDs.Contains(userID))
            {
                Feed.AddItem(evtInfo);
            }
        }

        Feed.OnItemSelected += Feed_OnItemSelected;
    }

    private void Feed_OnItemSelected(EventItem item) {
        AppManager.SwitchToEventScreen(item.EventInfo);
    }
}
