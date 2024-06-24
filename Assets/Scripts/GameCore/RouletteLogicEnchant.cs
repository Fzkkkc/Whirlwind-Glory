using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameCore
{
    public enum Combinations
    {
        Orange,
        Yellow,
        Green,
        Blue,
        CakeRed,
        CakeBlue
    }
    
    public class RouletteLogicEnchant : MonoBehaviour
    {
        [SerializeField] private List<Combinations> _firstRowCombinations;
        [SerializeField] private List<Combinations> _secondRowCombinations;
        [SerializeField] private List<Combinations> _thirdRowCombinations;
        [SerializeField] private List<Combinations> _fourthRowCombinations;
        [SerializeField] private List<Combinations> _fifthRowCombinations;
        [SerializeField] private List<Combinations> _sixthRowCombinations;
        [SerializeField] private List<float> _checkWinPosition;
        
        [SerializeField] private TextMeshProUGUI _betText;
        [SerializeField] private TextMeshProUGUI _winCountText;
        [SerializeField] private TextMeshProUGUI _winPopupText;
        
        [SerializeField] private Button _actionButton;
        
        [SerializeField] private int _maxBetCount = 2500;
        [SerializeField] private int _minBetCount = 25;
        [SerializeField] private Animator _baloonsAnimator;
        [SerializeField] private string _winAnimatorString;
        
        [HideInInspector] public int BetCount;
        
        private int _winCount = 0;
        
        public RectTransform[] rouletteRows; 
        
        public bool _isSpining = false;
        public int coroutineCounter = 0;
        
        [SerializeField] private bool _testMode;

        private void Start()
        {
            BetCount = _minBetCount;
            UpdateUiText();
            GameInstance.UINavigation.OnActivePopupChanged += ResetWinCounter;
        }

        private void OnDestroy()
        {
            GameInstance.UINavigation.OnActivePopupChanged -= ResetWinCounter;
        }

        private IEnumerator SpinRoulette(RectTransform row)
        {
            float duration = Random.Range(2.0f, 4.0f); 
            float elapsedTime = 0;
            float speed = Random.Range(1100.0f, 1400.0f);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newPositionY = row.localPosition.y - speed * Time.deltaTime;

                if (newPositionY <= _checkWinPosition[^1])
                {
                    newPositionY = 0;
                }

                row.localPosition = new Vector3(row.localPosition.x, newPositionY, row.localPosition.z);
                yield return null;
            }
            
            StartCoroutine(SmoothEnd(row));
        }

        private IEnumerator SmoothEnd(RectTransform row)
        {
            float duration = 0.5f; 
            float elapsedTime = 0;
            bool foundPos = false;
            float closestPosition = _checkWinPosition[0];
            foreach (float position in _checkWinPosition)
            {
                if (row.localPosition.y >= position && !foundPos)
                {
                    foundPos = true;
                    closestPosition = position;
                }
            }
            
            float startPositionY = row.localPosition.y;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                t = t * t * (3f - 2f * t); 
                float newPositionY = Mathf.Lerp(startPositionY, closestPosition, t);
                row.localPosition = new Vector3(row.localPosition.x, newPositionY, row.localPosition.z);
                yield return null;
            }

            row.localPosition = new Vector3(row.localPosition.x, closestPosition, row.localPosition.z);
            
            coroutineCounter++;
            if (coroutineCounter == rouletteRows.Length && _isSpining)
            {
                _actionButton.interactable = true;
                CheckWin();
                _isSpining = false;
                coroutineCounter = 0;
            }
        }

        public void SpinRoulette()
        {
            if(!GameInstance.MoneyManager.HasEnoughCoinsCurrency((ulong)BetCount)) return;
            GameInstance.MoneyManager.SpendCoinsCurrency((ulong)BetCount);
            _actionButton.interactable = false;
            foreach (var row in rouletteRows)
            {
                _isSpining = true;
                StartCoroutine(SpinRoulette(row));
            }
            
        }
        
        public void RaiseBet(int betCount)
        {
            if (BetCount == _minBetCount && betCount < 0) return;
            if (BetCount == _maxBetCount && betCount > 0) return;
            BetCount += betCount;
            UpdateUiText();
        }

        private void UpdateUiText()
        {
            _betText.text = $"{BetCount}";
            _winCountText.text = $"{_winCount}";
        }
        
        private void CheckWin()
        {
            float TOLERANCE = 0.001f;
            
            if (rouletteRows[0].localPosition.y == _checkWinPosition[0])
            {
                _firstRowCombinations[0] = Combinations.Yellow;
                _firstRowCombinations[1] = Combinations.Orange;
                _firstRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[1])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.Orange;
                _firstRowCombinations[1] = Combinations.CakeBlue;
                _firstRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[2])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.CakeBlue;
                _firstRowCombinations[1] = Combinations.Green;
                _firstRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[3])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.Green;
                _firstRowCombinations[1] = Combinations.CakeRed;
                _firstRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[4])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.CakeRed;
                _firstRowCombinations[1] = Combinations.Green;
                _firstRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[5])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.Green;
                _firstRowCombinations[1] = Combinations.Blue;
                _firstRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[6])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.Blue;
                _firstRowCombinations[1] = Combinations.CakeRed;
                _firstRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[7])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.CakeRed;
                _firstRowCombinations[1] = Combinations.Blue;
                _firstRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[8])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.Blue;
                _firstRowCombinations[1] = Combinations.Yellow;
                _firstRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[0].localPosition.y - (_checkWinPosition[9])) < TOLERANCE)
            {
                _firstRowCombinations[0] = Combinations.Yellow;
                _firstRowCombinations[1] = Combinations.Orange;
                _firstRowCombinations[2] = Combinations.CakeBlue;
            }
            //2nd
            if (rouletteRows[1].localPosition.y == _checkWinPosition[0])
            {
                _secondRowCombinations[0] = Combinations.CakeRed;
                _secondRowCombinations[1] = Combinations.CakeBlue;
                _secondRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[1])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.CakeBlue;
                _secondRowCombinations[1] = Combinations.Green;
                _secondRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[2])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Green;
                _secondRowCombinations[1] = Combinations.Yellow;
                _secondRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[3])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Yellow;
                _secondRowCombinations[1] = Combinations.Blue;
                _secondRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[4])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Blue;
                _secondRowCombinations[1] = Combinations.Yellow;
                _secondRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[5])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Yellow;
                _secondRowCombinations[1] = Combinations.Orange;
                _secondRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[6])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Orange;
                _secondRowCombinations[1] = Combinations.Blue;
                _secondRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[7])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Blue;
                _secondRowCombinations[1] = Combinations.Orange;
                _secondRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[8])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.Orange;
                _secondRowCombinations[1] = Combinations.CakeRed;
                _secondRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[1].localPosition.y - (_checkWinPosition[9])) < TOLERANCE)
            {
                _secondRowCombinations[0] = Combinations.CakeRed;
                _secondRowCombinations[1] = Combinations.CakeBlue;
                _secondRowCombinations[2] = Combinations.Green;
            }
            //3rd
            if (rouletteRows[2].localPosition.y == _checkWinPosition[0])
            {
                _thirdRowCombinations[0] = Combinations.Blue;
                _thirdRowCombinations[1] = Combinations.CakeRed;
                _thirdRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[1])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.CakeRed;
                _thirdRowCombinations[1] = Combinations.Orange;
                _thirdRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[2])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.Orange;
                _thirdRowCombinations[1] = Combinations.CakeBlue;
                _thirdRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[3])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.CakeBlue;
                _thirdRowCombinations[1] = Combinations.CakeBlue;
                _thirdRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[4])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.CakeBlue;
                _thirdRowCombinations[1] = Combinations.Green;
                _thirdRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[5])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.Green;
                _thirdRowCombinations[1] = Combinations.Yellow;
                _thirdRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[6])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.Yellow;
                _thirdRowCombinations[1] = Combinations.Yellow;
                _thirdRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[7])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.Yellow;
                _thirdRowCombinations[1] = Combinations.Green;
                _thirdRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[8])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.Green;
                _thirdRowCombinations[1] = Combinations.Blue;
                _thirdRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[2].localPosition.y - (_checkWinPosition[9])) < TOLERANCE)
            {
                _thirdRowCombinations[0] = Combinations.Blue;
                _thirdRowCombinations[1] = Combinations.CakeRed;
                _thirdRowCombinations[2] = Combinations.Orange;
            }
            //4th
            if (rouletteRows[3].localPosition.y == _checkWinPosition[0])
            {
                _fourthRowCombinations[0] = Combinations.Green;
                _fourthRowCombinations[1] = Combinations.Blue;
                _fourthRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[1])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.Blue;
                _fourthRowCombinations[1] = Combinations.CakeRed;
                _fourthRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[2])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.CakeRed;
                _fourthRowCombinations[1] = Combinations.CakeBlue;
                _fourthRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[3])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.CakeBlue;
                _fourthRowCombinations[1] = Combinations.Yellow;
                _fourthRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[4])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.Yellow;
                _fourthRowCombinations[1] = Combinations.Orange;
                _fourthRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[5])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.Orange;
                _fourthRowCombinations[1] = Combinations.Orange;
                _fourthRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[6])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.Orange;
                _fourthRowCombinations[1] = Combinations.CakeBlue;
                _fourthRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[7])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.CakeBlue;
                _fourthRowCombinations[1] = Combinations.Yellow;
                _fourthRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[8])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.Yellow;
                _fourthRowCombinations[1] = Combinations.Green;
                _fourthRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[3].localPosition.y - (_checkWinPosition[9])) < TOLERANCE)
            {
                _fourthRowCombinations[0] = Combinations.Green;
                _fourthRowCombinations[1] = Combinations.Blue;
                _fourthRowCombinations[2] = Combinations.CakeRed;
            }
            //5th
            if (rouletteRows[4].localPosition.y == _checkWinPosition[0])
            {
                _fifthRowCombinations[0] = Combinations.Orange;
                _fifthRowCombinations[1] = Combinations.Green;
                _fifthRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[1])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.Green;
                _fifthRowCombinations[1] = Combinations.Yellow;
                _fifthRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[2])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.Yellow;
                _fifthRowCombinations[1] = Combinations.CakeRed;
                _fifthRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[3])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.CakeRed;
                _fifthRowCombinations[1] = Combinations.Blue;
                _fifthRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[4])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.Blue;
                _fifthRowCombinations[1] = Combinations.CakeBlue;
                _fifthRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[5])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.CakeBlue;
                _fifthRowCombinations[1] = Combinations.CakeRed;
                _fifthRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[6])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.CakeRed;
                _fifthRowCombinations[1] = Combinations.CakeBlue;
                _fifthRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[7])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.CakeBlue;
                _fifthRowCombinations[1] = Combinations.Blue;
                _fifthRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[8])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.Blue;
                _fifthRowCombinations[1] = Combinations.Orange;
                _fifthRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[4].localPosition.y - (_checkWinPosition[9])) < TOLERANCE)
            {
                _fifthRowCombinations[0] = Combinations.Orange;
                _fifthRowCombinations[1] = Combinations.Green;
                _fifthRowCombinations[2] = Combinations.Yellow;
            }
            //6th
            if (rouletteRows[5].localPosition.y == _checkWinPosition[0])
            {
                _sixthRowCombinations[0] = Combinations.CakeBlue;
                _sixthRowCombinations[1] = Combinations.Yellow;
                _sixthRowCombinations[2] = Combinations.Blue;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[1])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.Yellow;
                _sixthRowCombinations[1] = Combinations.Blue;
                _sixthRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[2])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.Blue;
                _sixthRowCombinations[1] = Combinations.Green;
                _sixthRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[3])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.Green;
                _sixthRowCombinations[1] = Combinations.Orange;
                _sixthRowCombinations[2] = Combinations.Orange;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[4])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.Orange;
                _sixthRowCombinations[1] = Combinations.Orange;
                _sixthRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[5])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.Orange;
                _sixthRowCombinations[1] = Combinations.CakeRed;
                _sixthRowCombinations[2] = Combinations.Green;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[6])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.CakeRed;
                _sixthRowCombinations[1] = Combinations.Green;
                _sixthRowCombinations[2] = Combinations.CakeRed;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[7])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.Green;
                _sixthRowCombinations[1] = Combinations.CakeRed;
                _sixthRowCombinations[2] = Combinations.CakeBlue;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[8])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.CakeRed;
                _sixthRowCombinations[1] = Combinations.CakeBlue;
                _sixthRowCombinations[2] = Combinations.Yellow;
            }
            else if (Math.Abs(rouletteRows[5].localPosition.y - (_checkWinPosition[9])) < TOLERANCE)
            {
                _sixthRowCombinations[0] = Combinations.CakeBlue;
                _sixthRowCombinations[1] = Combinations.Yellow;
                _sixthRowCombinations[2] = Combinations.Blue;
            }
            
            if (_firstRowCombinations[0] == _secondRowCombinations[0] &&
                _firstRowCombinations[0] == _thirdRowCombinations[0] && 
                _firstRowCombinations[0] == _fourthRowCombinations[0] && 
                _firstRowCombinations[0] == _fifthRowCombinations[0] && 
                _firstRowCombinations[0] == _sixthRowCombinations[0])
            {
                WinBonus();
            }
            
            if (_firstRowCombinations[1] == _secondRowCombinations[1] &&
                _firstRowCombinations[1] == _thirdRowCombinations[1] && 
                _firstRowCombinations[1] == _fourthRowCombinations[1] && 
                _firstRowCombinations[1] == _fifthRowCombinations[1] && 
                _firstRowCombinations[1] == _sixthRowCombinations[1])
            {
                WinBonus();
            }
            
            if (_firstRowCombinations[2] == _secondRowCombinations[2] &&
                _firstRowCombinations[2] == _thirdRowCombinations[2] && 
                _firstRowCombinations[2] == _fourthRowCombinations[2] && 
                _firstRowCombinations[2] == _fifthRowCombinations[2] && 
                _firstRowCombinations[2] == _sixthRowCombinations[2])
            {
                WinBonus();
            }

            if (_testMode)
            {
                WinBonus();
            }
        }
        
        private void WinBonus()
        {
            var reward = Random.Range(50, 200);
            reward += BetCount*2;
            GameInstance.MoneyManager.AddCoinsCurrency((ulong) reward);
            _winCount += reward;
            UpdateUiText();

            _winPopupText.text = $"WIN {reward}";

            if (reward > 3500)
            {
                _baloonsAnimator.SetTrigger(_winAnimatorString);
            }
            else
            {
                _baloonsAnimator.SetTrigger("ConfettiIn");
            }
        }

        private void ResetWinCounter()
        {
            _winCount = 0;
            UpdateUiText();
        }
    }
}