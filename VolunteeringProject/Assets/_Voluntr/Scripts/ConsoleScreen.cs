using UnityEngine;
using UnityEngine.UI;

public class ConsoleScreen : MonoBehaviour {
    public Transform LogList;
    public Text LogText;

    public static ConsoleScreen Instance;

    public static void Log(string element, string message) {
        string log = AppLogger.Log(AppManager.CurrentScreen, element, message);

        Text logText = Instantiate(Instance.LogText, Instance.LogList, false);
        logText.text = log;
    }

    public void Export() {
        AppLogger.Export();
    }
}
