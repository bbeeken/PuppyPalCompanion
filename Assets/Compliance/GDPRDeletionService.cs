using UnityEngine;
using System.IO;

public sealed class GDPRDeletionService : MonoBehaviour
{
    public void DeleteAllUserData()
    {
        PlayerPrefs.DeleteAll();
        string dir = Application.persistentDataPath;
        foreach (var file in Directory.GetFiles(dir))
            File.Delete(file);
        Debug.Log("All user data deleted per GDPR request.");
    }
}
