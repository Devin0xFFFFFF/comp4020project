using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// DialogItem for inspector (Convert to DialogItem type at runtime and use it)
    /// </summary>
    public class DialogItemParameter : MonoBehaviour
    {
        public DialogItemType type = DialogItemType.Divisor;
        public string key = "";                     //Key to be associated with return value

        //Divisor
        public float lineHeight = 1;                //line width (dp)
        public Color lineColor = Color.black;       //When clear, it is not specified.

        //Text
        public string text = "";                    //text string
        public Color textColor = Color.black;       //When clear, it is not specified.
        public Color backgroundColor = Color.clear; //When clear, it is not specified.

        [Serializable] public enum TextAlign { None, Left, Center, Right }
        public TextAlign align = TextAlign.None;    //text alignment ("":not specified, "center", "right", "left")

        //Switch
        public bool defaultChecked = false;         //on/off

        //Slider
        public float value = 100;
        public float minValue = 0;
        public float maxValue = 100;
        public int digit = 0;

        //ToggleGroup
        [Serializable]
        public class ToggleItemData
        {
            public string text = "";
            public string value = "";
        }
        public ToggleItemData[] toggleItems;

        public int checkedIndex = 0;

        public string[] TogglesTexts {
            get { return toggleItems.Select(e => e.text).ToArray(); }
        }
        public string[] TogglesValues {
            get { return toggleItems.Select(e => e.value).ToArray(); }
        }
    }
}
