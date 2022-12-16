using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get => GetInstance();
    }

    private static T GetInstance()
    {
        if (_instance != null)
            return _instance;

        var instances = FindObjectsOfType<T>();
        var instance = instances.Length > 0 ? instances[0] : new GameObject(typeof(T).Name).AddComponent<T>();
        if(instances.Length > 1)
        {
            for (int i = 1; i < instances.Length; i++)
                Destroy(instances[i].gameObject);
        }

        return _instance = instance;
    }
}