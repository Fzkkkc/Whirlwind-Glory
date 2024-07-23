using GameCore;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button _showAdButton;
    [SerializeField] private Button _showAdButton2;
    [SerializeField] private Button _showAdButton3;
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _androidAdUnitId2 = "Rewarded_Android2";
    [SerializeField] private string _androidAdUnitId3 = "Rewarded_Android3";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
    [SerializeField] private string _iOSAdUnitId2 = "Rewarded_iOS2";
    [SerializeField] private string _iOSAdUnitId3 = "Rewarded_iOS3";
    private string _adUnitId; // This will remain null for unsupported platforms
    private string _adUnitId2;
    private string _adUnitId3;

    private void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
        _adUnitId2 = _iOSAdUnitId2;
        _adUnitId3 = _iOSAdUnitId3;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
        _adUnitId2 = _androidAdUnitId2;
        _adUnitId3 = _androidAdUnitId3;
#endif

        //Disable the button until the ad is ready to show:
        _showAdButton.interactable = false;
        _showAdButton2.interactable = false;
        _showAdButton3.interactable = false;
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        Debug.Log("Loading Ad: " + _adUnitId2);
        Advertisement.Load(_adUnitId2, this);

        Debug.Log("Loading Ad: " + _adUnitId3);
        Advertisement.Load(_adUnitId3, this);
        
        _showAdButton.onClick.AddListener(ShowAd);
        _showAdButton2.onClick.AddListener(ShowAd2);
        _showAdButton3.onClick.AddListener(ShowAd3);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        //// Button 1 ////
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }


        /// ad 2 ////         
        if (adUnitId.Equals(_adUnitId2))
        {
            // Configure the button to call the ShowAd() method when clicked:
            
            // Enable the button for users to click:
            //_showAdButton2.interactable = true;
            _showAdButton2.interactable = true;
        }

        /// ad 3 ////
        ///
        /// 
        
        //if (adUnitId.Equals(_adUnitId3))
        
            // Configure the button to call the ShowAd() method when clicked:
            
        // Enable the button for users to click:
        //_showAdButton3.interactable = true;
    }

    public void OnUnityAdsAdLoaded2(string adUnitId2)
    {
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    public void ShowAd2()
    {
        // Disable the button:
        _showAdButton2.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId2, this);
    }

    public void ShowAd3()
    {
        // Disable the button:
        _showAdButton3.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId3, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            //Wheel Ad
            GameInstance.ShelfMainMainController.IncreasePlayerMoves();
            Debug.Log("you get reward");

            // Load another ad:
            Advertisement.Load(_adUnitId, this);
        }

        if (adUnitId.Equals(_adUnitId2) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            //Daily Ad
            if (GameInstance.ShelfMainMainController._bigShelf.isActiveAndEnabled)
            {
                GameInstance.ShelfMainMainController._bigShelf.RemoveTriosWithSameName();
            }
            else if (GameInstance.ShelfMainMainController._smallShelf.isActiveAndEnabled)
            {
                GameInstance.ShelfMainMainController._smallShelf.RemoveTriosWithSameName();
            }
            Debug.Log("you get reward 2 ");

            // Load another ad:
            Advertisement.Load(_adUnitId2, this);
        }

        if (adUnitId.Equals(_adUnitId3) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            //Weekly Ad
            GameInstance.ShelfMainMainController.ContinueGame();
            Debug.Log("you get reward 3 ");

            // Load another ad:
            Advertisement.Load(_adUnitId3, this);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
    }

    private void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
        _showAdButton2.onClick.RemoveAllListeners();
        _showAdButton3.onClick.RemoveAllListeners();
    }
}