using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Model
{
    [RequireComponent(typeof(SpriteRenderer), typeof(RectTransform), typeof(BoxCollider2D))]
    public class Tile : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private SpriteRenderer _tileSpriteRenderer;
        [SerializeField] public SpriteRenderer IconSpriteRenderer;
        [SerializeField] public Transform IconTransform;

        [Header("Attributes")] 
        public int row;
        public int col;
        [SerializeField] private Item _item;

        public Item Item
        {
            get => _item;
            set
            {
                if (_item == value) return;
                _item = value;
                IconSpriteRenderer.sprite = _item.Sprite;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                _tileSpriteRenderer = GetComponent<SpriteRenderer>();
                IconTransform = transform.GetChild(0);
                IconSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

                row = transform.parent.GetSiblingIndex();
                col = transform.GetSiblingIndex();
                gameObject.name = "Tile[" + row + "," + col + "]";
            }
        }
        public override string ToString()
        {
            return $"Tile[{row},{col}]";
        }
#endif
        private void Awake()
        {
            _tileSpriteRenderer = GetComponent<SpriteRenderer>();
            IconTransform = transform.GetChild(0);
            IconSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void OnMouseDown()
        {
            SelectionController.Instance.SelectTile(this);
        }

        public void UpdateNewIcon(Item newItem)
        {
            IconTransform = transform.GetChild(0);
            IconSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            Item = newItem;
        }

        public void GenerateNewItem()
        {
            Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
        }
    }
}