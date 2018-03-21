using UnityEngine;
using UnityEngine.UI;

public class ListScreen : MonoBehaviour {
    public InputField FilterInput;
    public EventList EventList;

    [Range(0, 1)]
    public double MatchThreshold = 0.5;

    private string[] filterTags;

	private char[] stringSeparators = { ' ', ',' };

    private void Awake() {
        FilterInput.onEndEdit.AddListener(ApplyFilter);
        EventList.OnItemSelected += EventList_OnItemSelected;
    }

    private void OnEnable() {
        FilterInput.text = "";
        filterTags = new string[0];
        BuildList();
    }

    private void ApplyFilter(string filterInput) {
        filterTags = filterInput.ToLower().Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
        BuildList();
    }

    private void EventList_OnItemSelected(EventItem item) {
        AppManager.SwitchToEventScreen(item.EventInfo);
    }

    private void BuildList() {
        EventList.Clear();

        for (int i = 0; i < AppData.Events.Count; i++)
        {
            EventInfo evtInfo = AppData.Events[i];
            string[] nameMatchCriteria = evtInfo.Name.ToLower().Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
            string[] locationMatchCriteria = evtInfo.Location.ToLower().Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
            string[] tagsMatchCriteria = evtInfo.Tags.ToLower().Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
			if (filterTags.Length == 0 || MatchesFilter(nameMatchCriteria) || MatchesFilter(locationMatchCriteria) || MatchesFilter(tagsMatchCriteria))
            {
                EventList.AddItem(evtInfo);
            }
        }
    }

    private bool MatchesFilter(string[] tags) {
        foreach (string filterTag in filterTags)
        {
            foreach (string tag in tags)
            {
                if(StringComparer.CalculateSimilarity(tag, filterTag) > MatchThreshold)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
