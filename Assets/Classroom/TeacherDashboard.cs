using UnityEngine;
using System.IO;

public sealed class TeacherDashboard : MonoBehaviour
{
    private const string CsvFileName = "class_progress.csv";

    public void ExportCSV(string csvData)
    {
        string path = Path.Combine(Application.persistentDataPath, CsvFileName);
        File.WriteAllText(path, csvData);
        Debug.Log($"Class progress exported to {path}");
    }
}
