using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model
{
    public class Board : Singleton<Board>
    {
        [Header("Components")]
        [SerializeField] private Row[] _rows;
        [SerializeField] private Tile[,] _tiles;
        
        [Header("Attributes")]
        [SerializeField] private int Width => _tiles.GetLength(1);
        [SerializeField] private int Height => _tiles.GetLength(0);
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _rows = GetComponentsInChildren<Row>();
        }
#endif
        
        private void Start()
        {
            _tiles = new Tile[_rows.Length, _rows.Max(row => row.tiles.Length)];

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Tile tile = _rows[row].tiles[col];
                    tile.row = row;
                    tile.col = col;
                    tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                    
                    _tiles[row, col] = tile;
                }
            }
        }
    }
}
