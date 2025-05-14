using UnityEngine;

public sealed class StreakService : MonoBehaviour
{
    private const string KEY = "daily_streak";
    public int StreakCount => PlayerPrefs.GetInt(KEY, 0);

    public void Increment()
    {
        int count = StreakCount + 1;
        PlayerPrefs.SetInt(KEY, count);
    }

    public void Reset()
    {
        PlayerPrefs.SetInt(KEY, 0);
    }
}