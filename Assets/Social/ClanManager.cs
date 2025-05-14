using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages small “clan” groups of players for gifting and cooperative mini-quests.
/// </summary>
public sealed class ClanManager : MonoBehaviour
{
    [Tooltip("Maximum members allowed in one clan.")]
    [SerializeField] private int maxMembers = 20;

    private readonly List<string> members = new List<string>();

    /// <summary>
    /// Adds a member ID to the clan if there’s space.
    /// Returns true on success, false if full or already present.
    /// </summary>
    public bool AddMember(string userId)
    {
        if (members.Contains(userId) || members.Count >= maxMembers)
            return false;

        members.Add(userId);
        EventBus.Publish("clan_member_added", userId);
        return true;
    }

    /// <summary>
    /// Removes a member from the clan.
    /// Returns true if removed, false if not found.
    /// </summary>
    public bool RemoveMember(string userId)
    {
        bool removed = members.Remove(userId);
        if (removed)
            EventBus.Publish("clan_member_removed", userId);
        return removed;
    }

    /// <summary>
    /// Returns a read-only list of current member IDs.
    /// </summary>
    public IReadOnlyList<string> GetMembers() => members.AsReadOnly();

    /// <summary>
    /// Clears all members (e.g., at end of season).
    /// </summary>
    public void ClearClan()
    {
        members.Clear();
        EventBus.Publish("clan_cleared", null);
    }
}
