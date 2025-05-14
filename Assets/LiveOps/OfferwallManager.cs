using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Wraps a rewarded-offerwall ad placement.
/// Load on Awake, then ShowWall() triggers display.
/// </summary>
[DisallowMultipleComponent]
public sealed class OfferwallManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Ad Configuration")]
    [Tooltip("Placement ID for the offerwall in Unity Ads dashboard")]
    [SerializeField] private string placementId = "PPC_Offerwall";

    private void Awake()
    {
        Advertisement.Load(placementId, this);
    }

    /// <summary>
    /// Display the offerwall; Unity will handle rewards as configured.
    /// </summary>
    public void ShowWall()
    {
        if (Advertisement.IsReady(placementId))
            Advertisement.Show(placementId, this);
        else
            Debug.LogWarning("OfferwallManager: Offerwall not ready");
    }

    // IUnityAdsLoadListener
    public void OnUnityAdsAdLoaded(string id) { /* no-op */ }
    public void OnUnityAdsFailedToLoad(string id, UnityAdsLoadError error, string message)
        => Debug.LogError($"Offerwall load failed ({id}): {message}");

    // IUnityAdsShowListener
    public void OnUnityAdsShowFailure(string id, UnityAdsShowError error, string message)
        => Debug.LogError($"Offerwall show failed ({id}): {message}");

    public void OnUnityAdsShowStart(string id) { /* no-op */ }
    public void OnUnityAdsShowClick(string id) { /* no-op */ }

    public void OnUnityAdsShowComplete(string id, UnityAdsShowCompletionState state)
    {
        // Reload the offerwall for next display
        Advertisement.Load(placementId, this);
        EventBus.Publish("offerwall_completed", state);
    }
}
