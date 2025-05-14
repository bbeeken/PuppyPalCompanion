using UnityEngine;
using System.Collections.Generic;

public sealed class GhostRaceService : MonoBehaviour
{
    private Dictionary<string, float> personalBests = new Dictionary<string, float>();

    public void SubmitTime(string userId, float time)
    {
        if (!personalBests.ContainsKey(userId) || time < personalBests[userId])
            personalBests[userId] = time;
    }

    public float GetGhostTime(string userId)
    {
        return personalBests.TryGetValue(userId, out var t) ? t : float.MaxValue;
    }
}