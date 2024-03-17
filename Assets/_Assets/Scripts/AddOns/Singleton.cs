using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public interface ISingleton
    {
        bool IsSingletonLogsEnabled { get; set; }
    }

    public class Singleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton
    {
        static T s_Instance;

        [PropertyOrder(-31)]
        public bool DontDestroyLoad;

        [PropertyOrder(-30), ShowInInspector]
        public bool IsSingletonLogsEnabled { get => m_IsSingletonLogsEnabled; set => m_IsSingletonLogsEnabled = value; }

        [SerializeField, HideInInspector] bool m_IsSingletonLogsEnabled;

        [Title("Derived Fields Below:"), PropertyOrder(-18), ShowInInspector, HideLabel, DisplayAsString, VerticalGroup("1", PaddingBottom = -20)]
        string m_EmptyTitle = string.Empty;

        protected bool m_IsDestroyed;
        
        protected static bool s_ApplicationIsQuittingFlag;
        protected static bool s_ApplicationIsQuitting;

        public static bool IsInstanceNull => s_Instance == null;

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);

                    if (s_Instance == null)
                    {
                        if (s_ApplicationIsQuitting && Application.isPlaying)
                        {
                            Debug.LogWarning("[Singleton] IsQuitting Instance '" + typeof(T) + "' is null, returning.");
                            return s_Instance;
                        }
                        else
                        {
                            GameObject singleton = new GameObject();
                            s_Instance = singleton.AddComponent<T>();
                            singleton.name = "[Singleton] " + typeof(T);
                            Debug.Log("[Singleton] An instance of '" + typeof(T) + "' was created: " + singleton);
                        }
                    }
                    else
                    {
                        if (s_Instance.IsSingletonLogsEnabled)
                        {
                            Debug.Log("[Singleton] Found instance of '" + typeof(T) + "': " + s_Instance.gameObject.name);
                        }
                    }
                }
                return s_Instance;
            }
        }

        void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = gameObject.GetComponent<T>();
                if (DontDestroyLoad)
                {
                    setDontDestroyOnLoad();
                }
                OnAwakeEvent();
            }
            else
            {
                if (this == s_Instance)
                {
                    if (DontDestroyLoad)
                    {
                        setDontDestroyOnLoad();
                    }
                    OnAwakeEvent();
                }
                else
                {
                    m_IsDestroyed = true;
                    Destroy(this.gameObject);
                }
            }
        }

        protected virtual void OnAwakeEvent() 
        { 

        }

        // This is added to indicate for derived class that they should override
        // Since implementing a Start() function on any of the derived will hide base Start() functions - dangerous
        public virtual void Start() 
        {
        
        }

        public virtual void OnDisable()
        {
            s_ApplicationIsQuitting = s_ApplicationIsQuittingFlag;
        }

        public virtual void OnDestroy()
        {

        }

        protected void setDontDestroyOnLoad()
        {
            DontDestroyLoad = true;
            if (DontDestroyLoad)
            {
                if (!transform.parent)
                    transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed,
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnApplicationQuit()
        {
            s_ApplicationIsQuittingFlag = true;
        }
    }
}
