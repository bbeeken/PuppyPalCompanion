using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Loads all assets labeled “OfflinePack” via Addressables when running in offline/classroom mode.
/// Automatically releases them on destroy to free memory.
/// </summary>
[DisallowMultipleComponent]
public sealed class OfflinePackLoader : MonoBehaviour
{
    [Header("Offline Pack Settings")]
    [Tooltip("Addressables label used to tag the offline pack assets.")]
    [SerializeField] private string offlinePackLabel = "OfflinePack";

    // Tracks all loaded handles so we can release them later
    private readonly List<AsyncOperationHandle> _loadedHandles = new List<AsyncOperationHandle>();

    private void Awake()
    {
        // Only load offline assets when the PPC_OFFLINE define is set (e.g. Classroom builds)
#if PPC_OFFLINE
        LoadOfflineAssets();
#endif
    }

    private void OnDestroy()
    {
        // Release each handle to free Addressables memory
        foreach (var handle in _loadedHandles)
        {
            Addressables.Release(handle);
        }
        _loadedHandles.Clear();
    }

    /// <summary>
    /// Begins asynchronous loading of all assets tagged with the offline label.
    /// </summary>
    private void LoadOfflineAssets()
    {
        Debug.Log($"OfflinePackLoader: Starting load of assets with label '{offlinePackLabel}'");

        var handle = Addressables.LoadAssetsAsync<GameObject>(
            offlinePackLabel,
            onAssetLoaded: (obj) => { /* Optionally initialize the loaded prefab here */ },
            mergeMode: Addressables.MergeMode.Union
        );

        handle.Completed += OnAssetsLoaded;
        _loadedHandles.Add(handle);
    }

    /// <summary>
    /// Callback invoked when the Addressables load operation completes.
    /// </summary>
    private void OnAssetsLoaded(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"OfflinePackLoader: Successfully loaded {handle.Result.Count} offline assets.");
            // Optionally instantiate or register loaded assets here
        }
        else
        {
            Debug.LogError($"OfflinePackLoader: Failed to load offline assets (Status: {handle.Status}).");
        }
    }
}
