using System;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Exports classroom progress data as CSV to persistentDataPath.
/// </summary>
[DisallowMultipleComponent]
public sealed class TeacherDashboard : MonoBehaviour
{
    [Tooltip("Filename (including .csv) for the exported progress report.")]
    [SerializeField] private string csvFileName = "class_progress.csv";

    /// <summary>
    /// Writes the given 2D string array to a CSV file.
    /// Each sub-array represents one row.
    /// </summary>
    /// <param name="rows">Array of rows; each row is an array of string fields.</param>
    public void ExportCSV(string[][] rows)
    {
        if (rows == null || rows.Length == 0)
        {
            Debug.LogWarning("TeacherDashboard: No data provided for export.");
            return;
        }

        try
        {
            var sb = new StringBuilder();
            foreach (var row in rows)
            {
                sb.AppendLine(string.Join(",", EscapeFields(row)));
            }

            string path = Path.Combine(Application.persistentDataPath, csvFileName);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            Debug.Log($"TeacherDashboard: CSV exported to {path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"TeacherDashboard: Failed to export CSV – {ex}");
        }
    }

    /// <summary>
    /// Escapes any fields containing commas, quotes, or newlines.
    /// </summary>
    private string[] EscapeFields(string[] fields)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            var f = fields[i] ?? "";
            if (f.Contains(",") || f.Contains("\"") || f.Contains("\n"))
            {
                fields[i] = $"\"{f.Replace("\"", "\"\"")}\"";
            }
        }
        return fields;
    }
}
