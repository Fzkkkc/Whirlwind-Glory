using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;
using Random = UnityEngine.Random;

namespace GameCore
{
    public class WheelLogic : MonoBehaviour
    {
        [SerializeField] private RectTransform _wheelPlayablePart;
        [SerializeField] private Button _wheelButtonInteraction;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private List<Sprite> _rewardSprites;
        [SerializeField] private Animator _rewardAnimator;
        [SerializeField] private UINavigation _gameUINavigation;

        private bool _isInPopup;
        private ulong _rewardCoins;
        private DateTime lastClaimDailyBonusTime;
        private DateTime nextClaimTime;

        private void Start()
        {
            InitializeLastClaimTime();
            UpdateDailyButtonState();
        }
        
        private void Update()
        {
            if(_gameUINavigation.GamePopups[1].interactable)
                UpdateTimer();
        }

        public void StartWheel()
        {
            //if (!GameInstance.MoneyManager.HasEnoughCoinsCurrency(1000)) return;
            GameInstance.MoneyManager.SpendCoinsCurrency(1000);
            StartCoroutine(SpinWheel());
        }

        private void InitializeLastClaimTime()
        {
            var lastTime = PlayerPrefs.GetString("LastClaimDailyBonusTime", "");
            if (!string.IsNullOrEmpty(lastTime))
            {
                lastClaimDailyBonusTime = DateTime.Parse(lastTime);
            }
            else
            {
                lastClaimDailyBonusTime = DateTime.MinValue;
            }
        }

        private void UpdateDailyButtonState()
        {
            TimeSpan interval = TimeSpan.FromMinutes(15);
            nextClaimTime = lastClaimDailyBonusTime + interval;

            if (DateTime.Now > nextClaimTime)
            {
                _wheelButtonInteraction.interactable = true;
            }
            else
            {
                _wheelButtonInteraction.interactable = false;
            }
        }

        private string GetTimeToNextClaimDailyBonus()
        {
            TimeSpan timeRemaining = nextClaimTime - DateTime.Now;
            int hours = Mathf.FloorToInt((float)timeRemaining.TotalHours);
            int minutes = Mathf.FloorToInt((float)timeRemaining.TotalMinutes) % 60;
            int seconds = Mathf.FloorToInt((float)timeRemaining.TotalSeconds) % 60;

            string secondsString = seconds < 10 ? "0" + seconds : seconds.ToString();

            return $"{hours}:{minutes}:{secondsString}";
        }
        
        private void UpdateTimer()
        {
            _timerText.text = _wheelButtonInteraction.interactable ? "" : GetTimeToNextClaimDailyBonus();
        }

        public void GetDailyBonus()
        {
            PlayerPrefs.SetString("LastClaimDailyBonusTime", DateTime.Now.ToString());
            _wheelButtonInteraction.interactable = false;
            Debug.Log("Daily bonus claimed!");
        }
        
        public void ShowRewardPopup(float playerValue)
        {
            _rewardAnimator.SetTrigger("RewardIn");

            _isInPopup = true;
            ulong reward = 0;

            if (playerValue <= -720 && playerValue >= -745)
            {
                _rewardImage.sprite = _rewardSprites[2];
                reward = 300;
                Debug.Log("3 gems");
            }
            else if (playerValue <= -745.1 && playerValue >= -790)
            {
                _rewardImage.sprite = _rewardSprites[0];
                reward = 100;
                Debug.Log("1 gem");
            }
            else if (playerValue <= -790.1 && playerValue >= -832)
            {
                _rewardImage.sprite = _rewardSprites[1];
                reward = 200;
                Debug.Log("2 gems");
            }
            else if (playerValue <= -832.1 && playerValue >= -877)
            {
                _rewardImage.sprite = _rewardSprites[2];
                reward = 300;
                Debug.Log("3 gems");
            }
            else if (playerValue <= -877.1 && playerValue >= -922)
            {
                _rewardImage.sprite = _rewardSprites[1];
                reward = 200;
                Debug.Log("2 gems");
            }
            else if (playerValue <= -922.1 && playerValue >= -967)
            {
                _rewardImage.sprite = _rewardSprites[3];
                reward = 400;
                Debug.Log("4 gems");
            }
            else if (playerValue <= -967.1 && playerValue >= -1012)
            {
                _rewardImage.sprite = _rewardSprites[1];
                reward = 200;
                Debug.Log("2 gems");
            }
            else if (playerValue <= -1012.1 && playerValue >= -1055)
            {
                _rewardImage.sprite = _rewardSprites[0];
                reward = 100;
                Debug.Log("1 gems");
            }

            _rewardCoins = reward;
            _rewardText.text = $"+{reward}";
        }

        public void ClaimReward()
        {
            GameInstance.MoneyManager.AddCoinsCurrency(_rewardCoins);
            _rewardCoins = 0;
            _isInPopup = false;
            _rewardAnimator.SetTrigger("RewardOut");
            GetDailyBonus();
            InitializeLastClaimTime();
            UpdateDailyButtonState();
            UpdateTimer();
        }

        private IEnumerator SpinWheel()
        {
            _wheelButtonInteraction.gameObject.SetActive(false);

            var duration = 2.0f;
            float elapsedTime = 0;

            var startRotation = _wheelPlayablePart.localEulerAngles.z;
            float endRotation = Random.Range(-720, -1055);
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / duration;
                t = t * t * (3f - 2f * t);
                var newRotationZ = Mathf.Lerp(startRotation, endRotation, t);
                _wheelPlayablePart.localEulerAngles = new Vector3(_wheelPlayablePart.localEulerAngles.x,
                    _wheelPlayablePart.localEulerAngles.y, newRotationZ);
                yield return null;
            }

            _wheelPlayablePart.localEulerAngles = new Vector3(_wheelPlayablePart.localEulerAngles.x,
                _wheelPlayablePart.localEulerAngles.y, endRotation);

            ShowRewardPopup(endRotation);
            
            _wheelButtonInteraction.gameObject.SetActive(true);
        }
    }
}