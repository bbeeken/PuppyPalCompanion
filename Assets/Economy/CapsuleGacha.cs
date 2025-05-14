using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "PPC/CapsuleTable")]
public sealed class CapsuleTable : ScriptableObject
{
    public string[] commonIDs;
    public string[] rareIDs;
    [Range(0,100)] public int rareWeight = 5;
}

public sealed class CapsuleGacha : MonoBehaviour
{
    [SerializeField] private CapsuleTable table;
    [SerializeField] private PetManager pet;

    public string Roll()
    {
        bool rare = Random.Range(0, 100) < table.rareWeight;
        string id = rare
            ? table.rareIDs[Random.Range(0, table.rareIDs.Length)]
            : table.commonIDs[Random.Range(0, table.commonIDs.Length)];

        pet.UnlockAccessory(id);
        return id;
    }
}