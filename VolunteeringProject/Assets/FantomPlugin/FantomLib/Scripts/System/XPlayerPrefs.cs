using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Extend the save type of PlayerPrefs
    ///･bool -> 1 or 0 (int)
    ///･Save object (class, struct), Array, List or Dictionary -> JSON format
    /// </summary>
    public static class XPlayerPrefs
    {
        /// <summary>
        /// Save bool
        ///･bool -> int (1 or 0)
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="flg">Save value</param>
        public static void SetBool(string key, bool flg)
        {
            PlayerPrefs.SetInt(key, flg ? 1 : 0);
        }


        /// <summary>
        /// Load bool
        ///･int (1 or 0) -> bool
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value</returns>
        public static bool GetBool(string key, bool def = false)
        {
            return PlayerPrefs.GetInt(key, def ? 1 : 0) != 0;
        }



        /// <summary>
        /// Save long
        ///･long -> string
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="value">Save value</param>
        public static void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }


        /// <summary>
        /// Load long
        ///･string -> long
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value</returns>
        public static long GetLong(string key, long def = 0)
        {
            string s = PlayerPrefs.GetString(key, "");
            return string.IsNullOrEmpty(s) ? def : long.Parse(s);
        }


        /// <summary>
        /// Save double
        ///･double -> string
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="value">Save value</param>
        public static void SetDouble(string key, double value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }


        /// <summary>
        /// Load double
        ///･string -> double
        /// </summary>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value</returns>
        public static double GetDouble(string key, double def = 0)
        {
            string s = PlayerPrefs.GetString(key, "");
            return string.IsNullOrEmpty(s) ? def : double.Parse(s);
        }



        /// <summary>
        /// Save object (to JSON)
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="obj">Save value</param>
        public static void SetObject<T>(string key, T obj)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(obj));
        }


        /// <summary>
        /// Load object (from JSON)
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value (newly create instance)</returns>
        public static T GetObject<T>(string key, T def = default(T))
        {
            string json = PlayerPrefs.GetString(key);
            return !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<T>(json) : def;
        }


        /// <summary>
        /// Load object to be overwritten (from JSON)
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="obj">Saved value (to be overwritten)</param>
        public static void GetObjectOverwrite<T>(string key, ref T obj)
        {
            string json = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(json))
                JsonUtility.FromJsonOverwrite(json, obj);
        }



        /// <summary>
        /// Save Array (to JSON)
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="arr">Save value</param>
        public static void SetArray<T>(string key, T[] arr)
        {
            SetObject(key, new ArrayWrap<T>(arr));
        }


        /// <summary>
        /// Load Array (from JSON)
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value (newly create instance)</returns>
        public static T[] GetArray<T>(string key, T[] def = null)
        {
            ArrayWrap<T> obj = GetObject<ArrayWrap<T>>(key);
            return obj != null ? obj.ToArray() : def;
        }



        /// <summary>
        /// Save List (to JSON)
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="list">Save value</param>
        public static void SetList<T>(string key, List<T> list)
        {
            SetObject(key, new ListWrap<T>(list));
        }


        /// <summary>
        /// Load List (from JSON)
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value (newly create instance)</returns>
        public static List<T> GetList<T>(string key, List<T> def = null)
        {
            ListWrap<T> obj = GetObject<ListWrap<T>>(key);
            return obj != null ? obj.ToList() : def;
        }



        /// <summary>
        /// Save Dictionary (to JSON)
        ///･Array of keys, Array of values pair -> JSON
        /// </summary>
        /// <typeparam name="K">Type of keys</typeparam>
        /// <typeparam name="V">Type of values</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="dic">Save value</param>
        public static void SetDictionary<K, V>(string key, Dictionary<K, V> dic)
        {
            SetObject(key, new DictionaryWrap<K, V>(dic));
        }


        /// <summary>
        /// Load Dictionary (from JSON)
        ///･JSON -> Array of keys, Array of values pair -> Dictionary
        /// </summary>
        /// <typeparam name="K">Type of keys</typeparam>
        /// <typeparam name="V">Type of values</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="def">Default value</param>
        /// <returns>Saved value (newly create instance)</returns>
        public static Dictionary<K, V> GetDictionary<K, V>(string key, Dictionary<K, V> def = null)
        {
            DictionaryWrap<K, V> obj = GetObject<DictionaryWrap<K, V>>(key);
            return obj != null ? obj.ToDictionary() : def;
        }


        /// <summary>
        /// Save Array of keys, Array of values pair (to JSON)
        /// </summary>
        /// <typeparam name="K">Type of keys</typeparam>
        /// <typeparam name="V">Type of values<</typeparam>
        /// <param name="key">Save key</param>
        /// <param name="keys">Array of keys</param>
        /// <param name="values">Array of values</param>
        public static void SetArrayPair<K, V>(string key, K[] keys, V[] values)
        {
            SetObject(key, new DictionaryWrap<K, V>(keys, values));
        }


        /// <summary>
        /// Load Array of keys, Array of values pair (from JSON)
        /// </summary>
        /// <typeparam name="K">Type of keys</typeparam>
        /// <typeparam name="V">Type of values</typeparam>
        /// <param name="key"></param>
        /// <param name="keys">Saved Array of keys</param>
        /// <param name="values">Saved Array of values</param>
        /// <returns>get it -> true</returns>
        public static bool TryGetArrayPair<K, V>(string key, out K[] keys, out V[] values)
        {
            DictionaryWrap<K, V> obj = GetObject<DictionaryWrap<K, V>>(key);
            if (obj == null)
            {
                keys = null;
                values = null;
                return false;
            }
            else
            {
                keys = obj.Keys;
                values = obj.Values;
                return true;
            }
        }




        //====================================================================
        // A wrapping class that allows JSON to handle type by making type a member of class
        //･It is basically for work, and it is assumed to abandon after use (conversion -> returns copy).
        //(*) It does not allow to null (empty element is acceptable)

        //Wrap Array
        [Serializable]
        private class ArrayWrap<T>
        {
            public T[] array;

            public ArrayWrap(T[] array)
            {
                this.array = array;
            }

            public T[] ToArray()
            {
                return (T[])array.Clone();  //Returns copy
            }
        }


        //Wrap List
        [Serializable]
        private class ListWrap<T>
        {
            public List<T> list;

            public ListWrap(List<T> list)
            {
                this.list = list;
            }

            public List<T> ToList()
            {
                return new List<T>(list);   //Returns copy
            }
        }


        //Wrap Dictionary or Array of keys, Array of values pair
        //(*) The keys and values pair Array must have the same length.
        [Serializable]
        private class DictionaryWrap<K, V>
        {
            [SerializeField] private K[] keys;      //Array of keys (It is converted to JSON)
            [SerializeField] private V[] values;    //Array of values (It is converted to JSON)

            public DictionaryWrap(Dictionary<K, V> dic)
            {
                keys = dic.Keys.ToArray();
                values = dic.Values.ToArray();
            }

            public DictionaryWrap(K[] keys, V[] values)     //(*) Pair Array must have the same length.
            {
                this.keys = keys;
                this.values = values;
            }

            public Dictionary<K, V> ToDictionary()
            {
                return keys.Select((k, i) => new { k, v = values[i] })
                    .ToDictionary(a => a.k, a => a.v);      //(*) An error occurs if there is a duplicate key.
            }

            public K[] Keys {
                get { return (K[])keys.Clone(); }   //Returns copy
            }

            public V[] Values {
                get { return (V[])values.Clone(); } //Returns copy
            }
        }

    }

}