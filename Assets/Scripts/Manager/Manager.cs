using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    public static T instance;

    protected void Awake()
    {
        instance = (T)this;
    }
}
