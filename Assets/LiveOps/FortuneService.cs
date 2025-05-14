using UnityEngine;

[CreateAssetMenu(menuName = "PPC/FortunePool")]
public sealed class FortunePool : ScriptableObject
{
    [TextArea] public string[] fortunes;
    public int rareGiftChancePercent = 1;
    public int coinsOnOpen = 10;
}

public sealed class FortuneService : MonoBehaviour
{
    [SerializeField] private FortunePool pool;
    [SerializeField] private PetManager pet;
    private string Key => System.DateTime.UtcNow.ToString("yyyyMMdd");

    public bool TryOpen(out string fortune)
    {
        if (PlayerPrefs.HasKey(Key))
        {
            fortune = "Come back tomorrow!";
            return false;
        }

        fortune = pool.fortunes[Random.Range(0, pool.fortunes.Length)];
        pet.AddCoins(pool.coinsOnOpen);
        if (Random.Range(0, 100) < pool.rareGiftChancePercent)
            pet.UnlockAccessory("rare_" + fortune.GetHashCode());

        PlayerPrefs.SetInt(Key, 1);
        return true;
    }
}