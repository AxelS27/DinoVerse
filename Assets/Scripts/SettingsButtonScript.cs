using UnityEngine;
using TMPro;
using System.Collections;

public class SettingsButtonScript : MonoBehaviour
{
    public Animator gearAnimator;

    public Animator dropdownAnimator;
    public Animator cameraAnimator;
    public Animator clearAnimator;
    public Animator homeAnimator;
    public Animator musicAnimator;

    public GameObject dropdownUI;
    public GameObject cameraUI;
    public GameObject clearUI;
    public GameObject homeUI;
    public GameObject musicUI;

    private bool isMenuOpen = false;

    public float cooldownTime = 0.5f;
    private float lastClickTime = -Mathf.Infinity;

    void Start()
    {
        dropdownUI.SetActive(false);
        cameraUI.SetActive(false);
        clearUI.SetActive(false);
        homeUI.SetActive(false);
        musicUI.SetActive(false);
    }

    public void OnSettingsClicked()
    {
        if (Time.time - lastClickTime < cooldownTime)
        {
            return;
        }

        lastClickTime = Time.time;

        if (!isMenuOpen)
        {
            gearAnimator.Play("SettingsButtonOpen");

            dropdownUI.SetActive(true);
            cameraUI.SetActive(true);
            clearUI.SetActive(true);
            homeUI.SetActive(true);
            musicUI.SetActive(true);

            PlayFadeInAnimations();

            isMenuOpen = true;
        }
        else
        {
            gearAnimator.Play("SettingsButtonClose");

            PlayFadeOutAnimations();
            StartCoroutine(DisableUIAfterFadeOut());

            isMenuOpen = false;
        }
    }

    private void PlayFadeInAnimations()
    {
        dropdownAnimator.Play("DropdownFadeIn");
        cameraAnimator.Play("CameraFadeIn");
        clearAnimator.Play("ClearFadeIn");
        homeAnimator.Play("HomeFadeIn");
        musicAnimator.Play("MusicFadeIn");
    }

    private void PlayFadeOutAnimations()
    {
        dropdownAnimator.Play("DropdownFadeOut");
        cameraAnimator.Play("CameraFadeOut");
        clearAnimator.Play("ClearFadeOut");
        homeAnimator.Play("HomeFadeOut");
        musicAnimator.Play("MusicFadeOut");
    }

    private IEnumerator DisableUIAfterFadeOut()
    {
        yield return new WaitForSeconds(GetAnimationDuration(dropdownAnimator, "DropdownFadeOut"));

        dropdownUI.SetActive(false);
        cameraUI.SetActive(false);
        clearUI.SetActive(false);
        homeUI.SetActive(false);
        musicUI.SetActive(false);
    }

    private float GetAnimationDuration(Animator animator, string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f;
    }
}
