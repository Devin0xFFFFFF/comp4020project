using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace FantomLib
{
    /// <summary>
    /// Software Volume operation of the AudioMixer in the Slider Dialog
    /// http://fantom1x.blog130.fc2.com/blog-entry-281.html#VolumeSliderDialogController
    ///･Register the exporse parameter name of the AudioMixer and the AudioSource for preview playback as items in the inspector.
    ///･On the Silder Dialog, express the software volume with 0~100 -> convert to AudioMixer: -80~0db
    /// </summary>
    public class VolumeSliderDialogController : MonoBehaviour
    {

        public AudioMixer mixer;            //AudioMixer to control the volume

        public string title = "Sound Setting";
        public string message = "You can preview play by moving the slider.";
        public Color itemTextColor = Color.black;
        public string okButton = "OK";
        public string cancelButton = "Cancel";

        public string style = "android:Theme.DeviceDefault.Light.Dialog.Alert";

        public bool saveVolume = true;      //Save the software volume (PlayerPrefs)
        [SerializeField] private string saveKey = "";


        //Item parameters for Slider Dialog
        [Serializable]
        public class SliderItem
        {
            public string key;              //Returns key and Expose parameter name used in Slider Dialog
            public string text;             //Display text in dialog
            public AudioSource source;      //AudioSource to preview playback (Set in the inspector)
            [Range(0, 100)] public int volume = 50;  //Software volume: 0~100

            public SliderItem() { }

            public SliderItem(string key, string text, int volume)
            {
                this.key = key;
                this.text = text;
                this.volume = volume;
            }
        }

        //Slider Dialog Items
        [SerializeField]
        private SliderItem[] items = new SliderItem[] {
            new SliderItem("master", "Master", 100),
            new SliderItem("bgm", "Music", 50),
            new SliderItem("se", "Effect", 50),
            new SliderItem("voice", "Voice", 50),
        };


        //Generate arrays to be arguments of Slider Dialog
        private void GetItemArrays(out string[] keys, out string[] texts, out float[] volumes)
        {
            keys = new string[items.Length];
            texts = new string[items.Length];
            volumes = new float[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                keys[i] = items[i].key;
                texts[i] = items[i].text;
                volumes[i] = items[i].volume;
            }
        }


        //key -> SliderItem
        private Dictionary<string, SliderItem> dic = new Dictionary<string, SliderItem>();

        //For reset (Save the value of inspector at startup)
        private Dictionary<string, int> initVolumes = new Dictionary<string, int>();

        //PlayerPrefs Key (It is used only when saveKey is empty)
        const string VOLUME_PREF = "_volume";       //add name (PlayerPrefs)

        //Saved key in PlayerPrefs
        public string SaveKey {
            get { return string.IsNullOrEmpty(saveKey) ? gameObject.name + VOLUME_PREF : saveKey; }
        }


        // Use this for initialization
        private void Start()
        {
            //Load the software volume
            Dictionary<string, int> pref = null;
            if (saveVolume)
                pref = XPlayerPrefs.GetDictionary<string, int>(SaveKey); //nothing -> null

            foreach (var item in items)
            {
                dic[item.key] = item;
                initVolumes[item.key] = item.volume;  //Save the value of inspector at startup

                if (pref != null && pref.ContainsKey(item.key))
                    item.volume = pref[item.key];

                SetVolume(item.key, item.volume);
            }
        }


        private void OnDestroy()
        {
#if UNITY_EDITOR
            Debug.Log("AndroidPlugin.Release called");
#elif UNITY_ANDROID
            AndroidPlugin.Release();
#endif
        }


        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Set a software volume
        //volume: 0~100 -> -80~0db (AudioMixer)
        public void SetVolume(string key, float volume)
        {
            //Convert to db
            float val = Mathf.Clamp(volume / 100f, 0.0001f, 1.0f);
            float db = 20 * Mathf.Log10(val);
            mixer.SetFloat(key, Mathf.Clamp(db, -80.0f, 0.0f));

            if (dic.ContainsKey(key))
                dic[key].volume = (int)Mathf.Clamp(volume, 0, 100); //store to item
        }


        //Get software volumes
        public Dictionary<string, int> GetVolumes()
        {
            return items.ToDictionary(e => e.key, e => e.volume);
        }


        //Preview playback
        public void Play(string key)
        {
            if (dic.ContainsKey(key))
            {
                AudioSource src = dic[key].source;
                if (src != null && !src.isPlaying)
                    src.Play();
            }
        }


        //Stop preview
        public void Stop(string key)
        {
            if (dic.ContainsKey(key))
            {
                AudioSource src = dic[key].source;
                if (src != null && src.isPlaying)
                    src.Stop();
            }
        }


        //Call Adroid Slider Dialog
        public void OpenVolumeDialog()
        {
#if UNITY_EDITOR
            Debug.Log("AndroidPlugin.Release called");
#elif UNITY_ANDROID
            string[] keys; string[] texts; float[] volumes;
            GetItemArrays(out keys, out texts, out volumes);
            AndroidPlugin.ShowSliderDialog(
                title, message,
                texts, keys, volumes, null, null, null, itemTextColor,
                gameObject.name, "ReceiveVolume", "PreviewVolume",
                okButton, cancelButton, style);
#endif
        }


        //When "OK", the setting completion callback handler
        private void ReceiveVolume(string message)
        {
#if UNITY_EDITOR
            Debug.Log("ReceiveVolume : " + message);
#elif UNITY_ANDROID
            AndroidPlugin.ShowToast(message);
#endif
            if (!string.IsNullOrEmpty(message))
            {
                Dictionary<string, int> pref = new Dictionary<string, int>();   //For save
                string[] arr = message.Split('\n');
                for (int i = 0; i < arr.Length && i < items.Length; i++)
                {
                    string[] param = arr[i].Split('=');
                    items[i].volume = (param.Length > 1) ? int.Parse(param[1]) : int.Parse(param[0]);
                    pref[items[i].key] = items[i].volume;   //item key and software volume pair
                }

                if (saveVolume && pref.Count > 0)
                {
                    SetPrefs(pref);
                    PlayerPrefs.Save();
                }
            }
        }


        //Preview playback callback handler ('key' required)
        private void PreviewVolume(string message)
        {
#if UNITY_EDITOR
            Debug.Log("PreviewVolume : " + message);
#endif
            if (!string.IsNullOrEmpty(message))
            {
                string[] param = message.Split('=');  //"key=value" format only
                if (param.Length > 1)
                {
                    //Select AudioSource from the key
                    string key = param[0];
                    Play(key);

                    //Set a software volume
                    float vol = float.Parse(param[1]);
                    SetVolume(key, vol);
                }
            }
        }


        //Reset software volume -> Restore the value (defVolumes) and delete PlayerPrefs
        public void ResetVolumes()
        {
            foreach (var item in initVolumes)
                SetVolume(item.Key, item.Value);

            PlayerPrefs.DeleteKey(SaveKey);
        }


        //Set a software volume to PlayerPrefs (*) It does not affect the current software volume
        public void SetPrefs(Dictionary<string, int> pref)
        {
            if (pref != null && pref.Count > 0)
                XPlayerPrefs.SetDictionary(SaveKey, pref);
        }

    }

}