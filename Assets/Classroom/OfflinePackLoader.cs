using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class OfflinePackLoader : MonoBehaviour
{
    [SerializeField] private string offlinePackLabel = "OfflinePack";

    void Awake()
    {
#if PPC_OFFLINE
        LoadOfflineAssets();
#endif
    }

    private async void LoadOfflineAssets()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(offlinePackLabel, null);
        await handle.Task;
        Debug.Log("Offline assets loaded: " + handle.Result.Count + " items");
    }
}
