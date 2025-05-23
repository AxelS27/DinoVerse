using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [System.Serializable]
    public struct SlotMapping
    {
        public string commandType;
        public Transform slotTransform;
    }

    public SlotMapping[] slotMappings;

    void Awake()
    {
        Instance = this;
    }

    public Transform GetSlotByCommandType(string commandType)
    {
        foreach (var mapping in slotMappings)
        {
            if (mapping.commandType == commandType)
                return mapping.slotTransform;
        }
        return null;
    }
}
