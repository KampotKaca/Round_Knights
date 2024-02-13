using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                if (instance == null)
                {
                    Debug.LogWarning("Can't Find Instance Of Type "
                                     + typeof(T).FullName + " In the scene");
                }
            }

            return instance;
        }
    }
}