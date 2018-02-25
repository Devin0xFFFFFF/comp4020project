using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FantomLib
{
    /// <summary>
    /// 'Param' class is basically the same as Dictionary prepared for easy handling of value type conversion and default value.
    ///･All keys and values are stored in string type.
    /// "GetInt()", "GetFloat()", "GetBool()" is not checking type, note to an error (parse failure).
    ///･"Parse()", "ParseToDictionary()" is method to convert text format like "key1=value1" to dictionary.
    /// </summary>
    public class Param : Dictionary<string, string>
    {

        //====================================================================
        //Constructors

        public Param() : base() { }

        public Param(Dictionary<string, string> dic) : base(dic) { }


        //====================================================================
        //Get/Set a value

        public string GetString(string key, string def = "")
        {
            return ContainsKey(key) ? this[key] : def;
        }

        public int GetInt(string key, int def = 0)
        {
            return ContainsKey(key) ? int.Parse(this[key]) : def;
        }

        public float GetFloat(string key, float def = 0)
        {
            return ContainsKey(key) ? float.Parse(this[key]) : def;
        }

        public bool GetBool(string key, bool def = false)
        {
            return ContainsKey(key) ? bool.Parse(this[key]) : def;
        }

        public void Set(string key, object value)
        {
            this[key] = value.ToString();
        }


        //====================================================================
        //etc.

        public override string ToString()
        {
            if (Count > 0)
                return this.Select(e => e.Key + " => " + e.Value).Aggregate((s, a) => s + ", " + a).ToString();
            return "";
        }


        //====================================================================
        //static methods

        /// <summary>
        /// Parsing text and generating a Dictionary
        ///･string: "key1=value1\nkey2=value2\nkey3=value3" -> Dictionary: dic[key1] = value1, dic[key2] = value2, dic[key3] = value3
        ///･Note that we do not check for invalid text
        ///･Note that duplicate keys result in an error.
        ///･The generated Dictionary has both key and value as string type.
        /// </summary>
        /// <param name="text">Text to parse</param>
        /// <param name="itemSeparator">Delimiter for each item</param>
        /// <param name="pairSeparator">Delimiter for Key and value</param>
        /// <returns>Dictionary created with key and value (failure or empty -> null)</returns>
        public static Dictionary<string, string> ParseToDictionary(string text, char itemSeparator = '\n', char pairSeparator = '=')
        {
            if (string.IsNullOrEmpty(text))
                return null;

            return text.Split(new char[] { itemSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Split(new char[] { pairSeparator }, 2))
                .ToDictionary(a => a[0], a => a[1]);    //(*) Note that duplicate keys result in an error.
        }


        /// <summary>
        /// Parsing text and generating a Param
        ///･string: "key1=value1\nkey2=value2\nkey3=value3" -> Dictionary: dic[key1] = value1, dic[key2] = value2, dic[key3] = value3
        ///･Note that we do not check for invalid text
        ///･Note that duplicate keys result in an error.
        ///･The generated Dictionary has both key and value as string type.
        /// </summary>
        /// <param name="text">Text to parse</param>
        /// <param name="itemSeparator">Delimiter for each item</param>
        /// <param name="pairSeparator">Delimiter for Key and value</param>
        /// <returns>Param created with key and value (failure or empty -> null)</returns>
        public static Param Parse(string text, char itemSeparator = '\n', char pairSeparator = '=')
        {
            Dictionary<string, string> dic = ParseToDictionary(text, itemSeparator, pairSeparator);
            return (dic != null) ? new Param(dic) : null;
        }


        /// <summary>
        /// Convert it to JSON format (string type) as a Dictionary and save it in PlayerPrefs
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="param">Save value (Param)</param>
        public static void SetPlayerPrefs(string key, Param param)
        {
            XPlayerPrefs.SetDictionary(key, param);
        }


        /// <summary>
        /// Generate and return elements saved in JSON format (string type) in PlayerPrefs as Param
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="def">Defalut value</param>
        /// <returns>Saved value (Param: newly created)</returns>
        public static Param GetPlayerPrefs(string key, Param def = null)
        {
            Dictionary<string, string> dic = XPlayerPrefs.GetDictionary<string, string>(key);
            return (dic != null) ? new Param(dic) : def;
        }

    }

}