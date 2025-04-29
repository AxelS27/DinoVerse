using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabToSpawn; // Prefab objek yang akan muncul
    public AudioClip soundEnter; // Suara saat objek pertama kali muncul
    private AudioSource audioSource; // Audio source untuk memainkan suara

    private ARTrackedImageManager _arTrackedImageManager; // Manajer gambar yang terdeteksi AR
    private Dictionary<string, GameObject> _arObjects; // Dictionary untuk menyimpan objek berdasarkan nama gambar referensi

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;

        // Membuat objek untuk setiap prefab yang ditentukan
        foreach (GameObject prefab in prefabToSpawn)
        {
            GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newARObject.name = prefab.name;
            newARObject.SetActive(false);
            _arObjects.Add(newARObject.name, newARObject);
        }
    }

    private void OnDestroy()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Menangani gambar yang baru ditambahkan
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
            PlaySound(soundEnter);
        }

        // Menangani gambar yang diperbarui
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Menangani gambar yang dihapus
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (_arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                GameObject obj = _arObjects[trackedImage.referenceImage.name];
                obj.SetActive(false);
            }
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if (_arObjects.ContainsKey(trackedImage.referenceImage.name))
        {
            GameObject arObject = _arObjects[trackedImage.referenceImage.name];

            if (trackedImage.trackingState == TrackingState.Limited || trackedImage.trackingState == TrackingState.None)
            {
                arObject.SetActive(false);
                return;
            }

            if (!arObject.activeSelf)
            {
                PlaySound(soundEnter);
            }

            arObject.SetActive(true);
            arObject.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
