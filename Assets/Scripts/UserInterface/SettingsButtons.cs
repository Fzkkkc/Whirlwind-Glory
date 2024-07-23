using UnityEngine;
using UnityEngine.iOS;

namespace UserInterface
{
    public class SettingsButtons : MonoBehaviour
    {
        [SerializeField] private string _policyString;
        [SerializeField] private string _appURLString;

        public void RateApp()
        {
#if UNITY_IOS
            Device.RequestStoreReview();
#endif
        }

        public void PolicyView()
        {
            Application.OpenURL(_policyString);
        }
        
        public void ShareApp()
        {
            Application.OpenURL(_appURLString);
        }
    }
}