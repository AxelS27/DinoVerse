using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Popup : MonoBehaviour
{
    public GameObject popupPrefab; // Prefab popup yang akan ditampilkan
    private GameObject popupInstance; // Instansi dari popup
    public Vector3 offset = new Vector3(1, 0, 0); // Offset posisi popup (kanan objek)

    [Header("Popup Info")]
    public string objectName = "Nama Default"; // Nama objek
    public string description = "Ini adalah deskripsi default."; // Deskripsi objek
    public VideoClip videoClip; // VideoClip untuk video yang akan diputar

    private bool wasPopupActiveBeforeTrackingLost = false; // Menyimpan status popup sebelum kehilangan tracking

    void Update()
    {
        // Jika objek tidak aktif, sembunyikan popup
        if (!gameObject.activeInHierarchy && popupInstance != null)
        {
            HidePopup();
        }

        // Jika popup aktif, update posisinya
        if (popupInstance != null && popupInstance.activeSelf)
        {
            UpdatePopupPosition();
        }

        // Deteksi klik untuk toggle popup
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

        // Menghubungkan komponen dalam popup (Text dan Video)
        TextMeshPro namaText = popupInstance.transform.Find("NameText")?.GetComponent<TextMeshPro>();
        TextMeshPro deskripsiText = popupInstance.transform.Find("DescriptionText")?.GetComponent<TextMeshPro>();

        // Mendapatkan komponen VideoPlayer
        VideoPlayer videoPlayer = popupInstance.transform.Find("VideoPlayer")?.GetComponent<VideoPlayer>();

        // Cek jika VideoPlayer valid dan VideoClip terisi
        if (namaText != null) namaText.text = objectName;
        if (deskripsiText != null) deskripsiText.text = description;

        if (videoPlayer != null && videoClip != null)
        {
            // Menetapkan VideoClip ke VideoPlayer dan mulai memutar video
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }
    }

    void UpdatePopupPosition()
    {
        if (popupInstance == null) return;

        // Update posisi popup di kanan objek utama dengan offset
        popupInstance.transform.position = transform.position + (transform.right * offset.x) + (Vector3.up * offset.y) + (transform.forward * offset.z);

        // Popup selalu menghadap kamera tanpa rotasi yang terkunci di sumbu Y
        popupInstance.transform.LookAt(Camera.main.transform);

        // Putar 180 derajat untuk memastikan teks tidak terbalik
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

    // Properti untuk status popup
    public bool WasPopupActive => wasPopupActiveBeforeTrackingLost;
}
