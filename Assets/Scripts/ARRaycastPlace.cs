using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ARRaycastPlace : MonoBehaviour
{
    public TMP_Dropdown TMP_Dropdown;
    public ARRaycastManager raycastManager;
    public Camera arCamera;
    public ARPlaneManager arPlaneManager;
    public Button clearAllButton;
    public AudioSource placementAudio;

    public GameObject[] placeableObjects;
    public float placementCooldown = 0.1f;

    private float lastPlacementTime = 0f;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject objectToPlace;
    private List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        OnDropdownChanged(TMP_Dropdown.value);
        TMP_Dropdown.onValueChanged.AddListener(OnDropdownChanged);
        clearAllButton.onClick.AddListener(ClearAllObjects);
        StartCoroutine(HidePlaneIfNone());
    }

    IEnumerator HidePlaneIfNone()
    {
        yield return null;
        if (TMP_Dropdown.value == 0)
        {
            ToggleARPlanes(false);
        }
    }

    void OnDropdownChanged(int index)
    {
        if (index == 0)
        {
            objectToPlace = null;
            ToggleARPlanes(false);
        }
        else if (index >= 1 && index <= placeableObjects.Length)
        {
            objectToPlace = placeableObjects[index - 1];
            ToggleARPlanes(true);
        }
    }

    void ToggleARPlanes(bool isVisible)
    {
        arPlaneManager.enabled = isVisible;

        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(isVisible);

            var meshVisualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
            if (meshVisualizer != null)
                meshVisualizer.enabled = isVisible;

            var renderer = plane.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = isVisible;

            var collider = plane.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = isVisible;
        }
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(0) && objectToPlace != null)
        {
            if (Time.time - lastPlacementTime < placementCooldown)
                return;

            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);

            if (TMP_Dropdown.value != 0 && raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                ARPlane hitPlane = arPlaneManager.GetPlane(hits[0].trackableId);
                if (hitPlane != null && hitPlane.gameObject.activeInHierarchy)
                {
                    Quaternion randomYRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                    GameObject placedObject = Instantiate(objectToPlace, hitPose.position, randomYRotation);
                    placedObjects.Add(placedObject);

                    lastPlacementTime = Time.time;

                    if (placementAudio != null)
                        placementAudio.Play();

                    StartCoroutine(AnimatePlacement(placedObject, objectToPlace.transform.localScale));
                }
            }
        }
    }

    IEnumerator AnimatePlacement(GameObject obj, Vector3 targetScale, float duration = 0.15f)
    {
        float time = 0f;
        obj.transform.localScale = Vector3.zero;

        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }

    public void ClearAllObjects()
    {
        foreach (var placedObject in placedObjects)
        {
            Destroy(placedObject);
        }
        placedObjects.Clear();
    }
}