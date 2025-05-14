using System;
using UnityEngine;

/// <summary>
/// Manages “Quiet Mode” periods in Classroom Mode:
/// during configured hours, all audio & notifications are muted.
/// </summary>
[DisallowMultipleComponent]
public sealed class QuietModeManager : MonoBehaviour
{
    [Header("Quiet Mode Settings")]
    [Tooltip("Hour (0–23) to start muting (inclusive).")]
    [SerializeField] private int muteStartHour = 9;
    [Tooltip("Hour (0–23) to end muting (exclusive).")]
    [SerializeField] private int muteEndHour   = 14;
    [Tooltip("Enable or disable Classroom Quiet Mode.")]
    [SerializeField] private bool classroomMode = false;

    private bool isMuted;

    private void Awake()
    {
        // Load persisted setting
        classroomMode = PlayerPrefs.GetInt("ClassroomMode", 0) == 1;
        ApplyMuteState();
    }

    private void Update()
    {
        if (!classroomMode)
        {
            SetMuted(false);
            return;
        }

        int hour = DateTime.Now.Hour;
        bool shouldMute = (muteStartHour <= muteEndHour)
            ? (hour >= muteStartHour && hour < muteEndHour)
            : (hour >= muteStartHour || hour < muteEndHour);

        if (shouldMute != isMuted)
            SetMuted(shouldMute);
    }

    private void SetMuted(bool mute)
    {
        isMuted = mute;
        AudioListener.pause = mute;
        EventBus.Publish("QuietModeChanged", mute);
        Debug.Log($"QuietModeManager: Quiet mode {(mute ? "enabled" : "disabled")}.");
    }

    private void ApplyMuteState()
    {
        // Immediately enforce current state on Awake
        int hour = DateTime.Now.Hour;
        bool withinRange = (muteStartHour <= muteEndHour)
            ? (hour >= muteStartHour && hour < muteEndHour)
            : (hour >= muteStartHour || hour < muteEndHour);

        SetMuted(classroomMode && withinRange);
    }

    /// <summary>
    /// Call this to turn Classroom Mode on or off at runtime.
    /// Persists the choice across sessions.
    /// </summary>
    public void SetClassroomMode(bool enabled)
    {
        classroomMode = enabled;
        PlayerPrefs.SetInt("ClassroomMode", enabled ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"QuietModeManager: Classroom mode set to {enabled}.");
    }
}
