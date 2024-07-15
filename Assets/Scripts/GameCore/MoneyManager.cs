using System;
using UnityEngine;

namespace GameCore
{
    public class MoneyManager : MonoBehaviour
    {
        private ulong _coins;
        public Action<ulong> OnCoinsCurrencyChange;
        public Action OnCoinsCurrencyValueChange;
        
        private ulong PrefsMoney
        {
            get => ulong.Parse(PlayerPrefs.GetString("PREFS_Money", "500"));
            set => PlayerPrefs.SetString("PREFS_Money", value.ToString());
        }
        public void Init(ulong startMoney)
        {
            //PlayerPrefs.DeleteKey("PREFS_Money");
            _coins = PrefsMoney;
            //if (GetCoinsCurrency() <= 0UL)
            AddCoinsCurrency(startMoney);
        }
        
        public ulong GetCoinsCurrency()
        {
            return _coins;
        }
        
        public void AddCoinsCurrency(ulong count)
        {
            PrefsMoney = _coins = (_coins + count);
            OnCoinsCurrencyChange?.Invoke(_coins);
            OnCoinsCurrencyValueChange?.Invoke();
        }
        
        public void SpendCoinsCurrency(ulong count)
        {
            ulong result = 0UL;
            if (_coins >= count)
                result = _coins - count;
            PrefsMoney = _coins = result;
            OnCoinsCurrencyChange?.Invoke(_coins);
            OnCoinsCurrencyValueChange?.Invoke();
        }
        
        public bool HasEnoughCoinsCurrency(ulong amount)
        {
            return _coins >= amount;
        }
    }
}