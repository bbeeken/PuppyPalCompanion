using UnityEngine;

public sealed class LikeRainService : MonoBehaviour
{
    public void ShowerLikes(int count = 10)
    {
        for(int i=0; i<count; i++)
        {
            // Trigger a single like particle / effect
            Debug.Log("Like! " + i);
        }
    }
}