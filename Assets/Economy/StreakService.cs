using UnityEngine;
using System;

/// <summary>
/// Tracks daily play streaks and provides a grace freeze token.
/// </summary>
public sealed class StreakService : MonoBehaviour
{
    private const string KEY_STREAK_DATE  = "Streak_LastDate";
    private const string KEY_STREAK_COUNT = "Streak_Count";
    private const string KEY_GRACE_TOKEN  = "Streak_GraceToken";

    /// <summary>
    /// Current consecutive-day streak count.
    /// </summary>
    public int StreakCount => PlayerPrefs.GetInt(KEY_STREAK_COUNT, 0);

    /// <summary>
    /// Whether a grace freeze token is available.
    /// </summary>
    public bool HasGraceToken => PlayerPrefs.GetInt(KEY_GRACE_TOKEN, 0) > 0;

    private void Awake()
    {
        CheckReset();
    }

    /// <summary>
    /// Call at end of a play session to bump streak if new day.
    /// </summary>
    public void EndSession()
    {
        var last = PlayerPrefs.GetString(KEY_STREAK_DATE, "");
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        if (last == today) return;

        // New day: determine gap
        if (DateTime.TryParse(last, out var prevDate) && (DateTime.UtcNow - prevDate).TotalDays <= 1)
        {
            PlayerPrefs.SetInt(KEY_STREAK_COUNT, StreakCount + 1);
        }
        else
        {
            PlayerPrefs.SetInt(KEY_STREAK_COUNT, 1);
        }

        PlayerPrefs.SetString(KEY_STREAK_DATE, today);
        PlayerPrefs.Save();
        EventBus.Publish("streak_updated", StreakCount);
    }

    /// <summary>
    /// Use a grace token to freeze streak for one missed day.
    /// </summary>
    public bool UseGraceToken()
    {
        if (!HasGraceToken) return false;
        PlayerPrefs.SetInt(KEY_GRACE_TOKEN, 0);
        PlayerPrefs.Save();
        EventBus.Publish("streak_frozen", StreakCount);
        return true;
    }

    /// <summary>
    /// Award a grace token (e.g. via rewarded ad).
    /// </summary>
    public void GrantGraceToken()
    {
        PlayerPrefs.SetInt(KEY_GRACE_TOKEN, 1);
        PlayerPrefs.Save();
        EventBus.Publish("grace_token_awarded", StreakCount);
    }

    private void CheckReset()
    {
        var last = PlayerPrefs.GetString(KEY_STREAK_DATE, "");
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        if (last != today)
            EndSession();
    }
}
