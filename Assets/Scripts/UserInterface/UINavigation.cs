using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class UINavigation : MonoBehaviour
    {
        public List<CanvasGroup> GamePopups;
        public List<CanvasGroup> GameShipPopups;
        public List<CanvasGroup> GameBattleShipPopups;
        [SerializeField] private Animator _transitionAnimator;
        
        public CanvasGroup LoadingMenu;
        public CanvasGroup GameMenu;

        [SerializeField] private TextMeshProUGUI _winText;
        [SerializeField] private TextMeshProUGUI _buttonGameOverText;
        [SerializeField] private Button _gameOverNextButton;
        [SerializeField] private Button _gameOverWatchAdsButton;
        
        public Action OnActivePopupChanged;
        public Action OnGameStarted;
        
        public void Init()
        {
            ResetPopups();
            OpenGroup(LoadingMenu);
            CloseGroup(GameMenu);
        }

        private void ResetPopups()
        {
            foreach (var popup in GamePopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
            
            foreach (var popup in GameShipPopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
            
            foreach (var popup in GameBattleShipPopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
        }
        
        public void OpenMenu()
        {
            StartCoroutine(OpenPopup(0,0,1));
        }
        
        public void OpenWheelPopup()
        {
            StartCoroutine(OpenPopup(1,0,2));
        }

        public void OpenGameUI(int levelIndex)
        {
            GameInstance.MapRoadNavigation.SetCurrentLevelIndex(levelIndex);
            StartCoroutine(OpenPopup(2,0,0));
        }
        
        public void OpenNextLVLGameUI()
        {
            GameInstance.MapRoadNavigation.SetCurrentLevelIndex(GameInstance.MapRoadNavigation.GetCurrentLevelIndex() + 1);
            StartCoroutine(OpenPopup(2,0,0));
        }
        
        public void OpenCurrentLVLGameUI()
        {
            GameInstance.MapRoadNavigation.SetCurrentLevelIndex(GameInstance.MapRoadNavigation.GetCurrentLevelIndex());
            StartCoroutine(OpenPopup(2,0,0));
        }

        public void OpenBattleUI()
        {
            StartCoroutine(OpenPopup(2, 1,0));
        }

        public void OpenPauseUI()
        {
            SelectGameBattlePopup(0);
        }

        public void ClosePauseUI()
        {
            CloseGroup(GameBattleShipPopups[0]);
        }

        public void CloseGameOverPopup()
        {
            CloseGroup(GameBattleShipPopups[1]);
            GameInstance.FXController.StopLoseFX();
        }
        
        public void OpenGameOverUI(bool isWin)
        {
            SelectGameBattlePopup(1);
            GameInstance.Audio.PlayGameOverSound();
            if (isWin)
            {
                _buttonGameOverText.text = "NEXT";
                _winText.text = "YOU WON!";
                GameInstance.FXController.PlayWinFX();
                _gameOverWatchAdsButton.interactable = false;
                _gameOverNextButton.onClick.AddListener(OpenNextLVLGameUI);
            }
            else
            {
                _buttonGameOverText.text = "REPEAT";
                _winText.text = "GAME OVER";
                GameInstance.FXController.PlayLoseFX();
                _gameOverNextButton.onClick.AddListener(OpenCurrentLVLGameUI);
                _gameOverWatchAdsButton.interactable = true;
            }
        }
        
        private IEnumerator OpenPopup(int index, int indexDop, int indexFX)
        {
            TransitionAnimation();
            yield return new WaitForSeconds(1f);
            ResetPopups();
            SelectPopup(index);
            SelectGamePopup(indexDop);
            GameInstance.FXController.DisableWinShower();
            GameInstance.FXController.StopLoseFX();
            switch (indexFX)
            {
                case 0:
                    GameInstance.FXController.PlayBackgroundParticleGame();
                    GameInstance.FXController.DisableBackgroundParticleMenu();
                    OnGameStarted?.Invoke();
                    break;
                case 1:
                    GameInstance.FXController.DisableBackgroundParticleGame();
                    GameInstance.FXController.PlayBackgroundParticleMenu();
                    break;
                case 2:
                    GameInstance.FXController.DisableParticles();
                    break;
            }
        }

        public void TransitionAnimation()
        {
            _transitionAnimator.SetTrigger("Transition");
        }
        
        public void OpenGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        
        public void CloseGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        
        private void SelectPopup(int selectedIndex)
        {
            for (var i = 0; i < GamePopups.Count; i++)
            {
                if (i == selectedIndex)
                {
                    GamePopups[i].alpha = 1f;
                    GamePopups[i].blocksRaycasts = true;
                    GamePopups[i].interactable = true;
                }
                else
                {
                    GamePopups[i].alpha = 0f;
                    GamePopups[i].blocksRaycasts = false;
                    GamePopups[i].interactable = false;
                }
            }
            
            OnActivePopupChanged?.Invoke();
        }
        
        private void SelectGamePopup(int selectedIndex)
        {
            for (var i = 0; i < GameShipPopups.Count; i++)
            {
                if (i == selectedIndex)
                {
                    GameShipPopups[i].alpha = 1f;
                    GameShipPopups[i].blocksRaycasts = true;
                    GameShipPopups[i].interactable = true;
                }
                else
                {
                    GameShipPopups[i].alpha = 0f;
                    GameShipPopups[i].blocksRaycasts = false;
                    GameShipPopups[i].interactable = false;
                }
            }
            
            OnActivePopupChanged?.Invoke();
        }
        
        private void SelectGameBattlePopup(int selectedIndex)
        {
            for (var i = 0; i < GameBattleShipPopups.Count; i++)
            {
                if (i == selectedIndex)
                {
                    GameBattleShipPopups[i].alpha = 1f;
                    GameBattleShipPopups[i].blocksRaycasts = true;
                    GameBattleShipPopups[i].interactable = true;
                }
                else
                {
                    GameBattleShipPopups[i].alpha = 0f;
                    GameBattleShipPopups[i].blocksRaycasts = false;
                    GameBattleShipPopups[i].interactable = false;
                }
            }
            
            OnActivePopupChanged?.Invoke();
        }
    }
}