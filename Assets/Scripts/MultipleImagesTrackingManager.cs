using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabToSpawn;
    public AudioClip soundEnter;
    private AudioSource audioSource;

    private ARTrackedImageManager _arTrackedImageManager;
    private Dictionary<string, GameObject> _arObjects;

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;

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
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
            PlaySound(soundEnter);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

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
