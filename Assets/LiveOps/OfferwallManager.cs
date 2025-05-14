using UnityEngine;
using UnityEngine.Advertisements;

public sealed class OfferwallManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
#if UNITY_ANDROID
    private const string WALL_ID = "PPC_Offerwall";
#endif

    private void Awake()
    {
        Advertisement.Load(WALL_ID, this);
    }

    public void ShowWall()
    {
        if (Advertisement.IsReady(WALL_ID))
            Advertisement.Show(WALL_ID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId) { }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        Advertisement.Load(placementId, this);
    }
}