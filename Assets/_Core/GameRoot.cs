using UnityEngine;

public sealed class GameRoot : MonoBehaviour
{
    void Awake()
    {
        // Lock to 60 FPS for consistent timing and to save battery
        Application.targetFrameRate = 60;
        // Prevent the screen from sleeping while the game is running
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
