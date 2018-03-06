using DG.Tweening;
using UnityEngine;

public class Menu : MonoBehaviour {
    public float OffscreenOffset;
    public float TransitionDuration;

    private RectTransform _rectTransform;

    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Show() {
        _rectTransform.DOAnchorPos(new Vector2(), TransitionDuration);
    }

    public void Hide() {
        _rectTransform.DOAnchorPos(new Vector2(OffscreenOffset, 0), TransitionDuration);
    }

    public void SwitchToHomeScreen() {
        AppManager.SwitchToHomeScreen();
        Hide();
    }

    public void SwitchToProfileScreen() {
        AppManager.SwitchToProfileScreen(AppManager.CurrentUser);
        Hide();
    }

    public void SwitchToListScreen() {
        AppManager.SwitchToListScreen();
        Hide();
    }

    public void SwitchToMapScreen() {
        AppManager.SwitchToMapScreen();
        Hide();
    }

    public void SwitchToCreateScreen() {
        AppManager.SwitchToCreateScreen();
        Hide();
    }
}
