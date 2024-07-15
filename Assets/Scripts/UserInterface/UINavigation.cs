using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Action OnActivePopupChanged;
        public Action OnGameStarted;
        
        public void Init()
        {
            ResetPopups();
            OpenGroup(LoadingMenu);
            CloseGroup(GameMenu);
            ResetPopups();
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
            StartCoroutine(OpenPopup(0,0));
        }
        
        public void OpenWheelPopup()
        {
            StartCoroutine(OpenPopup(1,0));
        }

        public void OpenShipUI()
        {
            StartCoroutine(OpenPopup(2,0));
            OnGameStarted?.Invoke();
        }

        public void OpenBattleUI()
        {
            StartCoroutine(OpenPopup(2, 1));
        }
        
        private IEnumerator OpenPopup(int index, int indexDop)
        {
            TransitionAnimation();
            yield return new WaitForSeconds(1f);
            SelectPopup(index);
            SelectShipPopup(indexDop);
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
        
        private void SelectShipPopup(int selectedIndex)
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
        
        private void SelectShipBattlePopup(int selectedIndex)
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