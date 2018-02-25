using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FantomLib
{
    /// <summary>
    /// Debugging log for runtime system (singleton)
    /// </summary>
    public class XDebug : SingletonBehaviour<XDebug>
    {

        public int lines = 0;               //Limit number of lines (Queue) for text buffer (0: no limit)
        private static Queue lineQueue;     //Text buffer (each line)
        private static StringBuilder sb;    //For join


        //Log output UI-Text
        public Text outputText;

        //Log output UI-Scrollbar
        public Scrollbar scrollbar;


        // Use this for initialization
        private new void Awake()
        {
            base.Awake();

            if (lines > 0 && lineQueue == null)
            {
                lineQueue = new Queue(lines);
                sb = new StringBuilder(128 * lines);
            }
        }

        private void OnDestroy()
        {
            Clear(0);
            StopAllCoroutines();
        }

        private void Start()
        {

        }


        // Update is called once per frame
        //private void Update()
        //{
        //}


        //Make the scroll bar last (always display the last text)
        //(*) Note that it will not be affected unless it is called after 1 frame.
        // (If long length of strings delay it for a few frames will go well)
        public void ScrollLast()
        {
            if (scrollbar != null)
                scrollbar.value = 0;
        }

        //Wait n frames for action
        private IEnumerator WaitFrames(Action action, int count)
        {
            count = Math.Max(0, count);
            while (--count >= 0)
                yield return null;

            action();
        }

        //Wait n seconds for action
        private IEnumerator WaitSeconds(Action action, float sec)
        {
            yield return new WaitForSeconds(sec);
            action();
        }


        //Display log
        const int DEF_WAIT_FRAMES = 3;  //Automatic scrolling goes well if it is a few frames.

        //Display text (Join each line when limit number of lines)
        private static void OutputText(object mes, bool newline = true)
        {
            if (Instance.outputText)
            {
                if (Instance.lines > 0 && lineQueue != null)
                {
                    lineQueue.Enqueue(mes + (newline ? "\n" : ""));
                    while (lineQueue.Count > Instance.lines)
                        lineQueue.Dequeue();

                    sb.Length = 0;
                    foreach (var item in lineQueue)
                        sb.Append(item);

                    Instance.outputText.text = sb.ToString();
                }
                else
                {
                    Instance.outputText.text += mes + (newline ? "\n" : "");
                }
            }
        }

        //Wait n frames and display log
        private static void OutputTextDelayedFrames(object mes, int delayedFrames = DEF_WAIT_FRAMES, bool newline = true)
        {
            if (Instance.outputText)
            {
                OutputText(mes, newline);
                Instance.StartCoroutine(Instance.WaitFrames(() => Instance.ScrollLast(), delayedFrames));
            }
        }

        //Wait n seconds and display log
        private static void OutputTextDelayedSeconds(object mes, float delayedSeconds, bool newline = true)
        {
            if (Instance.outputText)
            {
                OutputText(mes, newline);
                Instance.StartCoroutine(Instance.WaitSeconds(() => Instance.ScrollLast(), delayedSeconds));
            }
        }

        //Log
        public static void Log(object mes, bool newline)
        {
            Log(mes, DEF_WAIT_FRAMES, newline);
        }

        public static void Log(object mes, int delayedFrames = DEF_WAIT_FRAMES, bool newline = true)
        {
            Debug.Log(mes);
            OutputTextDelayedFrames(mes, delayedFrames, newline);
        }

        public static void Log(object mes, float delayedSeconds, bool newline = true)
        {
            Debug.Log(mes);
            OutputTextDelayedSeconds(mes, delayedSeconds, newline);
        }

        //LogWarging
        public static void LogWarning(object mes, bool newline)
        {
            LogWarning(mes, DEF_WAIT_FRAMES, newline);
        }

        public static void LogWarning(object mes, int delayedFrames = DEF_WAIT_FRAMES, bool newline = true)
        {
            Debug.LogWarning(mes);
            OutputTextDelayedFrames("Warning: " + mes, delayedFrames, newline);
        }

        public static void LogWarning(object mes, float delayedSeconds, bool newline = true)
        {
            Debug.LogWarning(mes);
            OutputTextDelayedSeconds("Warning: " + mes, delayedSeconds, newline);
        }

        //LogError
        public static void LogError(object mes, bool newline)
        {
            LogError(mes, DEF_WAIT_FRAMES, newline);
        }

        public static void LogError(object mes, int delayedFrames = DEF_WAIT_FRAMES, bool newline = true)
        {
            Debug.LogError(mes);
            OutputTextDelayedFrames("Error: " + mes, delayedFrames, newline);
        }

        public static void LogError(object mes, float delayedSeconds, bool newline = true)
        {
            Debug.LogError(mes);
            OutputTextDelayedSeconds("Error: " + mes, delayedSeconds, newline);
        }

        //Clear
        public static void Clear()
        {
            Clear(DEF_WAIT_FRAMES);
        }

        public static void Clear(int delayedFrames)
        {
            if (Instance.outputText)
            {
                if (lineQueue != null)
                {
                    lineQueue.Clear();
                    sb.Length = 0;
                }

                Instance.outputText.text = "";
                if (delayedFrames > 0)
                    Instance.StartCoroutine(Instance.WaitFrames(() => Instance.ScrollLast(), delayedFrames));
                else
                    Instance.ScrollLast();
            }
        }

        public static void Clear(float delayedSeconds)
        {
            if (Instance.outputText)
            {
                if (lineQueue != null)
                {
                    lineQueue.Clear();
                    sb.Length = 0;
                }

                Instance.outputText.text = "";
                if (delayedSeconds > 0)
                    Instance.StartCoroutine(Instance.WaitSeconds(() => Instance.ScrollLast(), delayedSeconds));
                else
                    Instance.ScrollLast();
            }
        }
    }
}