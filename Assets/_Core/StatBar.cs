using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class StatBar : MonoBehaviour
{
    [Tooltip("Image whose fillAmount reflects the stat.")]
    [SerializeField] private Image fillImg;

    /// <summary>
    /// Sets the normalized fill based on current and maximum values.
    /// </summary>
    public void Set(int value, int max)
    {
        fillImg.fillAmount = Mathf.Clamp01((float)value / max);
    }
}
