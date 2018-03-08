using FantomLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateScreen : MonoBehaviour {
    public bool ConfirmCreate = true;

    public DatePickerController DatePickerController;
    public MultiChoiceDialogController LinkMultiChoiceDialogController;
    public MultiChoiceDialogController RequirementMultiChoiceDialogController;
    public YesNoDialogController CreateYesNoDialogController;
    public ToastController ToastController;

    public InputField NameInput;
    public Text DateText;
    public Button SelectDateButton;
    public InputField LocationInput;
    public InputField TagsInput;
    public InputField DescriptionInput;
    public Button AddLinksButton;
    public Transform LinksList;
    public Button AddRequirementsButton;
    public Transform RequirementsList;
    public Button CreateButton;

    public LinkButton LinkButton;
    public RequirementButton RequirementButton;

    private Link[] _links;
    private Requirement[] _requirements;

    void Start () {
        DatePickerController.OnResult.AddListener(OnSelectDate);
        SelectDateButton.onClick.AddListener(delegate ()
        {
            DatePickerController.Show();
        });

        AddLinksButton.onClick.AddListener(delegate ()
        {
            LinkMultiChoiceDialogController.Show();
        });

        AddRequirementsButton.onClick.AddListener(delegate ()
        {
            RequirementMultiChoiceDialogController.Show();
        });

        LinkMultiChoiceDialogController.OnResultIndex.AddListener(SetLinks);

        RequirementMultiChoiceDialogController.OnResultIndex.AddListener(SetRequirements);

        CreateButton.onClick.AddListener(delegate()
        {
            if(ConfirmCreate)
            {
                CreateYesNoDialogController.Show();
            }
            else
            {
                CreateOpportunity("");
            }
        });

        CreateYesNoDialogController.OnYes.AddListener(CreateOpportunity);

        _links = new Link[0];
        _requirements = new Requirement[0];
    }

    private void OnEnable() {
        NameInput.text = "";
        DateText.text = DateTime.Now.ToShortDateString();
        LocationInput.text = "";
        TagsInput.text = "";
        DescriptionInput.text = "";
        Clear(LinksList);
        Clear(RequirementsList);

        LinkMultiChoiceDialogController.SetItem(new string[] 
        {
            Link.LinkType.Default.ToString(),
            Link.LinkType.Facebook.ToString()
        }, false);

        RequirementMultiChoiceDialogController.SetItem(new string[]
        {
            Requirement.BackgroundCheck.ToString()
        }, false);
    }

    private void CreateOpportunity(string result) {
        StartCoroutine(_CreateOpportunity());
    }

    private IEnumerator _CreateOpportunity() {
        Vector2 loc = Vector2.zero;

        yield return StartCoroutine(GeocodeAPI._GetCoordinates(LocationInput.text, delegate (Vector2 coords)
        {
            loc = coords;
        }));

        EventInfo evtInfo = new EventInfo()
        {
            Name = NameInput.text,
            Time = DateTime.Parse(DateText.text),
            Location = LocationInput.text,
            Latitude = loc.x,
            Longitude = loc.y,
            Tags = TagsInput.text,
            Description = DescriptionInput.text,
            OrganizerIDs = new List<int>() { AppManager.CurrentUser.ID },
            Links = _links,
            Requirements = _requirements,
            VolunteerIDs = new List<int>()
        };

        AppData.Events.Add(evtInfo);

        AppManager.BuildMap();

        ToastController.Show("Created " + evtInfo.Name + ".");

        AppManager.SwitchToEventScreen(evtInfo);
    }

    private void OnSelectDate(string dateText) {
        DateText.text = dateText;
    }

    private void SetLinks(int[] linkTypes) {
        _links = new Link[linkTypes.Length];

        for(int i = 0; i < _links.Length; i++)
        {
            _links[i] = new Link() { Type = (Link.LinkType)linkTypes[i], Path = "" };
        }

        Clear(LinksList);

        foreach (Link link in _links)
        {
            LinkButton button = Instantiate(LinkButton, LinksList);
            button.Set(link);
        }
    }

    private void SetRequirements(int[] requirementTypes) {
        _requirements = new Requirement[requirementTypes.Length];

        for (int i = 0; i < _requirements.Length; i++)
        {
            _requirements[i] = (Requirement)requirementTypes[i];
        }

        Clear(RequirementsList);

        foreach (Requirement requirement in _requirements)
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
