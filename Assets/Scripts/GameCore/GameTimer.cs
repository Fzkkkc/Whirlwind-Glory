using System.Collections;
using TMPro;
using UnityEngine;

namespace GameCore
{
    public class GameTimer : MonoBehaviour
    {
        private Coroutine _timerCoroutine;
        private bool _isTimerRunning = false;
        private float _elapsedTime = 0f;
        
        [SerializeField] private TextMeshProUGUI _timerText;

        public void StartTimer()
        {
            if (_isTimerRunning) return;
            _isTimerRunning = true;
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public void StopTimer()
        {
            if (!_isTimerRunning) return;
            _isTimerRunning = false;
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }
        
        private IEnumerator TimerCoroutine()
        {
            _elapsedTime = 0f;

            while (_isTimerRunning)
            {
                _elapsedTime += Time.deltaTime;
                UpdateTimerUI();
                yield return null;
            }
        }

        private void UpdateTimerUI()
        {
            var minutes = Mathf.FloorToInt(_elapsedTime / 60f);
            var seconds = Mathf.FloorToInt(_elapsedTime % 60f);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}