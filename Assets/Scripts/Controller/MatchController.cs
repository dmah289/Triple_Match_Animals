using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Manager;
using Model;
using UnityEngine;

namespace Controller
{
    public class MatchController : Singleton<MatchController>
    {
        [Header("Matching Logic")]
        public static readonly Vector2Int[] offsets =
        {
            new Vector2Int(-1, 0),   // Left
            new Vector2Int(1, 0),    // Right
            new Vector2Int(0, -1),   // Top
            new Vector2Int(0, 1)     // Bottom
        };
        private Queue<Tile> _changedTiles = new();
        
        [Header("Animation Duration")]
        [SerializeField] private Vector3 originalScale = new Vector3(0.15f, 0.15f, 0.15f);
        [SerializeField] private float _popDuration = 0.5f;
        [SerializeField] private float _popDelay = 0.1f;
        [SerializeField] private float _genDuration = 0.25f;

        public static event Action<SoundType> OnTilesPoppedSFX;
        public static event Action<int> OnTilesPoppedScore;

        private void OnEnable()
        {
            SwapController.OnTileSwapped += TileSwappedCallback;
        }

        private void OnDisable()
        {
            SwapController.OnTileSwapped -= TileSwappedCallback;
        }

        /// <summary>
        /// Check if at least one tile will match with other neighbors at new position
        /// </summary>
        /// <param name="tiles">Selected Tiles</param>
        /// <returns>bool</returns>
        public bool CheckSwapAbility(Tile[] tiles)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (GetConnectedTiles(tiles[i], null).Count >= 3)
                    return true;
            }
            return false;
        }
        
        public bool TileSwappedCallback(Tile[] tiles)
        {
            if(CheckSwapAbility(tiles))
            {
                StartCoroutine(TileSwappedCoroutine(tiles));
                return true;
            }
            return false;
        }

        private IEnumerator TileSwappedCoroutine(params Tile[] tiles)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                _changedTiles.Enqueue(tiles[i]);

                while (_changedTiles.Count > 0)
                {
                    yield return HandleMatching(_changedTiles.Dequeue());
                }
            }
        }

        private IEnumerator HandleMatching(Tile currTile)
        {
            List<Tile> connectedTiles = GetConnectedTiles(currTile, null);
            
            if(connectedTiles.Count < 3) yield break;

            connectedTiles.ForEach(tile => _changedTiles.Enqueue(tile));

            yield return PopMatchedTiles(connectedTiles);
        }

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
        
        private IEnumerator PopMatchedTiles(List<Tile> connectedTiles)
        {
            for (int i = 0; i < connectedTiles.Count; i++)
            {
                connectedTiles[i].IconTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCirc);
                OnTilesPoppedSFX?.Invoke(SoundType.Collect);
            }
            OnTilesPoppedScore?.Invoke(connectedTiles[0].Item.Value * connectedTiles.Count);
            
            yield return Helper.NonAllocatingWait.GetWait(_popDuration + connectedTiles.Count * _popDelay);

            for (int i = 0; i < connectedTiles.Count; i++)
            {
                connectedTiles[i].GenerateNewItem();
            }

            yield return Helper.NonAllocatingWait.WaitForEndOfFrameNonAllocating;

            for (int i = 0; i < connectedTiles.Count; i++)
            {
                connectedTiles[i].IconTransform.DOScale(originalScale, _genDuration).SetEase(Ease.OutCubic);
            }
            
            yield return Helper.NonAllocatingWait.GetWait(_genDuration);
            
            yield return Helper.NonAllocatingWait.WaitForEndOfFrameNonAllocating;
        }
    }
}