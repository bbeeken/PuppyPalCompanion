using System;
using UnityEngine;

/// <summary>
/// Validates scanned QR codes against a stored classroom code.
/// Designed for Classroom Mode to authenticate teacher or student sessions.
/// </summary>
[DisallowMultipleComponent]
public sealed class QRClassroomAuth : MonoBehaviour
{
    [Header("QR Classroom Authentication")]
    [Tooltip("Persistent key for storing the valid classroom QR code in PlayerPrefs.")]
    [SerializeField] private string prefsKey = "ClassroomQRCode";

    [Tooltip("Default classroom code if none has been saved.")]
    [SerializeField] private string defaultCode = "CLASSROOM123";

    private string classroomCode;

    private void Awake()
    {
        // Load the saved classroom code or initialize to default
        classroomCode = PlayerPrefs.GetString(prefsKey, defaultCode);
        Debug.Log($"QRClassroomAuth: Loaded classroom code '{classroomCode}'");
    }

    /// <summary>
    /// Validates the scanned QR code string.
    /// Returns true if it matches the stored classroom code (case-insensitive).
    /// </summary>
    /// <param name="scannedCode">Decoded string from QR scanner.</param>
    public bool ValidateQRCode(string scannedCode)
    {
        if (string.IsNullOrWhiteSpace(scannedCode))
        {
            Debug.LogWarning("QRClassroomAuth: Scanned QR code is empty or null.");
            return false;
        }

        bool isValid = string.Equals(
            scannedCode.Trim(),
            classroomCode,
            StringComparison.OrdinalIgnoreCase
        );

        Debug.Log(
            $"QRClassroomAuth: Validation {(isValid ? "succeeded" : "failed")} " +
            $"(scanned: '{scannedCode.Trim()}', expected: '{classroomCode}')"
        );

        return isValid;
    }

    /// <summary>
    /// Updates the stored classroom code at runtime.
    /// Persists the new code in PlayerPrefs.
    /// </summary>
    /// <param name="newCode">The new QR code string to accept.</param>
    public void SetClassroomCode(string newCode)
    {
        if (string.IsNullOrWhiteSpace(newCode))
        {
            Debug.LogError("QRClassroomAuth: Attempted to set an empty or null classroom code.");
            return;
        }

        classroomCode = newCode.Trim();
        PlayerPrefs.SetString(prefsKey, classroomCode);
        PlayerPrefs.Save();

        Debug.Log($"QRClassroomAuth: Classroom code updated to '{classroomCode}'");
    }

    /// <summary>
    /// Returns the currently stored classroom code.
    /// </summary>
    public string GetCurrentCode() => classroomCode;
}
