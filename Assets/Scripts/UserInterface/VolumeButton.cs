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
        [SerializeField] private AudioSource _bgMusic;

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
                _bgMusic.volume = 0f;
            }
            else
            {
                _isOff = true;
                _volumeButtonImage.sprite = _volumeOnSprite;
                _bgMusic.volume = 1f;
            }
        }
    }
}