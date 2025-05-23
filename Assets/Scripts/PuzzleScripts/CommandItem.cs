using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string commandType;  // Misal: "Move Forward", "Turn Left", dll
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

        // Cari komponen TextMeshProUGUI di child
        commandText = GetComponentInChildren<TextMeshProUGUI>();
        backgroundImage = GetComponent<Image>();


        UpdateCommandText();
    }

    public void UpdateCommandText()
    {
        if (commandText != null)
        {
            commandText.text = commandType;

            // Set warna berdasarkan commandType
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

        // Lepaskan dari parent dulu supaya slot lama kosong dan update currentCommand bisa pas
        transform.SetParent(transform.root);

        // Kalau perlu update command slot lama
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
            // Drop gagal ditempat yang valid
            if (isFromInventory)
            {
                // Jika dari inventory, kembalikan ke inventory
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                // Jika bukan dari inventory (sudah ada di slot), berarti drop ke tempat invalid, hapus objek
                Destroy(gameObject);
            }

            CommandSlot oldSlot = originalParent.GetComponent<CommandSlot>();
            if (oldSlot != null)
                oldSlot.UpdateCurrentCommand();
        }
        else
        {
            // Drop berhasil ke slot baru, update posisi dan command slot baru
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
