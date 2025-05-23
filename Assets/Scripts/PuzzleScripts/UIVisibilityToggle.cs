using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityToggle : MonoBehaviour
{
    [Header("UI Elements to Toggle")]
    public GameObject[] uiElements;

    private bool isHidden = true;

    // Warna biru RGB (0, 133, 161)
    private Color visibleColor = new Color(255f / 255f, 0f / 255f, 0f / 255f); 
    // Warna merah RGB (255, 0, 0)
    private Color hiddenColor = new Color(0f / 255f, 133f / 255f, 161f / 255f);

    private Image buttonImage;

    // Cooldown waktu dalam detik
    private float clickCooldown = 2f;
    private float lastClickTime = -Mathf.Infinity; // Supaya bisa klik pertama tanpa delay

    void Start()
    {
        buttonImage = GetComponent<Image>();
        foreach (GameObject obj in uiElements)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Tambahkan listener
        GetComponent<Button>().onClick.AddListener(TryToggleUI);
    }

    // Fungsi ini yang dipanggil dari listener tombol
    public void TryToggleUI()
    {
        // Cek cooldown
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
        Debug.Log("Toggled UI. isHidden = " + isHidden);

        foreach (GameObject obj in uiElements)
        {
            if (obj != null)
            {
                obj.SetActive(!isHidden);
                Debug.Log("SetActive " + obj.name + ": " + (!isHidden));
            }
            else
            {
                Debug.LogWarning("Null object found in uiElements array.");
            }
        }

        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isHidden ? hiddenColor : visibleColor;
            Debug.Log("Button color updated to: " + (isHidden ? "Red" : "Blue"));
        }
        else
        {
            Debug.LogWarning("No Image component found on this GameObject.");
        }
    }
}
