using UnityEngine;
using UnityEngine.UI;

public class LinkButton : MonoBehaviour {
    public Button Button;
    public Image Image;

    public Sprite DefaultIcon;
    public Sprite FacebookIcon;

    public void Set(Link link) {
        switch (link.Type)
        {
            case Link.LinkType.Default:
                Image.sprite = DefaultIcon;
                break;
            case Link.LinkType.Facebook:
                Image.sprite = FacebookIcon;
                break;
        }
    }
}
