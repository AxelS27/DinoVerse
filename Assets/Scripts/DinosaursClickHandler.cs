using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class DinosaursClickHandler : MonoBehaviour
{
    private GameObject inspectorWorld;
    private Animator inspectorWorldAnimator;
    private TMP_Text inspectorText;

    private bool isButtonActive = false;
    private float clickCooldown = 1f;
    private float lastClickTime = -1f;

    void Start()
    {
        Canvas canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();

        if (canvas != null)
        {
            inspectorWorld = canvas.transform.Find("InspectorWorld")?.gameObject;

            if (inspectorWorld != null)
            {
                inspectorWorldAnimator = inspectorWorld.GetComponent<Animator>();
                inspectorText = inspectorWorld.GetComponentInChildren<TMP_Text>();

                if (inspectorText == null)
                {
                    Debug.LogError("TMP_Text not found in InspectorWorld!");
                }

                inspectorWorld.SetActive(false);
            }
            else
            {
                Debug.LogError("InspectorWorld not found inside Canvas!");
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene!");
        }
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Time.time - lastClickTime < clickCooldown)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    lastClickTime = Time.time;
                    isButtonActive = !isButtonActive;

                    if (inspectorWorld != null)
                    {
                        if (isButtonActive)
                        {
                            if (inspectorText != null)
                            {
                                inspectorText.text = gameObject.name;
                            }

                            inspectorWorld.SetActive(true);
                            inspectorWorldAnimator?.Play("InspectorFadeIn");
                        }
                        else
                        {
                            inspectorWorldAnimator?.Play("InspectorFadeOut");
                            StartCoroutine(WaitForFadeOutAndDisableButton());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator WaitForFadeOutAndDisableButton()
    {
        if (inspectorWorldAnimator != null)
        {
            yield return new WaitForSeconds(inspectorWorldAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        if (!isButtonActive && inspectorWorld != null)
        {
            inspectorWorld.SetActive(false);
        }
    }
}
