using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameCore
{
    public class ShipBattleGame : MonoBehaviour
    {
        [SerializeField] private List<Button> _playerShips;
        [SerializeField] private List<Button> _enemyShips;
        [SerializeField] private List<Button> _playerBattleShips;
        
        [SerializeField] private List<Button> _playerField;
        [SerializeField] private List<Button> _enemyField;
        [SerializeField] private Button _playButton;

        [SerializeField] private Sprite _fishSprite;
        [SerializeField] private Sprite _markSprite;

        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI _enemyScoreText;
        [SerializeField] private TextMeshProUGUI _cellText;
        
        private int _playerScore;
        private int _enemyScore;
        
        private Action OnShipsPlayerSetted;
        
        private void Start()
        {
            GameInstance.UINavigation.OnGameStarted += SelectEnemyShips;
            OnShipsPlayerSetted += SetButtonInteractable;
            _playButton.interactable = false;
            
            foreach (var button in _enemyField)
            {
                button.onClick.AddListener(() => PlayerChoice(button));
            }
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
                TransferPlayerBattleShips();
            }
            UpdateTextUI();
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
            
            UpdateTextUI();
        }

        private void PlayerChoice(Button enemyFieldButton)
        {
            if (_enemyShips.Contains(enemyFieldButton))
            {
                enemyFieldButton.interactable = false;
                enemyFieldButton.image.sprite = _fishSprite;
                _playerScore++;
            }
            else
            {
                enemyFieldButton.interactable = false;
                enemyFieldButton.image.sprite = _markSprite;
            }
            UpdateTextUI();
            
            EnemyChoice();
        }

        private void EnemyChoice()
        {
            var availableButtons = _playerField.FindAll(button => button.interactable);
            
            if (availableButtons.Count == 0) return; // если нет доступных кнопок, выход
            
            var randomIndex = Random.Range(0, availableButtons.Count);
            var chosenButton = availableButtons[randomIndex];
            
            if (_playerBattleShips.Contains(chosenButton))
            {
                chosenButton.interactable = false;
                chosenButton.image.sprite = _fishSprite;
                _enemyScore++;
            }
            else
            {
                chosenButton.interactable = false;
                chosenButton.image.sprite = _markSprite;
            }
            UpdateTextUI();
        }

        private void UpdateTextUI()
        {
            _cellText.text = $"{_playerShips.Count}/10";
            _playerScoreText.text = _playerScore.ToString();
            _enemyScoreText.text = _enemyScore.ToString();
        }
        
        private void TransferPlayerBattleShips()
        {
            _playerBattleShips.Clear();

            foreach (var playerFieldButton in _playerField)
            {
                foreach (var playerShip in _playerShips)
                {
                    if (playerFieldButton.name == playerShip.name)
                    {
                        _playerBattleShips.Add(playerFieldButton);
                    }
                }
            }
        }
    }
}