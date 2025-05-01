using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

[RequireComponent(typeof(Button))]
public class ScreenshotHandler : MonoBehaviour
{
    public AudioClip cameraSound;
    public Image flashImage;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = cameraSound;

        GetComponent<Button>().onClick.AddListener(() => StartCoroutine(CaptureScreenshot()));
    }

    private IEnumerator CaptureScreenshot()
    {
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true);
            flashImage.color = new Color(1, 1, 1, 0.8f);
        }

        if (cameraSound != null)
        {
            audioSource.Play();
        }

        yield return new WaitForEndOfFrame();

        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(fullPath);
        Debug.Log("Screenshot saved to: " + fullPath);

        string galleryFolder = "/storage/emulated/0/Pictures";
        if (!Directory.Exists(galleryFolder))
        {
            Directory.CreateDirectory(galleryFolder);
        }

        string galleryPath = Path.Combine(galleryFolder, fileName);
        File.Move(fullPath, galleryPath);

        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                using (AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
                {
                    mediaScanner.CallStatic("scanFile", context, new string[] { galleryPath }, null, null);
                }
            }
        }

        Debug.Log("Screenshot moved to gallery: " + galleryPath);

        yield return new WaitForSeconds(0.3f);
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(false);
        }
    }
}
