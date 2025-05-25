using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIVisibilityToggle : MonoBehaviour
{
    [Header("UI Elements to Fade In/Out (with CanvasGroup)")]
    public GameObject[] uiElements;

    private bool isHidden = false;

    // Warna biru RGB (0, 133, 161)
    private Color visibleColor = new Color(0f / 255f, 133f / 255f, 161f / 255f);
    // Warna merah RGB (255, 0, 0)
    private Color hiddenColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);

    private Image buttonImage;

    // Cooldown waktu dalam detik
    private float clickCooldown = .2f;
    private float lastClickTime = -Mathf.Infinity;

    // Durasi fade
    public float fadeDuration = 0.5f;

    void Start()
    {
        buttonImage = GetComponent<Image>();

        isHidden = false; // memastikan visibilitas awal tampil

        foreach (GameObject obj in uiElements)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                CanvasGroup cg = EnsureCanvasGroup(obj);
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        UpdateButtonColor();
        GetComponent<Button>().onClick.AddListener(TryToggleUI);
    }

    public void TryToggleUI()
    {
        if (Time.time - lastClickTime < clickCooldown)
        {
            Debug.Log("ToggleUI blocked by cooldown.");
            return;
        }

        lastClickTime = Time.time;
        ToggleUI();
    }

    public void ToggleUI()
    {
        isHidden = !isHidden;

        foreach (GameObject obj in uiElements)
        {
            if (obj != null)
            {
                CanvasGroup cg = EnsureCanvasGroup(obj);

                if (!isHidden)
                {
                    obj.SetActive(true); // Aktifkan dulu sebelum fade in
                    StartCoroutine(FadeCanvasGroup(cg, 1f, fadeDuration, false));
                }
                else
                {
                    StartCoroutine(FadeCanvasGroup(cg, 0f, fadeDuration, true)); // Fade out, lalu disable
                }
            }
        }

        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isHidden ? hiddenColor : visibleColor;
        }
    }

    CanvasGroup EnsureCanvasGroup(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = obj.AddComponent<CanvasGroup>();
        return cg;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration, bool disableAfterFade)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 1f)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        if (disableAfterFade)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
