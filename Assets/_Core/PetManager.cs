using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public sealed class PetData
{
    public int hunger = 100, happiness = 100, hygiene = 100;
    public int level = 0, xp = 0, coins = 50;
    public int[] accessories = Array.Empty<int>();
}

[DisallowMultipleComponent]
public sealed class PetManager : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] private StatBar hungerBar;
    [SerializeField] private StatBar happinessBar;
    [SerializeField] private StatBar hygieneBar;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private Animator petAnimator;

    [Header("Config")]
    [Range(5, 120)]
    [SerializeField] private int decayIntervalSec = 60;
    [SerializeField] private int maxStat = 100;
    [SerializeField] private int xpPerTask = 5;
    [SerializeField] private int xpPerLevel = 100;
    [Range(0, 100)]
    [SerializeField] private int rareDropPercent = 5;

    private PetData data = new PetData();
    private float decayTimer;

    private void Awake()
    {
        Load();
        UpdateUI();
    }

    private void Update()
    {
        decayTimer += Time.unscaledDeltaTime;
        if (decayTimer >= decayIntervalSec)
        {
            decayTimer = 0f;
            data.hunger = Mathf.Max(0, data.hunger - 1);
            data.happiness = Mathf.Max(0, data.happiness - 1);
            data.hygiene = Mathf.Max(0, data.hygiene - 1);
            UpdateUI();
            Save();
        }
    }

    public void Feed(int amount = 20) => ModifyStat(ref data.hunger, amount);
    public void Play(int amount = 15) => ModifyStat(ref data.happiness, amount);
    public void Clean(int amount = 25) => ModifyStat(ref data.hygiene, amount);

    private void ModifyStat(ref int stat, int delta)
    {
        stat = Mathf.Clamp(stat + delta, 0, maxStat);
        GrantRewards();
        UpdateUI();
        Save();
    }

    private void GrantRewards()
    {
        data.coins += Random.Range(3, 8);
        data.xp += xpPerTask;
        if (data.xp >= xpPerLevel)
        {
            data.xp -= xpPerLevel;
            data.level++;
            petAnimator.SetInteger("Stage", Mathf.Clamp(data.level / 5, 0, 2));
        }
        if (Random.Range(0, 100) < rareDropPercent)
        {
            var id = "rare_" + Guid.NewGuid().ToString();
            Array.Resize(ref data.accessories, data.accessories.Length + 1);
            data.accessories[^1] = id.GetHashCode();
        }
    }

    private void UpdateUI()
    {
        hungerBar.Set(data.hunger, maxStat);
        happinessBar.Set(data.happiness, maxStat);
        hygieneBar.Set(data.hygiene, maxStat);
        levelText.text = $"Lv {data.level}";
    }

    private void Save()
    {
        var path = Path.Combine(Application.persistentDataPath, "pet_state.json");
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    private void Load()
    {
        var path = Path.Combine(Application.persistentDataPath, "pet_state.json");
        if (File.Exists(path))
            data = JsonUtility.FromJson<PetData>(File.ReadAllText(path));
    }

    public int Coins => data.coins;
    public bool SpendCoins(int amount)
    {
        if (data.coins < amount) return false;
        data.coins -= amount;
        Save();
        return true;
    }
}
