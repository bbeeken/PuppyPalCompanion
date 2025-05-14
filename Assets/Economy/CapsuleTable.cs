using UnityEngine;

[CreateAssetMenu(menuName = "PPC/Capsule Table", fileName = "NewCapsuleTable")]
public sealed class CapsuleTable : ScriptableObject
{
    [Tooltip("IDs of common accessories")]
    public string[] commonIDs = new string[0];
    [Tooltip("IDs of rare accessories")]
    public string[] rareIDs   = new string[0];
    [Range(0,100), Tooltip("Chance (0–100%) to drop a rare capsule")]
    public int rareWeight     = 5;
}
