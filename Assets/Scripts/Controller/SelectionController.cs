using System;
using Framework;
using Model;
using UnityEngine;

namespace Controller
{
    public class SelectionController : Singleton<SelectionController>
    {
        [SerializeField] public Tile[] _selectedTiles = new Tile[2];

        public int SelectedCount
        {
            get
            {
                if (_selectedTiles[0] == null && _selectedTiles[1] == null)
                    return 0;
                else if (_selectedTiles[0] != null && _selectedTiles[1] != null)
                    return 2;
                else return 1;
            }
        }

        private bool IsContained(Tile tile)
        {
            return (_selectedTiles[0] != null && _selectedTiles[0].Equals(tile)) || (_selectedTiles[1] != null && _selectedTiles[1].Equals(tile));
        }

        private void SetSelectedTile(Tile tile)
        {
            if (_selectedTiles[0] == null)
            {
                _selectedTiles[0] = tile;
                return;
            }
            _selectedTiles[1] = tile;
        }

        private void ResetSelectedTiles()
        {
            _selectedTiles[0] = null;
            _selectedTiles[1] = null;
        }

        public async void SelectTile(Tile tile)
        {
            if (!IsContained(tile))
                SetSelectedTile(tile);
            
            print(SelectedCount);

            if (SelectedCount < 2) return;

            print($"Selected tiles: ({_selectedTiles[0].row},{_selectedTiles[0].col}) - ({_selectedTiles[1].row},{_selectedTiles[1].col})");
            await SwapController.Instance.Swap(_selectedTiles[0], _selectedTiles[1]);
            
            ResetSelectedTiles();
        }
        
        private void Start()
        {
            print(SelectedCount);
        }
    }
}