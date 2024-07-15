using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameCore
{
    public class ShipBattleGame : MonoBehaviour
    {
        [SerializeField] private List<Button> _playerShips;
        [SerializeField] private List<Button> _enemyShips;
        
        [SerializeField] private List<Button> _playerField;
        [SerializeField] private List<Button> _enemyField;
        [SerializeField] private Button _playButton;
        
        private Action OnShipsPlayerSetted;
        
        private void Start()
        {
            GameInstance.UINavigation.OnGameStarted += SelectEnemyShips;
            OnShipsPlayerSetted += SetButtonInteractable;
            _playButton.interactable = false;
        }

        private void OnDestroy()
        {
            GameInstance.UINavigation.OnGameStarted -= SelectEnemyShips;
            OnShipsPlayerSetted -= SetButtonInteractable;
        }

        public void SelectPlayerShip(Button ship)
        {
            if(_playerShips.Count>9) return;
            //ship.image.color = new Color(83f,133f,237f,255f);
            ship.interactable = false;
            _playerShips.Add(ship);
            
            if (_playerShips.Count == 10)
            {
                OnShipsPlayerSetted?.Invoke();
            }
        }

        private void SetButtonInteractable()
        {
            _playButton.interactable = true;
        }
        
        private void SelectEnemyShips()
        {
            _enemyShips.Clear();
            
            var selectedIndices = new List<int>();

            while (_enemyShips.Count < 10)
            {
                var randomIndex = Random.Range(0, _enemyField.Count);

                if (selectedIndices.Contains(randomIndex)) continue;
                selectedIndices.Add(randomIndex);
                var selectedShip = _enemyField[randomIndex];
                _enemyShips.Add(selectedShip);
            }
        }
    }
}