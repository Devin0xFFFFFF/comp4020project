using UnityEngine;
using UnityEngine.UI;

public class ListScreen : MonoBehaviour {
    public InputField FilterInput;
    public EventList EventList;

    [Range(0, 1)]
    public double MatchThreshold = 0.5;

    private string[] filterTags;

    private void Awake() {
        FilterInput.onEndEdit.AddListener(ApplyFilter);
    }

    private void Start() {
        filterTags = new string[0];
        BuildList();
    }

    private void ApplyFilter(string filterInput) {
        filterTags = filterInput.ToLower().Split(' ', ',');
        BuildList();
    }

    private void BuildList() {
        EventList.Clear();

        for (int i = 0; i < AppData.Events.Count; i++)
        {
            EventInfo evtInfo = AppData.Events[i];
            if(filterTags.Length == 0 || MatchesFilter(evtInfo.Tags.ToLower().Split(' ', ',')))
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
