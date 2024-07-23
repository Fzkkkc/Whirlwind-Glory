using Services;
using UnityEngine;
using UserInterface;

namespace GameCore
{
    public class GameInstance : Singleton<GameInstance>
    {
        [SerializeField] private MoneyManager _moneyManager;
        [SerializeField] private UINavigation _uiNavigation;
        [SerializeField] private MapRoadNavigation _roadNavigation;
        [SerializeField] private FXController _fxController;
        [SerializeField] private AudioSystem _audio;
        [SerializeField] private ShelfMainController _shelfMainController;

        public static MoneyManager MoneyManager => Default._moneyManager;
        public static UINavigation UINavigation => Default._uiNavigation;
        public static MapRoadNavigation MapRoadNavigation => Default._roadNavigation;
        public static FXController FXController => Default._fxController;
        public static AudioSystem Audio => Default._audio;
        public static ShelfMainController ShelfMainMainController => Default._shelfMainController;

        protected override void Awake()
        { 
            //PlayerPrefs.DeleteAll();
            base.Awake();
            _moneyManager.Init(0);
            _uiNavigation.Init();
            _roadNavigation.Init();
            _fxController.Init();
            _audio.Init();
            _shelfMainController.Init();
        }
    }
}