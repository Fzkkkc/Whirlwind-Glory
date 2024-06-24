using UnityEngine;

namespace GameCore
{
    public class GameInstance : Singleton<GameInstance>
    {
        [SerializeField] private MoneyManager _moneyManager;

        public static MoneyManager MoneyManager => Default._moneyManager;

        protected override void Awake()
        {
            //PlayerPrefs.DeleteAll();
            base.Awake();
            _moneyManager.Init(0);
        }
    }
}