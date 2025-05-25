using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CommandSlot : MonoBehaviour, IDropHandler
{
    public string currentCommand;

    public CommandItem commandItemPrefab;
    public Transform inventoryPanel;

    public void OnDrop(PointerEventData eventData)
    {
        CommandItem droppedItem = eventData.pointerDrag.GetComponent<CommandItem>();
        if (droppedItem != null)
        {
            // Cek apakah di slot ini sudah ada CommandItem
            CommandItem existingItem = GetComponentInChildren<CommandItem>();

            if (existingItem != null)
            {
                if (existingItem != droppedItem)
                {
                    // Pindahkan existing item ke inventory fixed slotnya sesuai commandType
                    Transform targetInventorySlot = InventoryManager.Instance.GetSlotByCommandType(existingItem.commandType);
                    existingItem.transform.SetParent(targetInventorySlot, false);
                    existingItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    existingItem.isFromInventory = true;
                }
            }

            // Pasang item dropped ke command slot ini
            droppedItem.transform.SetParent(transform, false);
            RectTransform rt = droppedItem.GetComponent<RectTransform>();
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;

            currentCommand = droppedItem.commandType;

            // Kalau dari inventory, buat refill baru di slot fixednya
            if (droppedItem.isFromInventory)
            {
                Transform refillSlot = InventoryManager.Instance.GetSlotByCommandType(droppedItem.commandType);
                CommandItem newItem = Instantiate(commandItemPrefab, refillSlot);
                newItem.commandType = droppedItem.commandType;
                newItem.name = droppedItem.commandType;
                newItem.UpdateCommandText();
                newItem.isFromInventory = true;
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

            droppedItem.isFromInventory = false;

            UpdateCurrentCommand();
        }
    }

    public void UpdateCurrentCommand()
    {
        CommandItem item = GetComponentInChildren<CommandItem>();
        currentCommand = item != null ? item.commandType : "";
    }

}
