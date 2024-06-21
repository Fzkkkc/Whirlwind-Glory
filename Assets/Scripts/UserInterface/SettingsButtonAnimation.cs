using UnityEngine;

namespace UserInterface
{
    public class SettingsButtonAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private bool _isSettingsOpened = false;
        
        public void PlaySettingsButtonAnimation()
        {
            if (!_isSettingsOpened)
            {
                _isSettingsOpened = true;
                _animator.SetTrigger("ButtonIn");
            }
            else if (_isSettingsOpened)
            {
                _isSettingsOpened = false;
                _animator.SetTrigger("ButtonOut");
            }
        }
    }
}
