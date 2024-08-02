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
        public List<CanvasGroup> MainPopups;
        public List<CanvasGroup> GameShelfPopups;
        [SerializeField] private Animator _transitionPopupAnimator;
        [SerializeField] private Animator _trainingPopupAnimator;
        
        public CanvasGroup LoadingMenuUI;
        public CanvasGroup GameMenuUI;

        [SerializeField] private TextMeshProUGUI _winTextGameOver;
        [SerializeField] private TextMeshProUGUI _buttonGameOverText;
        [SerializeField] private Button _gameOverNextButton;
        [SerializeField] private Button _gameOverWatchAdsButton;
        [SerializeField] private AudioCueScriptableObject StarIN;
        
        public Action OnGameStarted;

        private bool _isTraining = false;
        
        public void Init()
        {
            ResetPopups();
            OpenGroup(LoadingMenuUI);
            CloseGroup(GameMenuUI);
        }

        private void ResetPopups()
        {
            foreach (var popup in MainPopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
            
            foreach (var popup in GameShelfPopups)
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
        
        public void OpenGameUI(int levelIndex)
        {
            GameInstance.MapRoadNavigation.SetCurrentLevelIndex(levelIndex);
            StartCoroutine(OpenPopup(1,0,0));

            _isTraining = PlayerPrefs.GetInt("FirstPlay", 0) == 0;
        }
        
        public void OpenNextLVLGameUI()
        {
            GameInstance.MapRoadNavigation.SetCurrentLevelIndex(GameInstance.MapRoadNavigation.GetCurrentLevelIndex() + 1);
            StartCoroutine(OpenPopup(1,0,0));
            _gameOverNextButton.onClick.RemoveAllListeners();
        }
        
        public void OpenCurrentLVLGameUI()
        {
            GameInstance.MapRoadNavigation.SetCurrentLevelIndex(GameInstance.MapRoadNavigation.GetCurrentLevelIndex());
            StartCoroutine(OpenPopup(1,0,0));
            _gameOverNextButton.onClick.RemoveAllListeners();
        }
        
        public void CloseGameOverPopup()
        {
            CloseGroup(GameShelfPopups[0]);
            GameInstance.FXController.StopLoseFX();
        }
        
        public void OpenGameOverUI(bool isWin)
        {
            SelectUIPopup(GameShelfPopups,0);
            GameInstance.Audio.PlayGameOverSound();
            StartCoroutine(OpenStarsUI());
            if (isWin)
            {
                _buttonGameOverText.text = "NEXT";
                _winTextGameOver.text = "YOU WON!";
                GameInstance.FXController.PlayWinFX();
                _gameOverWatchAdsButton.interactable = false;
                _gameOverNextButton.onClick.AddListener(OpenNextLVLGameUI);
            }
            else
            {
                _buttonGameOverText.text = "REPEAT";
                _winTextGameOver.text = "GAME OVER";
                GameInstance.FXController.PlayLoseFX();
                _gameOverNextButton.onClick.AddListener(OpenCurrentLVLGameUI);
                _gameOverWatchAdsButton.interactable = true;
            }
        }

        private void HideTraining()
        {
            StartCoroutine(HideTrainingCoroutine());
        }

        private IEnumerator HideTrainingCoroutine()
        {
            yield return new WaitForSeconds(3f);
            TransitionAnimation();
            yield return new WaitForSeconds(1f);
            _trainingPopupAnimator.SetTrigger("TrainingOut");
            ResetPopups();
            SelectUIPopup(MainPopups, 1);
        }
        
        private IEnumerator OpenStarsUI()
        {
            for (int i = 0; i < GameInstance.ShelfMainMainController.StarCounter; i++)
            {
                GameInstance.FXController.StarsFX[i].gameObject.SetActive(true);
                GameInstance.FXController.StarsFX[i].Play();
                GameInstance.Audio.Play(StarIN);
                yield return new WaitForSeconds(1f);
                
                if (i < GameInstance.ShelfMainMainController._starButtons.Count)
                {
                    GameInstance.ShelfMainMainController._starButtons[i].interactable = true;
                }
            }
        }
        
        private IEnumerator OpenPopup(int index, int indexDop, int indexFX)
        {
            TransitionAnimation();
            yield return new WaitForSeconds(1f);
            GameInstance.MapRoadNavigation.OpenButtonsLevel();
            ResetPopups();
            SelectUIPopup(MainPopups, index);
            GameInstance.FXController.DisableWinShower();
            GameInstance.FXController.StopLoseFX();
            if (_isTraining)
            {
                SelectUIPopup(GameShelfPopups,1);
                _trainingPopupAnimator.SetTrigger("TrainingIN");
                _isTraining = false;
                PlayerPrefs.SetInt("FirstPlay", 1);
                HideTraining();
            }
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
            _transitionPopupAnimator.SetTrigger("Transition");
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

        private void SelectUIPopup(List<CanvasGroup> canvasGroups, int selectedIndexPopup)
        {
            for (var i = 0; i < canvasGroups.Count; i++)
            {
                if (i == selectedIndexPopup)
                {
                    canvasGroups[i].alpha = 1f;
                    canvasGroups[i].blocksRaycasts = true;
                    canvasGroups[i].interactable = true;
                }
                else
                {
                    canvasGroups[i].alpha = 0f;
                    canvasGroups[i].blocksRaycasts = false;
                    canvasGroups[i].interactable = false;
                }
            }
        }
    }
}