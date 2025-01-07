using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Model;
using UnityEngine;

namespace Controller
{
    public class MatchController : Singleton<MatchController>
    {
        public static readonly Vector2Int[] offsets =
        {
            new Vector2Int(-1, 0),   // Left
            new Vector2Int(1, 0),    // Right
            new Vector2Int(0, -1),   // Top
            new Vector2Int(0, 1)     // Bottom
        };

        private bool IsOutOfGrid(int newRow, int newCol)
        {
            return newRow < 0 || newRow >= Board.Instance.Height || newCol < 0 || newCol >= Board.Instance.Width;
        }
        
        public List<Tile> GetConnectedTiles(Tile tile, List<Tile> excludeTiles = null)
        {
            List<Tile> result = new List<Tile>{tile};

            if (excludeTiles == null) excludeTiles = new List<Tile> { tile };
            else excludeTiles.Add(tile);

            for (int i = 0; i < offsets.Length; i++)
            {
                int newRow = tile.row + offsets[i].x;
                int newCol = tile.col + offsets[i].y;

                if (IsOutOfGrid(newRow, newCol)) continue;
                
                Tile currTile = Board.Instance.Tiles[newRow, newCol];
                if(!excludeTiles.Contains(currTile) && tile.Item.Equals(currTile.Item))
                    result.AddRange(GetConnectedTiles(currTile, excludeTiles));
            }
            return result;
        }

        public bool IsPopable(Tile tile)
        {
            List<Tile> connectedTiles = GetConnectedTiles(tile, null);
            return connectedTiles.Count > 3;
        }

        public void PopTileMatched(Tile tile)
        {
            if (IsPopable(tile))
            {
                print("Popable");
            }
        }
    }
}