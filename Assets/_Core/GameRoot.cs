using UnityEngine;

public sealed class GameRoot : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout         = SleepTimeout.NeverSleep;
    }
}
