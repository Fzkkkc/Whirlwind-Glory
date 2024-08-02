using System;
using System.Collections;
using GameCore;
using UnityEngine;

namespace UserInterface
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private UINavigation _uiNavigation;

        private void Start()
        {
            LoadingState();
        }

        private void LoadingState()
        {
            StartCoroutine(AnimatePercentage());
        }
        
        private IEnumerator AnimatePercentage()
        {
            _uiNavigation.OpenGroup(_uiNavigation.LoadingMenuUI);
            yield return new WaitForSeconds(2f);
            _uiNavigation.TransitionAnimation();
            yield return new WaitForSeconds(1f);
            _uiNavigation.OpenGroup(_uiNavigation.GameMenuUI);
            _uiNavigation.OpenGroup(_uiNavigation.MainPopups[0]);
            _uiNavigation.CloseGroup(_uiNavigation.LoadingMenuUI);
            GameInstance.FXController.DisableBackgroundParticleGame();
            GameInstance.FXController.PlayBackgroundParticleMenu();
        }
    }
}