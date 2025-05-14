using System.IO;
using System.IO.Compression;
using UnityEngine;

/// <summary>
/// Zips all player-generated data in persistentDataPath into a single archive.
/// </summary>
public sealed class DataExportPortal : MonoBehaviour
{
    [Tooltip("Name of the export ZIP placed in persistentDataPath.")]
    [SerializeField] private string exportFileName = "PPC_UserData.zip";

    /// <summary>
    /// Call this (e.g. via UI button) to package all saved data.
    /// </summary>
    public void ExportAllData()
    {
        string dataDir = Application.persistentDataPath;
        string zipPath = Path.Combine(dataDir, exportFileName);

        // Remove existing archive if present
        if (File.Exists(zipPath))
            File.Delete(zipPath);

        // Create ZIP (includes all files & subfolders)
        try
        {
            ZipFile.CreateFromDirectory(dataDir, zipPath, CompressionLevel.Fastest, includeBaseDirectory: false);
            Debug.Log($"DataExportPortal: Created export at {zipPath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"DataExportPortal: Failed to export data – {ex}");
        }
    }
}
