using UnityEngine;
using UnityEngine.Video;

public class VideoController3D : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isVideoPlaying = true;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.sprite = pauseSprite;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    ToggleVideo();
                }
            }
        }
    }

    private void ToggleVideo()
    {
        if (isVideoPlaying)
        {
            videoPlayer.Pause();
            spriteRenderer.sprite = playSprite;
        }
        else
        {
            videoPlayer.Play();
            spriteRenderer.sprite = pauseSprite;
        }


        isVideoPlaying = !isVideoPlaying;
    }
}
