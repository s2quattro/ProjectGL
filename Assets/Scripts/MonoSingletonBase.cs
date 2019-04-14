using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static object _syncobj = new object();
    
    public static T Instance
	{
        get
        {
            lock (_syncobj)
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (objs.Length > 0)
                        _instance = objs[0];

                    if (objs.Length > 1)
                        Debug.LogError("There is more than one " + typeof(T).Name + "in the scene");

                    if (_instance == null)
                    {
                        string goName = typeof(T).ToString();
                        GameObject go = GameObject.Find(goName);
                        if (go == null)
                            go = new GameObject(goName);

                        _instance = go.AddComponent<T>();
                    }
                }

                return _instance;                
            }            
        }
    }
}