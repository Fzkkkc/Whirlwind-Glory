using System;
using GameCore;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class UITextPlayerMoves : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerMovesText;

        private void OnValidate()
        {
            _playerMovesText ??= GetComponentInChildren<TextMeshProUGUI>();
        }

        protected void Start()
        {
            GameInstance.ShelfMainMainController.OnPlayerMovesValueChanged += OnMovesValueChanged;
        }
        
        private void OnDestroy()
        {
            GameInstance.ShelfMainMainController.OnPlayerMovesValueChanged -= OnMovesValueChanged;
        }
        
        private void OnMovesValueChanged(int playerMoves) 
        {
            _playerMovesText.SetText("Move " + playerMoves.ToString());
        }
    }
}