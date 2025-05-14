using UnityEngine;
using System;

/// <summary>
/// Handles gacha-style capsule drops.  
/// Use Roll() to perform a spin; subscribes to EventBus for analytics.
/// </summary>
[RequireComponent(typeof(Animator))]
public sealed class CapsuleGacha : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField, Tooltip("Lookup table for drop pools")]
    private CapsuleTable table;

    [Header("References")]
    [SerializeField, Tooltip("Animator with 'Open' trigger for capsule animation")]
    private Animator animator;

    [SerializeField, Tooltip("PetManager to unlock accessories")]
    private PetManager pet;

    /// <summary>
    /// Performs a spin, returns the unlocked ID.
    /// </summary>
    public string Roll()
    {
        if (table == null || animator == null || pet == null)
        {
            Debug.LogError("CapsuleGacha: Missing references");
            return null;
        }

        bool isRare = UnityEngine.Random.Range(0, 100) < table.rareWeight;
        var pool = isRare ? table.rareIDs : table.commonIDs;
        if (pool == null || pool.Length == 0)
        {
            Debug.LogWarning("CapsuleGacha: Pool empty");
            return null;
        }

        string id = pool[UnityEngine.Random.Range(0, pool.Length)];
        pet.UnlockAccessory(id);

        // Fire animation and analytics event
        animator.SetTrigger("Open");
        EventBus.Publish("capsule_dropped", new { id, isRare });

        return id;
    }
}
