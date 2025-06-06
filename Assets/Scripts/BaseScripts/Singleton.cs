// Unity公式のデザインパターンのサンプルを参考に作成
// https://github.com/Unity-Technologies/game-programming-patterns-demo

using UnityEngine;

/// <summary>
/// シングルトンパターンの基底クラス(ジェネリック)
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    SetupInstance();
                }
            }
            else
            {
                string typeName = typeof(T).Name;
                Debug.Log("[Singleton] " + typeName + " インスタンスが既に存在: " +
                                _instance.gameObject.name);
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private static void SetupInstance()
    {
        _instance = (T)FindObjectOfType(typeof(T));
        if(_instance == null)
        {
            GameObject singletonObject = new GameObject();
            singletonObject.name = typeof(T).Name;
            _instance = singletonObject.AddComponent<T>();
            DontDestroyOnLoad(singletonObject);
        }
    }

    private void RemoveDuplicates()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

