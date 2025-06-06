using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAccess<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected void CleanMemory()
    {
        Instance = null;
    }
}
