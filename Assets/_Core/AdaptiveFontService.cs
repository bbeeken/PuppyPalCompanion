using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public sealed class AdaptiveFontService : MonoBehaviour
{
    [SerializeField] private TMP_Text[] allText;
    [SerializeField] private int gradeLevel = 3;

    private void Awake() => Apply(gradeLevel);

    public void Apply(int newGrade)
    {
        gradeLevel = Mathf.Clamp(newGrade, 1, 6);
        float multiplier = 1f + 0.05f * (3 - gradeLevel);
        foreach (var t in allText)
            t.fontSize = t.fontSizeBase * multiplier;
    }
}
