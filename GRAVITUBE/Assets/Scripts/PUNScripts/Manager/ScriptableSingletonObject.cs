using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableSingletonObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] list = Resources.FindObjectsOfTypeAll<T>();

                if (list.Length == 0)
                {
                    Debug.Log("No Scriptable Object reference found of type" + typeof(T).ToString());
                    return null;
                }
                if (list.Length > 1)
                {
                    Debug.Log("Multiple References Found of Type " + typeof(T).ToString());
                    return null;
                }
                _instance = list[0];
            }
            return _instance;
        }
        
    }
}
