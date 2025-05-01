using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Popup : MonoBehaviour
{
    public GameObject popupPrefab;
    private GameObject popupInstance;
    public Vector3 offset = new Vector3(1, 0, 0);

    [Header("Popup Info")]
    public string objectName = "Nama Default";
    public string description = "Ini adalah deskripsi default.";
    public VideoClip videoClip;

    private bool wasPopupActiveBeforeTrackingLost = false;

    void Update()
    {
        if (!gameObject.activeInHierarchy && popupInstance != null)
        {
            HidePopup();
        }

        if (popupInstance != null && popupInstance.activeSelf)
        {
            UpdatePopupPosition();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                TogglePopup();
            }
        }
    }

    void TogglePopup()
    {
        if (popupInstance == null)
        {
            CreatePopup();
            wasPopupActiveBeforeTrackingLost = true;
        }
        else
        {
            bool isActive = popupInstance.activeSelf;
            popupInstance.SetActive(!isActive);
            wasPopupActiveBeforeTrackingLost = !isActive;
        }
    }

    void CreatePopup()
    {
        popupInstance = Instantiate(popupPrefab, transform.position + offset, Quaternion.identity);
        UpdatePopupPosition();

        TextMeshPro namaText = popupInstance.transform.Find("NameText")?.GetComponent<TextMeshPro>();
        TextMeshPro deskripsiText = popupInstance.transform.Find("DescriptionText")?.GetComponent<TextMeshPro>();

        VideoPlayer videoPlayer = popupInstance.transform.Find("VideoPlayer")?.GetComponent<VideoPlayer>();

        if (namaText != null) namaText.text = objectName;
        if (deskripsiText != null) deskripsiText.text = description;

        if (videoPlayer != null && videoClip != null)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }
    }

    void UpdatePopupPosition()
    {
        if (popupInstance == null) return;

        popupInstance.transform.position = transform.position + (transform.right * offset.x) + (Vector3.up * offset.y) + (transform.forward * offset.z);
        popupInstance.transform.LookAt(Camera.main.transform);
        popupInstance.transform.Rotate(0, 180, 0);
    }

    public void HidePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
        }
    }

    public void ShowPopup()
    {
        if (popupInstance == null)
        {
            CreatePopup();
        }
    }

    private void OnDisable()
    {
        if (popupInstance != null)
        {
            wasPopupActiveBeforeTrackingLost = popupInstance.activeSelf;
            HidePopup();
        }
    }
    public bool WasPopupActive => wasPopupActiveBeforeTrackingLost;
}
