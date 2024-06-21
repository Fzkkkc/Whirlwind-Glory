using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class VolumeButton : MonoBehaviour
    {
        [SerializeField] private Sprite _volumeOnSprite;
        [SerializeField] private Sprite _volumeOffSprite;
        [SerializeField] private Image _volumeButtonImage;

        private bool _isOff = false;
        
        private void OnValidate()
        {
            _volumeOnSprite ??= GetComponent<Image>().sprite;
            _volumeButtonImage ??= GetComponent<Image>();
        }

        public void ChangeSpriteButton()
        {
            if (_isOff)
            {
                _isOff = false;
                _volumeButtonImage.sprite = _volumeOffSprite;
            }
            else
            {
                _isOff = true;
                _volumeButtonImage.sprite = _volumeOnSprite;
            }
        }
    }
}