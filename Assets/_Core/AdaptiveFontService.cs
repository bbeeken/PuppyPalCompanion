using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public sealed class AdaptiveFontService : MonoBehaviour
{
    [Tooltip("All TextMeshProUGUI components to be auto-scaled.")]
    [SerializeField] private TMP_Text[] allText;
    [Tooltip("Target reading grade level (1–6).")]
    [SerializeField] private int gradeLevel = 3;

    private void Awake() => Apply(gradeLevel);

    /// <summary>
    /// Scales font sizes: lower grades get larger text.
    /// </summary>
    public void Apply(int newGrade)
    {
        gradeLevel = Mathf.Clamp(newGrade, 1, 6);
        float multiplier = 1f + 0.05f * (3 - gradeLevel);  // +10% for grade 1
        foreach (var t in allText)
            t.fontSize = t.fontSizeBase * multiplier;
    }
}
