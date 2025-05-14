using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public sealed class PetData
{
    public int hunger = 100, happiness = 100, hygiene = 100;
    public int level = 0, xp = 0, coins = 50;
    public List<string> accessories = new List<string>();
}

[DisallowMultipleComponent]
public sealed class PetManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private StatBar hungerBar;
    [SerializeField] private StatBar happinessBar;
    [SerializeField] private StatBar hygieneBar;
    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private Animator petAnimator;

    [Header("Config")]
    [SerializeField, Range(5, 120)] private int decayIntervalSec = 60;
    [SerializeField] private int maxStat = 100;
    [SerializeField] private int xpPerTask = 5;
    [SerializeField] private int xpPerLevel = 100;
    [SerializeField, Range(0,100)] private int rareDropPercent = 5;

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

    // Public methods for UI buttons
    public void Feed(int amount = 20)  => ModifyStat(ref data.hunger,  amount);
    public void Play(int amount = 15)  => ModifyStat(ref data.happiness, amount);
    public void Clean(int amount = 25) => ModifyStat(ref data.hygiene,   amount);

    private void ModifyStat(ref int stat, int delta)
    {
        stat = Mathf.Clamp(stat + delta, 0, maxStat);
        GrantRewards();
        UpdateUI();
        Save();
    }

    private void GrantRewards()
    {
        // Random coin bonus
        data.coins += Random.Range(3, 8);
        // XP and leveling
        data.xp += xpPerTask;
        if (data.xp >= xpPerLevel)
        {
            data.xp -= xpPerLevel;
            data.level++;
            petAnimator.SetInteger("Stage", Mathf.Clamp(data.level / 5, 0, 2));
        }
        // Occasional rare accessory
        if (Random.Range(0, 100) < rareDropPercent)
        {
            string id = $"rare_{Guid.NewGuid():N}";
            data.accessories.Add(id);
            EventBus.Publish("accessory_unlocked", id);
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
        string path = Path.Combine(Application.persistentDataPath, "pet_state.json");
        File.WriteAllText(path, JsonUtility.ToJson(data), System.Text.Encoding.UTF8);
    }

    private void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "pet_state.json");
        if (File.Exists(path))
            data = JsonUtility.FromJson<PetData>(File.ReadAllText(path));
    }
}
