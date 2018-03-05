using UnityEngine;
using UnityEngine.UI;

public class EventScreen : MonoBehaviour {
    public Text NameText;
    public Button ActionButton;
    public Transform OrganizerList;
    public Text TimeText;
    public Text LocationText;
    public Text TagsText;
    public Text DescriptionText;
    public Transform LinksList;
    public Transform RequirementsList;

    public OrganizerButton OrganizerButton;
    public LinkButton LinkButton;
    public RequirementButton RequirementButton;

    private EventInfo _eventInfo;

    public void Set(EventInfo eventInfo) {
        _eventInfo = eventInfo;

        NameText.text = eventInfo.Name;
        SetActionButton();
        SetOrganizers();
        TimeText.text = eventInfo.Time.ToShortDateString();
        LocationText.text = eventInfo.Location;
        TagsText.text = eventInfo.Tags;
        DescriptionText.text = eventInfo.Description;
        SetLinks();
        SetRequirements();
    }

    private void SetActionButton() {
        //ActionButton state control and signup
    }

    private void SetOrganizers() {
        Clear(OrganizerList);

        foreach(int id in _eventInfo.OrganizerIDs)
        {
            User organizer = AppData.Users[id];
            OrganizerButton button = Instantiate(OrganizerButton, OrganizerList);
            button.Button.onClick.AddListener( delegate()
            {
                AppManager.SwitchToProfileScreen(organizer);
            });
        }
    }

    private void SetLinks() {
        Clear(LinksList);

        foreach (Link link in _eventInfo.Links)
        {
            LinkButton button = Instantiate(LinkButton, LinksList);
            button.Set(link);
            button.Button.onClick.AddListener(delegate ()
            {
                //Android open browser to link.Path
            });
        }
    }

    private void SetRequirements() {
        Clear(RequirementsList);

        foreach (Requirement requirement in _eventInfo.Requirements)
        {
            RequirementButton button = Instantiate(RequirementButton, RequirementsList);
            button.Set(requirement);
        }
    }

    private void Clear(Transform list) {
        for (int i = list.childCount - 1; i >= 0; i--)
        {
            Destroy(list.GetChild(i).gameObject);
        }
    }
}
