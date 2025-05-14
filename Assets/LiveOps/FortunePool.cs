using UnityEngine;

/// <summary>
/// ScriptableObject holding daily fortune texts and reward configuration.
/// </summary>
[CreateAssetMenu(menuName = "PPC/LiveOps/Fortune Pool", fileName = "New FortunePool")]
public sealed class FortunePool : ScriptableObject
{
    [Tooltip("List of fortune messages displayed to the player.")]
    public string[] fortunes;

    [Tooltip("Percentage chance (0–100) to grant a rare accessory.")]
    [Range(0, 100)]
    public int rareGiftChancePercent = 1;

    [Tooltip("Number of coins granted when opening the fortune.")]
    public int coinsOnOpen = 10;
}
