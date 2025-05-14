using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and retrieves asynchronous “ghost” race times for friend-race mini-games.
/// </summary>
public sealed class GhostRaceService : MonoBehaviour
{
    private readonly Dictionary<string, float> personalBests = new Dictionary<string, float>();

    /// <summary>
    /// Submit a completed time for a given friend ID.
    /// If it’s faster than their previous best, updates it.
    /// </summary>
    public void SubmitTime(string friendId, float timeSeconds)
    {
        if (!personalBests.ContainsKey(friendId) || timeSeconds < personalBests[friendId])
        {
            personalBests[friendId] = timeSeconds;
            EventBus.Publish("ghost_time_updated", new { friendId, time = timeSeconds });
        }
    }

    /// <summary>
    /// Retrieves the stored best time for a friend ID, or Mathf.Infinity if none.
    /// </summary>
    public float GetGhostTime(string friendId)
    {
        return personalBests.TryGetValue(friendId, out var t) ? t : Mathf.Infinity;
    }

    /// <summary>
    /// Returns all stored ghost entries as friendId→time map.
    /// </summary>
    public IReadOnlyDictionary<string, float> GetAllGhostTimes() => personalBests;
}
