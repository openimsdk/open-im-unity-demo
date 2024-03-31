using UnityEngine;

public class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
{
    private static T Instance;

    public static T GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();
            if (Instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name);
                Instance = go.AddComponent<T>();
            }
        }
        return Instance;
    }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Debug.LogErrorFormat("试图实例化第二个单例类: {0}", GetType().Name);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}