using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class StatBar : MonoBehaviour
{
    [SerializeField] private Image fillImg;

    public void Set(int value, int max)
    {
        fillImg.fillAmount = Mathf.Clamp01((float)value / max);
    }
}
