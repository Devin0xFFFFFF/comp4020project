using UnityEngine;
using UnityEngine.UI;

public class EventScreen : MonoBehaviour {
    public Text NameText;

    public void Set(EventInfo eventInfo) {
        NameText.text = eventInfo.Name;
    }
}
