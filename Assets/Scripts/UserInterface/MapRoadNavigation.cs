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

        private void OnValidate()
        {
            
        }

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

        private void OpenButtonsLevel()
        {
            for (int i = 0; i <= PrefsLevel; i++)
            {
                _levelButtons[i].interactable = true;
                _levelImages[i].sprite = _levelNumbersSprites[i];
            }
        }
    }
}