using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// Use like Singleton MonoBehaviour
    /// </summary>
    /// <typeparam name="T">Type of subclass</typeparam>
    public abstract class SingletonBehaviour<T> : MonoBehaviour
                                    where T : UnityEngine.Component
    {

        public bool dontDestroyOnLoad = false;


        //Singleton instance
        protected static T instance;

        public static T Instance {
            get {
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                    if (instance.GetComponent<SingletonBehaviour<T>>().dontDestroyOnLoad)
                        DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        // Use this for initialization
        protected void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);   //Delete when duplicated startup
            }
            else
            {
                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(this.gameObject);
                instance = this as T;
            }
        }

    }
}
