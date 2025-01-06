using System;
using UnityEngine;

namespace Framework
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else Destroy(this);
        }
    }
}