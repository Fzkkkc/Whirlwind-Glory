using GameCore;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class UITextMoney : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyText;

        private void OnValidate()
        {
            _currencyText ??= GetComponentInChildren<TextMeshProUGUI>();
        }

        protected void Start()
        {
            GameInstance.MoneyManager.OnCoinsCurrencyChange += OnMoneyChanged;
            OnMoneyChanged(GameInstance.MoneyManager.GetCoinsCurrency());
        }
        
        private void OnDestroy()
        {
            GameInstance.MoneyManager.OnCoinsCurrencyChange -= OnMoneyChanged;
        }
        
        private void OnMoneyChanged(ulong money) 
        {
            _currencyText.SetText(money.ToString());
        }
    }
}