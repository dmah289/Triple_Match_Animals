using System;
using Framework;
using UnityEngine;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public Camera _cam;
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        #endif
        protected override void Awake()
        {
            
        }
        
    }
}
