using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource; // Taruh AudioSource dari AR Session Origin
    public bool isMusicOn = true;

    [Header("UI")]
    public Image musicIcon;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (musicSource != null)
        {
            musicSource.mute = !isMusicOn;
        }

        if (musicIcon != null && musicOnSprite != null && musicOffSprite != null)
        {
            musicIcon.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
        }
    }
}
