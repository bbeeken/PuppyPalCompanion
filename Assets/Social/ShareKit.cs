using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// Simplifies sharing text and images to native social platforms.
/// On mobile it invokes native share dialogs; in editor it logs the action.
/// </summary>
public sealed class ShareKit : MonoBehaviour
{
    [Tooltip("Optional overlay message shown before share.")]
    [SerializeField] private string shareMessage = "Check out my Puppy Pal!";

    /// <summary>
    /// Shares a screenshot to installed social apps.
    /// </summary>
    public void ShareScreenshot()
    {
        StartCoroutine(CaptureAndShare());
    }

    private System.Collections.IEnumerator CaptureAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        byte[] imageData = screenImage.EncodeToPNG();
        string path = System.IO.Path.Combine(Application.temporaryCachePath, "share.png");
        System.IO.File.WriteAllBytes(path, imageData);

#if UNITY_ANDROID
        using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
        using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_SEND")))
        {
            intent.Call<AndroidJavaObject>("setType", "image/png");
            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);
            AndroidJavaObject file = new AndroidJavaObject("java.io.File", path);
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", file);
            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uri);
            intent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("startActivity", intent);
        }
#elif UNITY_IOS
        ShareiOS(path, shareMessage);
#else
        Debug.Log($"ShareKit: Would share '{path}' with message '{shareMessage}'");
#endif

        yield break;
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void ShareiOS(string path, string message);
#endif
}
