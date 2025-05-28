using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string commandType;
    public bool isFromInventory = true;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private TextMeshProUGUI commandText;

    private Image backgroundImage;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        commandText = GetComponentInChildren<TextMeshProUGUI>();
        backgroundImage = GetComponent<Image>();


        UpdateCommandText();
    }

    public void UpdateCommandText()
    {
        if (commandText != null)
        {
            commandText.text = commandType;

            switch (commandType)
            {
                case "Move Forward":
                    backgroundImage.color = new Color(44/255f, 112/255f, 174/255f);
                    commandText.color = Color.white;
                    break;
                case "Turn Left":
                    backgroundImage.color = new Color(46/255f, 157/255f, 166/255f);
                    commandText.color = Color.white;
                    break;
                case "Turn Right":
                    backgroundImage.color = new Color(213/255f, 84/255f, 109/255f);
                    commandText.color = Color.white;
                    break;
                case "Move Backward":
                    backgroundImage.color = new Color(234/255f, 169/255f, 46/255f);
                    commandText.color = Color.white;
                    break;
                default:
                    commandText.color = Color.white;
                    break;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        transform.SetParent(transform.root);

        CommandSlot oldSlot = originalParent.GetComponent<CommandSlot>();
        if (oldSlot != null)
        {
            oldSlot.UpdateCurrentCommand();
        }

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetCanvasScaleFactor();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == transform.root)
        {
            if (isFromInventory)
            {
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                Destroy(gameObject);
            }

            CommandSlot oldSlot = originalParent.GetComponent<CommandSlot>();
            if (oldSlot != null)
                oldSlot.UpdateCurrentCommand();
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.zero;

            CommandSlot newSlot = transform.parent.GetComponent<CommandSlot>();
            if (newSlot != null)
                newSlot.UpdateCurrentCommand();
        }
    }

    private float GetCanvasScaleFactor()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.scaleFactor : 1f;
    }
}
