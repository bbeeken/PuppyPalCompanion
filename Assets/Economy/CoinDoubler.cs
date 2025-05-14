using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Presents a rewarded ad to double the player's current coins.
/// </summary>
public sealed class CoinDoubler : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Ads Settings")]
    [SerializeField, Tooltip("Placement ID for rewarded ad")]
    private string rewardedPlacementId = "Rewarded_Android";

    [Header("References")]
    [SerializeField, Tooltip("PetManager to apply coin doubling")]
    private PetManager pet;

    private System.Action onReward;

    private void Awake()
    {
        // Load the rewarded ad early
        Advertisement.Load(rewardedPlacementId, this);
    }

    /// <summary>
    /// Call to show a rewarded ad; on completion, doubles coins.
    /// </summary>
    public void ShowDoubleCoinsAd()
    {
        if (!Advertisement.IsReady(rewardedPlacementId))
        {
            Debug.LogWarning("CoinDoubler: Ad not ready");
            return;
        }

        onReward = () =>
        {
            pet.AddCoins(pet.Coins);
            EventBus.Publish("coins_doubled", pet.Coins);
        };
        Advertisement.Show(rewardedPlacementId, this);
    }

    // IUnityAdsLoadListener
    public void OnUnityAdsAdLoaded(string placementId) { }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        => Debug.LogError($"CoinDoubler: Failed to load ad {placementId} – {message}");

    // IUnityAdsShowListener
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        => Debug.LogError($"CoinDoubler: Ad show failure {placementId} – {message}");

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        if (placementId == rewardedPlacementId && state == UnityAdsShowCompletionState.COMPLETED)
            onReward?.Invoke();
        // Reload for next time
        Advertisement.Load(rewardedPlacementId, this);
    }
}
