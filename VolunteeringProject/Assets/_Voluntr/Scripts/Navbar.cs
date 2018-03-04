using UnityEngine;

public class Navbar : MonoBehaviour {

    public void SwitchToHomeScreen() {
        AppManager.SwitchToHomeScreen();
    }

    public void SwitchToProfileScreen() {
        AppManager.SwitchToProfileScreen(AppManager.CurrentUser);
    }

    public void SwitchToListScreen() {
        AppManager.SwitchToListScreen();
    }

    public void SwitchToMapScreen() {
        AppManager.SwitchToMapScreen();
    }

    public void SwitchToCreateScreen() {
        AppManager.SwitchToCreateScreen();
    }
}
