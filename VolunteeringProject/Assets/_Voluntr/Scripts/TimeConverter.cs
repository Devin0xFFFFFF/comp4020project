using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeConverter : MonoBehaviour {
    public InputField Start;
    public InputField End;
    public InputField Result;

    private long _startTicks;
    private long _endTicks;

    public void OnStartValueChanged(string val) {
        if(long.TryParse(val, out _startTicks))
        {
            CalculateResult();
        }
    }

    public void OnEndValueChanged(string val) {
        if (long.TryParse(val, out _endTicks))
        {
            CalculateResult();
        }
    }

    private void CalculateResult() {
        DateTime start = new DateTime(_startTicks);
        DateTime end = new DateTime(_endTicks);

        TimeSpan diff = end.Subtract(start);

        Result.text = diff.Milliseconds.ToString();
    }
}
