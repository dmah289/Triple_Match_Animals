using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Row : MonoBehaviour
    {
        public Tile[] tiles;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            tiles = GetComponentsInChildren<Tile>();
        }
#endif
    }
}
