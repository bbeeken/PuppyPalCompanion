using UnityEngine;
using System.IO;

public sealed class DataExportPortal : MonoBehaviour
{
    public void ExportAllData()
    {
        string dir = Application.persistentDataPath;
        string exportPath = Path.Combine(dir, "PPC_Export");
        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);

        File.WriteAllText(Path.Combine(exportPath, "prefs.json"), JsonUtility.ToJson(PlayerPrefsUtility.GetAllPrefs(), true));
        Debug.Log("Data export complete: " + exportPath);
    }
}
