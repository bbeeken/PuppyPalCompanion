using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

/// <summary>
/// Fetches live-ops events from Firestore on a schedule,
/// with local fallback to a default JSON.
/// Publishes each event via the EventBus under "liveops_event".
/// </summary>
[DisallowMultipleComponent]
public sealed class LiveOpsCalendar : MonoBehaviour
{
    [Tooltip("Firestore collection name for live-ops events")]
    [SerializeField] private string collectionName = "liveops";

    [Tooltip("Default JSON file in Resources for offline fallback")]
    [SerializeField] private string defaultJsonResource = "liveops_default";

    [Tooltip("Initial delay (seconds) before first fetch")]
    [SerializeField] private float initialDelay = 2f;

    [Tooltip("Interval (seconds) between fetches")]
    [SerializeField] private float fetchInterval = 3600f;

    private FirebaseFirestore db;
    private bool initialized;

    private async void Awake()
    {
        // Initialize Firestore
        try
        {
            db = FirebaseFirestore.DefaultInstance;
            initialized = true;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"LiveOpsCalendar: Firestore init failed – {ex.Message}");
            initialized = false;
        }

        await Task.Delay(TimeSpan.FromSeconds(initialDelay));
        await FetchAndPublishAsync();

        // Schedule recurring fetches
        InvokeRepeating(nameof(ScheduledFetch), fetchInterval, fetchInterval);
    }

    private void ScheduledFetch() => _ = FetchAndPublishAsync();

    private async Task FetchAndPublishAsync()
    {
        List<Dictionary<string, object>> events = null;

        if (initialized)
        {
            try
            {
                var snapshot = await db.Collection(collectionName).GetSnapshotAsync();
                events = new List<Dictionary<string, object>>(snapshot.Count);
                foreach (var doc in snapshot.Documents)
                    events.Add(doc.ToDictionary());
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"LiveOpsCalendar: Firestore fetch failed – {ex.Message}");
                initialized = false;
            }
        }

        if (!initialized || events == null || events.Count == 0)
        {
            // Fallback to default JSON in Resources
            var text = Resources.Load<TextAsset>(defaultJsonResource);
            if (text != null)
            {
                try
                {
                    events = JsonUtility.FromJson<LiveOpsContainer>(text.text).events;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"LiveOpsCalendar: Failed to parse default JSON – {ex.Message}");
                    return;
                }
            }
            else
            {
                Debug.LogError("LiveOpsCalendar: Default JSON resource not found");
                return;
            }
        }

        // Publish each event
        foreach (var evt in events)
            EventBus.Publish("liveops_event", evt);
    }

    [Serializable]
    private class LiveOpsContainer
    {
        public List<Dictionary<string, object>> events;
    }
}
