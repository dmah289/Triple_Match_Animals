using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Match_3/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        public int Value;
        public Sprite Sprite;
    }
}
