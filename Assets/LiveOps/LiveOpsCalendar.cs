using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;

public sealed class LiveOpsCalendar : MonoBehaviour
{
    [SerializeField] private string collection = "liveops";
    private FirebaseFirestore db;
    private float lastPull;

    private async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        await Pull();
        InvokeRepeating(nameof(Tick), 60f, 3600f);
    }

    private async void Tick()
    {
        if (Time.realtimeSinceStartup - lastPull < 3600f) return;
        await Pull();
    }

    private async Task Pull()
    {
        lastPull = Time.realtimeSinceStartup;
        var snap = await db.Collection(collection).GetSnapshotAsync();
        foreach (var doc in snap.Documents)
            EventBus.Publish("liveops_event", doc.ToDictionary());
    }
}