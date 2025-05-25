using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialSlidesManager : MonoBehaviour
{
    [System.Serializable]
    public class SlideData
    {
        public Sprite image;
        public string title;
        [TextArea] public string description;
    }

    [Header("Slides Content")]
    public SlideData[] slides;
    public Image slideImage; // ganti dari SpriteRenderer ke UI Image
    public TextMeshPro titleText;
    public TextMeshPro descriptionText;

    [Header("Navigation Buttons")]
    public GameObject nextButtonObject;
    public GameObject previousButtonObject;

    private int currentIndex = 0;

    void Start()
    {
        if (slides.Length == 0)
        {
            Debug.LogWarning("No slides available.");
            return;
        }

        ShowSlide(currentIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Clicked on: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == nextButtonObject)
                {
                    Debug.Log("Next Button Clicked");
                    NextSlide();
                }
                else if (hit.collider.gameObject == previousButtonObject)
                {
                    Debug.Log("Previous Button Clicked");
                    PreviousSlide();
                }
            }
            else
            {
                Debug.Log("Raycast hit nothing");
            }
        }
    }

    public void NextSlide()
    {
        if (slides.Length == 0) return;

        currentIndex = (currentIndex + 1) % slides.Length;
        ShowSlide(currentIndex);
    }

    public void PreviousSlide()
    {
        if (slides.Length == 0) return;

        currentIndex = (currentIndex - 1 + slides.Length) % slides.Length;
        ShowSlide(currentIndex);
    }

    private void ShowSlide(int index)
    {
        if (index < 0 || index >= slides.Length) return;

        var slide = slides[index];
        if (slideImage != null) slideImage.sprite = slide.image;
        if (titleText != null) titleText.text = slide.title;
        if (descriptionText != null) descriptionText.text = slide.description;

        Debug.Log($"Showing slide {index + 1}/{slides.Length}: {slide.title}");
    }
}
