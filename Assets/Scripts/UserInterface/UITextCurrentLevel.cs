using GameCore;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class UITextCurrentLevel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        private void OnValidate()
        {
            _levelText ??= GetComponentInChildren<TextMeshProUGUI>();
        }

        protected void Start()
        {
            GameInstance.MapRoadNavigation.OnCurrentLevelValueChanged += OnLevelValueChanged;
        }
        
        private void OnDestroy()
        {
            GameInstance.MapRoadNavigation.OnCurrentLevelValueChanged -= OnLevelValueChanged;
        }
        
        private void OnLevelValueChanged(int level)
        {
            _levelText.text = $"Level {level + 1}";
        }
    }
}