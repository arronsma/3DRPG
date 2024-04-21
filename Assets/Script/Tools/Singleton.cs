using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :Singleton<T>, new()
{

    private static T _instance;

    public void Awake()
    {
        _instance = (T)this;
    }
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
}
