using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Localization of strings
    ///･Search in System language setting -> defaultLanguage -> English -> Japanese (nothing -> "")
    ///･Be sure to initialize the inspector registration list with the "Initialize()" method.
    /// Initialize even if you change data later (To convert to List -> Dictionary at runtime)
    /// </summary>
    [Serializable]
    public class LocalizeString
    {

        private const int DEF_FONTSIZE = 14;    //default font size


        /// <summary>
        /// Parameter for each language
        /// </summary>
        [Serializable]
        public class Data
        {
            public SystemLanguage language;
            public string text;
            public int fontSize;

            public Data(SystemLanguage language = SystemLanguage.English, string text = "", int fontSize = DEF_FONTSIZE)
            {
                this.language = language;
                this.text = text;
                this.fontSize = fontSize;
            }
        }



        //Default language setting (language not found in System Language)
        public SystemLanguage defaultLanguage = SystemLanguage.English;


        //Note: List will not be used at runtime -> convert to Dictionary (Initialize())
        [SerializeField]
        private List<Data> list = new List<Data>()
        {
            new Data(SystemLanguage.English, "English"),
            new Data(SystemLanguage.Japanese, "日本語"),
        };

        private Dictionary<SystemLanguage, Data> table = new Dictionary<SystemLanguage, Data>();  //from the List (Initialize())


        //indexer
        public Data this[SystemLanguage language] {
            get {
                if (table.ContainsKey(language))
                    return table[language];

                return null;
            }
        }


        //Constructors
        public LocalizeString()
        {
            Initialize();
        }

        public LocalizeString(List<Data> list)
        {
            this.list = list;
            Initialize();
        }



        //Create a Dictionary from the List.
        public void Initialize()
        {
            table.Clear();

            foreach (var item in list)
            {
                table[item.language] = item;
            }
        }

        //Add to dictionary
        public void Add(Data newData)
        {
            table[newData.language] = newData;
        }

        //Remove from dictionary
        public void Remove(Data delData)
        {
            if (table.ContainsKey(delData.language))
                table.Remove(delData.language);
        }

        //Remove from dictionary
        public void Remove(SystemLanguage language)
        {
            if (table.ContainsKey(language))
                table.Remove(language);
        }



        //Language property (determined from the language setting of the current system)
        //･Search in System language setting -> defaultLanguage -> English -> Japanese (nothing -> Unknown)
        public SystemLanguage Language {
            get {
                if (table.ContainsKey(Application.systemLanguage))
                    return Application.systemLanguage;

                if (table.ContainsKey(defaultLanguage))
                    return defaultLanguage;

                if (table.ContainsKey(SystemLanguage.English))
                    return SystemLanguage.English;

                if (table.ContainsKey(SystemLanguage.Japanese))
                    return SystemLanguage.Japanese;

                return SystemLanguage.Unknown;  //Normally it should not be.
            }
        }

        //Localized string property (Data.text)
        //･Search in System language setting -> defaultLanguage -> English -> Japanese (nothing -> "")
        public string Text {
            get {
                if (Language != SystemLanguage.Unknown)
                    return table[Language].text;

                return "";
            }
        }

        //Font size property (Data.fontSize)
        //･Search in System language setting -> defaultLanguage -> English -> Japanese (nothing -> DEF_FONTSIZE)
        public int FontSize {
            get {
                if (Language != SystemLanguage.Unknown)
                    return table[Language].fontSize;

                return DEF_FONTSIZE;
            }
        }
    }

}