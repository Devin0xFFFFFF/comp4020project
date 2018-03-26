using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {
    private class BackStackFrame {
        public int Screen;
        public object Param;
    }

    public static User CurrentUser;

    private static AppManager _instance;

    public Menu Menu;

    public HomeScreen HomeScreen;
    public EventScreen EventScreen;
    public ProfileScreen ProfileScreen;
    public ListScreen ListScreen;
    public MapScreen MapScreen;
    public CreateScreen CreateScreen;
    public ConsoleScreen ConsoleScreen;

    public static string CurrentScreen;

    private Stack<BackStackFrame> _backStack;

    private void Awake() {
        _instance = this;
        CurrentUser = AppData.Users[AppData.Users.Count - 1];
        _backStack = new Stack<BackStackFrame>();
        ConsoleScreen.Instance = ConsoleScreen;
        HideMenu();
        SwitchToHomeScreen();
        BuildMap();
    }

    public static void ShowMenu() {
        _instance.Menu.Show();
    }

    public static void HideMenu() {
        _instance.Menu.Hide();
    }

    public void ToggleConsole() {
        ConsoleScreen.gameObject.SetActive(!ConsoleScreen.gameObject.activeInHierarchy);
    }

    public void Back() {
        if(_instance._backStack.Count > 1)
        {
            _instance._backStack.Pop();
            BackStackFrame frame = _instance._backStack.Pop();
            string lastScreen = CurrentScreen;
            string param = "";
            switch(frame.Screen)
            {
                case 0: SwitchToHomeScreen();
                    break;
                case 1: SwitchToEventScreen((EventInfo)frame.Param);
                    param = ((EventInfo)frame.Param).Name;
                    break;
                case 2: SwitchToProfileScreen((User)frame.Param);
                    param = ((User)frame.Param).Name;
                    break;
                case 3: SwitchToListScreen();
                    break;
                case 4: SwitchToMapScreen();
                    break;
                case 5: SwitchToCreateScreen();
                    break;
            }

            ConsoleScreen.Log("BACK", string.Format("{0} -> {1} {2}", lastScreen, CurrentScreen, param));
        }
    }

    public static void SwitchToHomeScreen() {
        _instance.DisableAllScreens();
        _instance.HomeScreen.gameObject.SetActive(true);
        _instance._backStack.Push(new BackStackFrame() { Screen = 0, Param = null });
        string lastScreen = CurrentScreen;
        CurrentScreen = "HOME";
        ConsoleScreen.Log("SWITCH", string.Format("{0} -> {1}", lastScreen, CurrentScreen));
    }

    public static void SwitchToEventScreen(EventInfo eventInfo) {
        _instance.DisableAllScreens();
        _instance.EventScreen.gameObject.SetActive(true);
        _instance.EventScreen.Set(eventInfo);
        _instance._backStack.Push(new BackStackFrame() { Screen = 1, Param = eventInfo });
        string lastScreen = CurrentScreen;
        CurrentScreen = "EVENT";
        ConsoleScreen.Log("SWITCH", string.Format("{0} -> {1}", lastScreen, CurrentScreen));
    }

    public static void SwitchToProfileScreen(User user) {
        _instance.DisableAllScreens();
        _instance.ProfileScreen.gameObject.SetActive(true);
        _instance.ProfileScreen.Set(user);
        _instance._backStack.Push(new BackStackFrame() { Screen = 2, Param = user });
        string lastScreen = CurrentScreen;
        CurrentScreen = "PROFILE";
        ConsoleScreen.Log("SWITCH", string.Format("{0} -> {1}", lastScreen, CurrentScreen));
    }

    public static void SwitchToListScreen() {
        _instance.DisableAllScreens();
        _instance.ListScreen.gameObject.SetActive(true);
        _instance._backStack.Push(new BackStackFrame() { Screen = 3, Param = null });
        string lastScreen = CurrentScreen;
        CurrentScreen = "LIST";
        ConsoleScreen.Log("SWITCH", string.Format("{0} -> {1}", lastScreen, CurrentScreen));
    }

    public static void SwitchToMapScreen() {
        _instance.DisableAllScreens();
        _instance.MapScreen.gameObject.SetActive(true);
        _instance._backStack.Push(new BackStackFrame() { Screen = 4, Param = null });
        string lastScreen = CurrentScreen;
        CurrentScreen = "MAP";
        ConsoleScreen.Log("SWITCH", string.Format("{0} -> {1}", lastScreen, CurrentScreen));
    }

    public static void SwitchToCreateScreen() {
        _instance.DisableAllScreens();
        _instance.CreateScreen.gameObject.SetActive(true);
        _instance._backStack.Push(new BackStackFrame() { Screen = 5, Param = null });
        string lastScreen = CurrentScreen;
        CurrentScreen = "CREATE";
        ConsoleScreen.Log("SWITCH", string.Format("{0} -> {1}", lastScreen, CurrentScreen));
    }

    public static void BuildMap() {
        _instance.StartCoroutine(_instance.MapScreen._BuildMap());
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
