using UnityEngine;

public sealed class QuietModeManager : MonoBehaviour
{
    [SerializeField] private int muteStartHour = 9;
    [SerializeField] private int muteEndHour = 14;
    [SerializeField] private bool classroomMode = false;

    void Update()
    {
        if (!classroomMode) return;
        int hour = System.DateTime.Now.Hour;
        bool mute = hour >= muteStartHour && hour < muteEndHour;
        AudioListener.pause = mute;
        // Notify other systems
        EventBus.Publish("quiet_mode", mute);
    }

    public void SetClassroomMode(bool enabled) => classroomMode = enabled;
}
