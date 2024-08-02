using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class MapRoadNavigation : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _levelNumbersSprites;
        [SerializeField] private List<Button> _levelButtons;
        [SerializeField] private List<Image> _levelImages;
        private int _level;

        private int _currentLevel;

        public Action<int> OnCurrentLevelValueChanged;

        private int PrefsLevel
        {
            get => int.Parse(PlayerPrefs.GetString("PREFS_Level", "0"));
            set => PlayerPrefs.SetString("PREFS_Level", value.ToString());
        }

        public void Init()
        {
            _level = PrefsLevel;
            OpenButtonsLevel();
        }

        public void IncreaseLevel()
        {
            PrefsLevel = _level = (_level + 1);
            OpenButtonsLevel();
        }

        public void SetCurrentLevelIndex(int index)
        {
            _currentLevel = index;
            OnCurrentLevelValueChanged?.Invoke(_currentLevel);
        }
        
        public int GetCurrentLevelIndex()
        {
            return _currentLevel;
        }

        public void OpenButtonsLevel()
        {
            UpdateAllButtonStates();
            int numButtons = _levelButtons.Count;
            int numImages = _levelImages.Count;
            int numSprites = _levelNumbersSprites.Count;

            for (int i = 0; i < numButtons; i++)
            {
                if (i <= PrefsLevel)
                {
                    _levelButtons[i].interactable = true;

                    if (i < numImages)
                    {
                        if (i < numSprites)
                        {
                            _levelImages[i].sprite = _levelNumbersSprites[i];
                        }
                    }
                }
                else
                {
                    _levelButtons[i].interactable = false;
                }
            }
            UpdateChildObjects();
        }
        
        public void UpdateAllButtonStates()
        {
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                int starCount = PlayerPrefs.GetInt($"Level_{i}_Stars", 0); 

                var childButtons = _levelButtons[i].GetComponentsInChildren<Button>(true); 

                for (int j = 0; j < childButtons.Length; j++)
                {
                    childButtons[j].interactable = (j <= starCount);
                }
            }
        }
        
        public void UpdateChildObjects()
        {
            foreach (Button button in _levelButtons)
            {
                // Проверяем, доступна ли кнопка для взаимодействия
                bool isInteractable = button.interactable;

                // Включаем или выключаем все дочерние объекты
                foreach (Transform child in button.transform)
                {
                    child.gameObject.SetActive(isInteractable);
                }
            }
        }
    }
}
