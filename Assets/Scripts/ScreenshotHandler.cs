using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

[RequireComponent(typeof(Button))]
public class ScreenshotHandler : MonoBehaviour
{
    public AudioClip cameraSound;
    public Image flashImage; // UI Image putih full screen

    private AudioSource audioSource;

    private void Start()
    {
        // Setup audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = cameraSound;

        // Tambahkan listener ke tombol ini sendiri
        GetComponent<Button>().onClick.AddListener(() => StartCoroutine(CaptureScreenshot()));
    }

    private IEnumerator CaptureScreenshot()
    {
        // Efek kilat
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true);
            flashImage.color = new Color(1, 1, 1, 0.8f);
        }

        // Suara kamera
        if (cameraSound != null)
        {
            audioSource.Play();
        }

        yield return new WaitForEndOfFrame();

        // Simpan screenshot
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(folderPath, fileName);

        // Simpan screenshot di folder aplikasi
        ScreenCapture.CaptureScreenshot(fullPath);
        Debug.Log("Screenshot saved to: " + fullPath);

        // Setelah screenshot disimpan, pindahkan ke folder Pictures (galeri)
        string galleryFolder = "/storage/emulated/0/Pictures"; // Direktori galeri Android
        if (!Directory.Exists(galleryFolder))
        {
            Directory.CreateDirectory(galleryFolder);
        }

        string galleryPath = Path.Combine(galleryFolder, fileName);
        File.Move(fullPath, galleryPath); // Pindahkan file screenshot

        // Gunakan MediaScanner untuk memberitahu Android untuk menambahkan gambar ke galeri
        if (Application.platform == RuntimePlatform.Android)
        {
            // Pastikan Anda mengakses UnityPlayer dengan benar untuk konteks Android
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

        // Sembunyikan flash
        yield return new WaitForSeconds(0.3f);
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(false);
        }
    }
}
