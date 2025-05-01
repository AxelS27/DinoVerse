using UnityEngine;
using System.Collections;
public class CameraRotationController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float swipeSpeed = 0.5f;
    [SerializeField] private float minRotationY = -35f;
    [SerializeField] private float maxRotationY = 35f;

    [SerializeField] public float startAnimationDuration = 3f;
    [SerializeField] public Vector3 startPosition = new Vector3(0f, 5f, -10f);
    [SerializeField] public Vector3 endPosition = new Vector3(0f, 0f, 0f);

    private float lastTouchPositionX = 0f;
    private float rotationY = 0f;
    private bool isAnimating = true;

    void Start()
    {
        StartCoroutine(AnimateCameraIntro());
    }

    void Update()
    {
        if (!isAnimating)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPositionX = touch.position.x;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = (lastTouchPositionX - touch.position.x);

                    rotationY += deltaX * swipeSpeed * Time.deltaTime;
                    rotationY = Mathf.Clamp(rotationY, minRotationY, maxRotationY);
                    mainCamera.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
                    lastTouchPositionX = touch.position.x;
                }
            }
        }
    }

    private IEnumerator AnimateCameraIntro()
    {
        float elapsedTime = 0f;

        mainCamera.transform.position = startPosition;

        while (elapsedTime < startAnimationDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / startAnimationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = endPosition;

        isAnimating = false;
    }
}
