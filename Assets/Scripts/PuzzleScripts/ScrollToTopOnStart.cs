using UnityEngine;
using UnityEngine.UI;

public class ScrollToTopOnStart : MonoBehaviour
{
    public ScrollRect scrollRect;

    void Start()
    {
        scrollRect.verticalNormalizedPosition = 1f; // posisi scroll paling atas
    }
}
