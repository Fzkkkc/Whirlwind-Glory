using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    public class UINavigation : MonoBehaviour
    {
        public List<CanvasGroup> GamePopups;
        [SerializeField] private Animator _transitionAnimator;
        
        public CanvasGroup LoadingMenu;
        public CanvasGroup GameMenu;

        private void Start()
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
        }
        
        public void OpenMenu()
        {
            StartCoroutine(OpenPopup(0));
        }
        
        public void OpenWheelPopup()
        {
            StartCoroutine(OpenPopup(1));
        }

        public void OpenSlotCandy()
        {
            StartCoroutine(OpenPopup(2));
        }
        
        public void OpenSlotSea()
        {
            StartCoroutine(OpenPopup(3));
        }
        
        private IEnumerator OpenPopup(int index)
        {
            TransitionAnimation();
            yield return new WaitForSeconds(1f);
            SelectPopup(index);
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
        }
    }
}