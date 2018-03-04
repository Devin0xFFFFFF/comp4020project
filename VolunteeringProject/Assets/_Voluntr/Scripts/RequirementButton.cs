using UnityEngine;
using UnityEngine.UI;

public class RequirementButton : MonoBehaviour {
    public Button Button;
    public Image Image;

    public Sprite BackGroundCheckIcon;

    public void Set(Requirement requirement) {
        switch(requirement)
        {
            case Requirement.BackgroundCheck: Image.sprite = BackGroundCheckIcon;
                break;
        }
    }
}
