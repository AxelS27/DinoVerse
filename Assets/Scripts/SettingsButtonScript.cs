using UnityEngine;
using TMPro;
using System.Collections;

public class SettingsButtonScript : MonoBehaviour
{
    public Animator gearAnimator;

    // Animator untuk masing-masing objek UI yang memiliki animasi fade-in dan fade-out
    public Animator dropdownAnimator;
    public Animator cameraAnimator;
    public Animator clearAnimator;
    public Animator homeAnimator;
    public Animator musicAnimator;

    // Objek-objek UI yang akan diaktifkan atau dinonaktifkan
    public GameObject dropdownUI;
    public GameObject cameraUI;
    public GameObject clearUI;
    public GameObject homeUI;
    public GameObject musicUI;

    private bool isMenuOpen = false;

    // Variabel untuk cooldown
    public float cooldownTime = 0.5f; // Durasi cooldown dalam detik
    private float lastClickTime = -Mathf.Infinity; // Waktu terakhir tombol ditekan

    void Start()
    {
        // Pastikan semua objek UI dalam keadaan tidak aktif saat pertama kali
        dropdownUI.SetActive(false);
        cameraUI.SetActive(false);
        clearUI.SetActive(false);
        homeUI.SetActive(false);
        musicUI.SetActive(false);
    }

    public void OnSettingsClicked()
    {
        // Cek apakah tombol ditekan sebelum cooldown berakhir
        if (Time.time - lastClickTime < cooldownTime)
        {
            return; // Jangan lakukan apa-apa jika cooldown belum selesai
        }

        // Update waktu terakhir tombol ditekan
        lastClickTime = Time.time;

        if (!isMenuOpen)
        {
            // Menu settings akan dibuka, gear animasi bergerak
            gearAnimator.Play("SettingsButtonOpen");

            // Aktifkan UI satu per satu sebelum fade-in
            dropdownUI.SetActive(true);
            cameraUI.SetActive(true);
            clearUI.SetActive(true);
            homeUI.SetActive(true);
            musicUI.SetActive(true);

            // Mainkan fade-in animations untuk setiap objek UI
            PlayFadeInAnimations();

            isMenuOpen = true;
        }
        else
        {
            // Menu settings akan ditutup, gear animasi bergerak
            gearAnimator.Play("SettingsButtonClose");

            // Mainkan fade-out animations untuk setiap objek UI
            PlayFadeOutAnimations();

            // Menonaktifkan UI satu per satu setelah fade-out selesai
            StartCoroutine(DisableUIAfterFadeOut());

            isMenuOpen = false;
        }
    }

    private void PlayFadeInAnimations()
    {
        // Mainkan animasi fade-in untuk masing-masing objek UI
        dropdownAnimator.Play("DropdownFadeIn");
        cameraAnimator.Play("CameraFadeIn");
        clearAnimator.Play("ClearFadeIn");
        homeAnimator.Play("HomeFadeIn");
        musicAnimator.Play("MusicFadeIn");
    }

    private void PlayFadeOutAnimations()
    {
        // Mainkan animasi fade-out untuk masing-masing objek UI
        dropdownAnimator.Play("DropdownFadeOut");
        cameraAnimator.Play("CameraFadeOut");
        clearAnimator.Play("ClearFadeOut");
        homeAnimator.Play("HomeFadeOut");
        musicAnimator.Play("MusicFadeOut");
    }

    private IEnumerator DisableUIAfterFadeOut()
    {
        // Tunggu sampai animasi fade-out selesai untuk setiap UI
        yield return new WaitForSeconds(GetAnimationDuration(dropdownAnimator, "DropdownFadeOut"));
        // Menonaktifkan UI setelah animasi selesai
        dropdownUI.SetActive(false);
        cameraUI.SetActive(false);
        clearUI.SetActive(false);
        homeUI.SetActive(false);
        musicUI.SetActive(false);
    }

    private float GetAnimationDuration(Animator animator, string animationName)
    {
        // Mendapatkan durasi animasi berdasarkan nama animasi yang sedang dimainkan
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length; // Kembalikan durasi animasi
            }
        }
        return 0f; // Jika animasi tidak ditemukan, kembalikan 0
    }
}
