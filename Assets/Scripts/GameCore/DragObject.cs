using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace GameCore
{
    public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 _offset;
        private Camera _mainCamera;

        private bool _spotted = false;
        private Vector3 _startPosition;
        private Coroutine _moveCoroutine;
        private Transform _currentSpot;

        [SerializeField] private int _index;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            _startPosition = transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(eventData.position);
            _offset = transform.position - worldPoint;
            _offset.z = 0;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(eventData.position);
            transform.position = worldPoint + _offset;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Spot") && hit.TryGetComponent(out SpotForObject spotForObject))
                {
                    if (!spotForObject.HasChildren)
                    {
                        _spotted = true;
                        _currentSpot = hit.transform;
                        break;
                    }
                    
                }
            }

            if (_spotted && _currentSpot != null)
            {
                _startPosition = transform.position = _currentSpot.position;
                transform.SetParent(_currentSpot);
                GameInstance.Audio.PlayPlayerRightChoice();
                GameInstance.FXController.PlayRightChoiceFX(_index,transform.position);
            }
            else
            {
                GameInstance.Audio.PlayPlayerMissChoice();
                if (_moveCoroutine != null)
                {
                    StopCoroutine(_moveCoroutine);
                }
                _moveCoroutine = StartCoroutine(SmoothMove(transform.position, _startPosition, 0.5f));
            }

            _spotted = false;
            _currentSpot = null;
            GameInstance.ShelfMainMainController.OnPlayersMove?.Invoke();
        }

        private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = end;
            transform.rotation = Quaternion.identity;
        }
    }
}
