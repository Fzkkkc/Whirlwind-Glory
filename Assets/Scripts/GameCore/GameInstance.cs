using UnityEngine;
using UserInterface;

namespace GameCore
{
    public class GameInstance : Singleton<GameInstance>
    {
        [SerializeField] private MoneyManager _moneyManager;
        [SerializeField] private UINavigation _uiNavigation;
        [SerializeField] private MapRoadNavigation _roadNavigation;

        public static MoneyManager MoneyManager => Default._moneyManager;
        public static UINavigation UINavigation => Default._uiNavigation;
        public static MapRoadNavigation MapRoadNavigation => Default._roadNavigation;

        protected override void Awake()
        { 
            //PlayerPrefs.DeleteAll();
            base.Awake();
            _moneyManager.Init(0);
            _uiNavigation.Init();
            _roadNavigation.Init();
        }
    }
}