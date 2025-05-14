using UnityEngine;

public sealed class QRClassroomAuth : MonoBehaviour
{
    [SerializeField] private string validCode;

    public bool ValidateQRCode(string scannedCode)
    {
        return scannedCode == validCode;
    }
}
