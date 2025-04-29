using UnityEngine;
using UnityEngine.Video;

public class VideoController3D : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;  // Video Player untuk memutar video
    [SerializeField] private Sprite playSprite;        // Sprite untuk play
    [SerializeField] private Sprite pauseSprite;       // Sprite untuk pause
    [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer pada objek tombol

    private bool isVideoPlaying = true; // Status video (apakah diputar atau dipause)

    void Start()
    {
        // Pastikan spriteRenderer telah diset jika tidak diberikan di Inspector
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Set sprite awal (misalnya play)
        spriteRenderer.sprite = pauseSprite;
    }

    void Update()
    {
        // Menggunakan raycast untuk mendeteksi klik pada objek 3D (tombol)
        if (Input.GetMouseButtonDown(0)) // 0 berarti klik kiri mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Mengecek apakah ray mengenai objek ini
            if (Physics.Raycast(ray, out hit))
            {
                // Mengecek apakah yang diklik adalah objek ini (tombol 3D)
                if (hit.collider.gameObject == gameObject)
                {
                    // Toggle antara play dan pause video
                    ToggleVideo();
                }
            }
        }
    }

    private void ToggleVideo()
    {
        if (isVideoPlaying)
        {
            // Jika video sedang diputar, pause video
            videoPlayer.Pause();
            // Ubah sprite menjadi play
            spriteRenderer.sprite = playSprite;
        }
        else
        {
            // Jika video dipause, play video
            videoPlayer.Play();
            // Ubah sprite menjadi pause
            spriteRenderer.sprite = pauseSprite;
        }

        // Toggle status video
        isVideoPlaying = !isVideoPlaying;
    }
}
