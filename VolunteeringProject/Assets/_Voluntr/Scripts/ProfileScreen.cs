using UnityEngine;
using UnityEngine.UI;

public class ProfileScreen : MonoBehaviour {
    public Text NameText;
    public Image UserImage;
    public Transform LinksList;
    public Text AboutText;
    public EventList EventList;

    public LinkButton LinkButton;

    private User _user;

    public void Set(User user) {
        _user = user;

        NameText.text = user.Name;
        SetLinks();
        AboutText.text = user.About;
        SetEvents();
    }

    private void OnEnable() {
        if(_user.Name !=  null) // Cheap way to check if page has been set
        {
            SetLinks();
            SetEvents();
        }
    }

    private void SetLinks() {
        Clear(LinksList);

        foreach (Link link in _user.Links)
        {
            LinkButton button = Instantiate(LinkButton, LinksList);
            button.Set(link);
            button.Button.onClick.AddListener(delegate ()
            {
                //Android open browser to link.Path
            });
        }
    }

    private void SetEvents() {
        if(_user.PreviousExperienceIDs.Length == 0)
        {
            EventList.gameObject.SetActive(false);
            return;
        }
        else
        {
            EventList.gameObject.SetActive(true);
        }

        EventList.Clear();

        for (int i = 0; i < _user.PreviousExperienceIDs.Length; i++)
        {
            EventList.AddItem(AppData.Events[_user.PreviousExperienceIDs[i]]);
        }

        EventList.OnItemSelected += EventList_OnItemSelected;
    }

    private void EventList_OnItemSelected(EventItem item) {
        AppManager.SwitchToEventScreen(item.EventInfo);
    }

    private void Clear(Transform list) {
        for (int i = list.childCount - 1; i >= 0; i--)
        {
            Destroy(list.GetChild(i).gameObject);
        }
    }
}
