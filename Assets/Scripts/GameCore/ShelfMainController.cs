using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class ShelfMainController : MonoBehaviour
    {
        public Action OnPlayersMove;

        public ShelfController _smallShelf;
        public ShelfController _bigShelf;
        public GameTimer GameTimer;

        public Action OnBigShelfGameStarted;
        public Action OnSmallShelfGameStarted;
        
        private int _playerMoves;
        
        public Action<int> OnPlayerMovesValueChanged;
        [SerializeField] private Slider _gameSlider;

        private int _playerScore;
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI _playerScoreTextGameOver;
        [SerializeField] public List<Button> _starButtons;
        
        private int _playerMatched = 0;

        public int _initalMoves = 0;
        public int StarCounter;
        
        private int PlayerMoves
        {
            get => _playerMoves;
            set => _playerMoves = value;
        }
        
        public void Init()
        {
            GameInstance.UINavigation.OnGameStarted += StartGame;
            OnPlayersMove += DecreasePlayerMoves;
        }
        
        public void OnDestroy()
        {
            GameInstance.UINavigation.OnGameStarted -= StartGame;
            OnPlayersMove -= DecreasePlayerMoves;
        }

        private void StartGame()
        {
            if (GameInstance.MapRoadNavigation.GetCurrentLevelIndex() % 2 == 0)
            {
                _smallShelf.gameObject.SetActive(true);
                _bigShelf.gameObject.SetActive(false);
                OnSmallShelfGameStarted?.Invoke();
            }
            else
            {
                _bigShelf.gameObject.SetActive(true);
                _smallShelf.gameObject.SetActive(false);
                OnBigShelfGameStarted?.Invoke();
            }

            StarCounter = 0;
            _gameSlider.value = 0;
            _playerScore = 0;
            _playerMatched = 0;
            UpdateUI();
            GameTimer.StartTimer();
            SetStarGameOverButtonsInteractable();
        }

        private void SetStarGameOverButtonsInteractable()
        {
            foreach (var starButton in _starButtons)
            {
                starButton.interactable = false;
            }
        }
        
        public void CheckWin()
        {
            if (_playerMatched == (_initalMoves - 20) / 3) 
            {
                GameInstance.MapRoadNavigation.IncreaseLevel();
                GameTimer.StopTimer();
                SetStarsOfSlider();
                GameInstance.UINavigation.OpenGameOverUI(true);
            }
        }

        public void ContinueGame()
        {
            IncreasePlayerMoves();
            GameInstance.UINavigation.CloseGameOverPopup();
        }

        private void SetStarsOfSlider()
        {
            if (_gameSlider.value >= 20 &&_gameSlider.value < 70)
            {
                StarCounter = 1;
            }   
            else if(_gameSlider.value >= 70 && _gameSlider.value < 90)
            {
                StarCounter = 2;
            } 
            else if(_gameSlider.value >= 90)
            {
                StarCounter = 3;
            }
            
            int currentLevelIndex = GameInstance.MapRoadNavigation.GetCurrentLevelIndex();
            
            int previousStarCount = PlayerPrefs.GetInt($"Level_{currentLevelIndex}_Stars", 0); 
            
            if (StarCounter > previousStarCount)
            {
                PlayerPrefs.SetInt($"Level_{currentLevelIndex}_Stars", StarCounter);
                PlayerPrefs.Save(); 
            }
        }
        
        public void IncreaseMatches()
        {
            _playerMatched++;
        }
        
        public void SetPlayerMoves(int moves)
        {
            PlayerMoves = moves;
            OnPlayerMovesValueChanged?.Invoke(PlayerMoves);
            _initalMoves = PlayerMoves;
        }

        private void DecreasePlayerMoves()
        {
            if (PlayerMoves <= 0) return;
            PlayerMoves--;
            OnPlayerMovesValueChanged?.Invoke(PlayerMoves);

            if (PlayerMoves <= 0)
            {
                GameInstance.UINavigation.OpenGameOverUI(false);
                GameTimer.StopTimer();
                SetStarsOfSlider();
            }
        }
        
        public void IncreasePlayerMoves()
        {
            PlayerMoves += 5;
            OnPlayerMovesValueChanged?.Invoke(PlayerMoves);
            GameInstance.FXController.PlayEnergyFX();
        }

        public void IncreaseSliderAndScoreValue()
        {
            StartCoroutine(SmoothSliderIncrease());
        }

        private IEnumerator SmoothSliderIncrease()
        {
            var startScore = _playerScore;
            var targetScore = _playerScore + 100;
            var targetValue = _gameSlider.value + 10;
            var startValue = _gameSlider.value;
            var elapsedTime = 0f;
            GameInstance.FXController.PlayStarStream();
            while (elapsedTime < 0.5f)
            {
                elapsedTime += Time.deltaTime;
                _gameSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime /  0.5f);
                _playerScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, elapsedTime / 0.5f));
                UpdateUI();
                yield return null; 
            }
            _playerScore = targetScore;
            _gameSlider.value = targetValue;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _playerScoreText.text = $"{_playerScore}";
            _playerScoreTextGameOver.text = $"{_playerScore}";
        }
    }
}