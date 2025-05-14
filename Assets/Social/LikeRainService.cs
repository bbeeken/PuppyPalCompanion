using UnityEngine;

public sealed class LikeRainService : MonoBehaviour
{
    public void ShowerLikes(int count = 10)
    {
        for(int i=0; i<count; i++)
        {
            // Trigger a single like particle / effect
            Debug.Log("Like! " + i);
        }
    }
}using UnityEngine;

/// <summary>
/// Spawns a brief “rain” of like-particles or UI icons for social feedback.
/// </summary>
public sealed class LikeRainService : MonoBehaviour
{
    [Tooltip("Prefab to spawn for each like (e.g., a heart sprite).")]
    [SerializeField] private GameObject likePrefab;

    [Tooltip("Parent transform under which like prefabs are spawned.")]
    [SerializeField] private Transform spawnParent;

    [Tooltip("Number of likes to spawn in one shower.")]
    [SerializeField] private int spawnCount = 10;

    [Tooltip("Delay between each spawn (seconds).")]
    [SerializeField] private float spawnInterval = 0.05f;

    /// <summary>
    /// Starts a shower of like prefabs.
    /// </summary>
    public void ShowerLikes()
    {
        StartCoroutine(SpawnLikes());
    }

    private System.Collections.IEnumerator SpawnLikes()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var obj = Instantiate(likePrefab, spawnParent);
            // Optionally randomize position/animation here
            Destroy(obj, 2f); // auto-cleanup after 2 seconds
            yield return new WaitForSeconds(spawnInterval);
        }
        EventBus.Publish("likes_showered", spawnCount);
    }
}
