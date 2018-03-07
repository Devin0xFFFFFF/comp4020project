using FantomLib;
using UnityEngine;
using UnityEngine.UI;

public class EventScreen : MonoBehaviour {
    public YesNoWithCheckBoxDialogController RegisterYesNoWithCheckBoxDialogController;
    public AndroidActionController LinkController;
    public ToastController ToastController;

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

    public Sprite UnregisteredSprite;
    public Sprite RegisteredSprite;

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

        ActionButton.onClick.AddListener(ShowRegister);

        RegisterYesNoWithCheckBoxDialogController.OnYes.AddListener(Register);
    }

    private void ShowRegister() {
        bool registered = _eventInfo.VolunteerIDs.Contains(AppManager.CurrentUser.ID);
        RegisterYesNoWithCheckBoxDialogController.DefaultChecked = registered;
        RegisterYesNoWithCheckBoxDialogController.message = (registered ? "Unregister" : "Register") + " for Opportunity?";
        RegisterYesNoWithCheckBoxDialogController.checkBoxText = "Check to " + (registered ? "Unregister" : "Register");
        RegisterYesNoWithCheckBoxDialogController.Show();
    }

    private void Register(string s, bool yes) {
        if (yes && !_eventInfo.VolunteerIDs.Contains(AppManager.CurrentUser.ID))
        {
            _eventInfo.VolunteerIDs.Add(AppManager.CurrentUser.ID);
            ToastController.Show("Registered.");
        }
        else if(_eventInfo.VolunteerIDs.Contains(AppManager.CurrentUser.ID))
        {
            _eventInfo.VolunteerIDs.Remove(AppManager.CurrentUser.ID);
            ToastController.Show("Unregistered.");
        }

        SetActionButton();
    }

    private void SetActionButton() {
        if(_eventInfo.VolunteerIDs.Contains(AppManager.CurrentUser.ID))
        {
            ActionButton.transform.GetChild(0).GetComponent<Image>().sprite = RegisteredSprite;
        }
        else
        {
            ActionButton.transform.GetChild(0).GetComponent<Image>().sprite = UnregisteredSprite;
        }
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
                LinkController.StartActionURI(link.Path);
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
