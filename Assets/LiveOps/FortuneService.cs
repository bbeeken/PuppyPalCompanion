using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages daily fortune openings: ensures only one per calendar day,
/// grants coins and occasional rare accessories.
/// </summary>
[DisallowMultipleComponent]
public sealed class FortuneService : MonoBehaviour
{
    [Header("Pool & Rewards")]
    [SerializeField] private FortunePool pool;
    [SerializeField] private PetManager pet;

    private string lastOpenKey => $"Fortune_{DateTime.UtcNow:yyyyMMdd}";

    /// <summary>
    /// Attempts to open today's fortune.
    /// Returns (success, message).
    /// </summary>
    public (bool success, string message) TryOpenFortune()
    {
        // Prevent multiple opens per UTC day
        if (PlayerPrefs.HasKey(lastOpenKey))
            return (false, "Come back tomorrow for your next fortune!");

        // Choose a random fortune message
        var message = pool.fortunes.Length > 0
            ? pool.fortunes[Random.Range(0, pool.fortunes.Length)]
            : "Have a great day!";

        // Grant coin reward
        pet.AddCoins(pool.coinsOnOpen);

        // Possibly grant a rare accessory
        if (Random.Range(0, 100) < pool.rareGiftChancePercent)
        {
            string accessoryId = $"rare_fortune_{Guid.NewGuid():N}";
            pet.UnlockAccessory(accessoryId);
            EventBus.Publish("fortune_rare_awarded", accessoryId);
        }

        // Mark as opened for today
        PlayerPrefs.SetInt(lastOpenKey, 1);
        PlayerPrefs.Save();

        // Publish analytics event
        EventBus.Publish("fortune_opened", message);

        return (true, message);
    }
}
