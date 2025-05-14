using System.IO;
using UnityEngine;

/// <summary>
/// Completely removes all user data in compliance with a deletion request.
/// </summary>
public sealed class GDPRDeletionService : MonoBehaviour
{
    /// <summary>
    /// Deletes PlayerPrefs and all files/folders under persistentDataPath.
/// Use with extreme caution; this is irreversible.
/// </summary>
    public void DeleteAllUserData()
    {
        // 1. Clear Unity PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2. Delete all files & folders in persistentDataPath
        string dataDir = Application.persistentDataPath;
        try
        {
            foreach (string file in Directory.GetFiles(dataDir))
                File.Delete(file);

            foreach (string dir in Directory.GetDirectories(dataDir))
                Directory.Delete(dir, recursive: true);

            Debug.Log("GDPRDeletionService: All user data deleted.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"GDPRDeletionService: Error deleting data – {ex}");
        }
    }
}
