using UnityEngine;
using System.Collections;
public class CameraSwipeRotation : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Referensi ke kamera utama
    [SerializeField] private float swipeSpeed = 0.5f; // Kecepatan rotasi berdasarkan geseran
    [SerializeField] private float minRotationY = -35f; // Batas minimum rotasi Y
    [SerializeField] private float maxRotationY = 35f;  // Batas maksimum rotasi Y

    [SerializeField] public float startAnimationDuration = 3f;  // Durasi animasi awal kamera
    [SerializeField] public Vector3 startPosition = new Vector3(0f, 5f, -10f); // Posisi awal kamera (di belakang)
    [SerializeField] public Vector3 endPosition = new Vector3(0f, 0f, 0f); // Posisi akhir kamera (di depan)

    private float lastTouchPositionX = 0f; // Posisi X sentuhan terakhir
    private float rotationY = 0f; // Sudut rotasi kamera di sumbu Y
    private bool isAnimating = true; // Untuk mengatur animasi bergerak atau tidak

    void Start()
    {
        // Mulai animasi kamera dari belakang ke depan
        StartCoroutine(AnimateCameraIntro());
    }

    void Update()
    {
        if (!isAnimating) // Jika animasi sudah selesai, aktifkan swipe
        {
            // Memeriksa apakah ada sentuhan pada layar
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // Ambil touch pertama

                // Cek apakah sentuhan baru (dimulai)
                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPositionX = touch.position.x; // Menyimpan posisi sentuhan awal
                }
                // Ketika sentuhan bergerak (Moved)
                else if (touch.phase == TouchPhase.Moved)
                {
                    // Hitung perubahan posisi X, balikkan arah dengan mengalikan dengan -1
                    float deltaX = (lastTouchPositionX - touch.position.x); // Invert gerakan

                    // Rotasi kamera berdasarkan perbedaan gerakan horizontal pada layar
                    rotationY += deltaX * swipeSpeed * Time.deltaTime;

                    // Batasi rotasi agar tidak melebihi min/max
                    rotationY = Mathf.Clamp(rotationY, minRotationY, maxRotationY);

                    // Tentukan rotasi baru di sumbu Y
                    mainCamera.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

                    // Menyimpan posisi X terakhir untuk perhitungan berikutnya
                    lastTouchPositionX = touch.position.x;
                }
            }
        }
    }

    // Coroutine untuk animasi kamera bergerak dari belakang ke depan
    private IEnumerator AnimateCameraIntro()
    {
        float elapsedTime = 0f;

        // Set kamera ke posisi awal
        mainCamera.transform.position = startPosition;

        while (elapsedTime < startAnimationDuration)
        {
            // Menghitung posisi kamera berdasarkan waktu
            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / startAnimationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Pastikan kamera sampai di posisi akhir
        mainCamera.transform.position = endPosition;

        // Animasi selesai, kini rotasi kamera bisa dilakukan
        isAnimating = false;
    }
}
