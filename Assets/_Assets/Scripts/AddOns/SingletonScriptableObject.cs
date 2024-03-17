using UnityEngine;

namespace RoundKnights
{
    /// <summary>
    /// Abstract class for making reload-proof singletons out of ScriptableObjects
    /// Returns the asset created on the editor, or null if there is none
    /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T s_Instance;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var objects = Resources.LoadAll<T>(string.Empty);

                    if (objects.Length > 0 && objects[0] != null)
                    {
                        s_Instance = objects[0];
                    }

                    if (objects.Length > 1)
                        Debug.LogError("[SingletonScriptableObject] You have more than 1 object of type - " + typeof(T).FullName + ". Choosing first.");
                    else if (objects.Length == 0)
                        Debug.LogError("[SingletonScriptableObject] Something is wrong. Can't find the object - " + typeof(T).FullName + ". Make sure it's in the Resources folder.");
                }
                return s_Instance;
            }
        }
    }
}