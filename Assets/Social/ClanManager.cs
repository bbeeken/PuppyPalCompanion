using UnityEngine;
using System.Collections.Generic;

public sealed class ClanManager : MonoBehaviour
{
    private readonly List<string> members = new List<string>();

    public void AddMember(string id)
    {
        if (!members.Contains(id))
            members.Add(id);
    }

    public void RemoveMember(string id)
    {
        members.Remove(id);
    }

    public IEnumerable<string> GetMembers() => members;
}