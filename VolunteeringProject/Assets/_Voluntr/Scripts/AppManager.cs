using UnityEngine;

public class AppManager : MonoBehaviour {
    public static User CurrentUser;

    private static AppManager _instance;

    public Menu Menu;

    public HomeScreen HomeScreen;
    public EventScreen EventScreen;
    public ProfileScreen ProfileScreen;
    public ListScreen ListScreen;
    public MapScreen MapScreen;
    public CreateScreen CreateScreen;

    private void Awake() {
        _instance = this;
        CurrentUser = AppData.Users[AppData.Users.Count - 1];
        HideMenu();
        SwitchToHomeScreen();
        StartCoroutine(MapScreen._GetMap());
    }

    public static void ShowMenu() {
        _instance.Menu.Show();
    }

    public static void HideMenu() {
        _instance.Menu.Hide();
    }

    public static void SwitchToHomeScreen() {
        _instance.DisableAllScreens();
        _instance.HomeScreen.gameObject.SetActive(true);
    }

    public static void SwitchToEventScreen(EventInfo eventInfo) {
        _instance.DisableAllScreens();
        _instance.EventScreen.gameObject.SetActive(true);
        _instance.EventScreen.Set(eventInfo);
    }

    public static void SwitchToProfileScreen(User user) {
        _instance.DisableAllScreens();
        _instance.ProfileScreen.gameObject.SetActive(true);
        _instance.ProfileScreen.Set(user);
    }

    public static void SwitchToListScreen() {
        _instance.DisableAllScreens();
        _instance.ListScreen.gameObject.SetActive(true);
    }

    public static void SwitchToMapScreen() {
        _instance.DisableAllScreens();
        _instance.MapScreen.gameObject.SetActive(true);
    }

    public static void SwitchToCreateScreen() {
        _instance.DisableAllScreens();
        _instance.CreateScreen.gameObject.SetActive(true);
    }

    private void DisableAllScreens() {
        HomeScreen.gameObject.SetActive(false);
        EventScreen.gameObject.SetActive(false);
        ProfileScreen.gameObject.SetActive(false);
        ListScreen.gameObject.SetActive(false);
        MapScreen.gameObject.SetActive(false);
        CreateScreen.gameObject.SetActive(false);
    }
}
