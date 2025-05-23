using UnityEngine;
using System.Collections;

public class CommandExecutor : MonoBehaviour
{
    public GameObject dino;
    public CommandSlot[] slots;

    public void RunCommands()
    {
        var dinoCtrl = dino.GetComponent<DinoController>();
        dinoCtrl.ResetPosition();  // Reset posisi dinosaurus ke posisi awal dulu
        StartCoroutine(ExecuteCommands());
    }
    public void ResetCommands()
    {
        foreach (var slot in slots)
        {
            CommandItem item = slot.GetComponentInChildren<CommandItem>();
            if (item != null)
            {
                Destroy(item.gameObject);
            }

            // Reset currentCommand slot
            slot.currentCommand = "";
        }
    }
    private IEnumerator ExecuteCommands()
    {
        foreach (var slot in slots)
        {
            if (string.IsNullOrEmpty(slot.currentCommand)) continue;

            var dinoCtrl = dino.GetComponent<DinoController>();
            switch (slot.currentCommand)
            {
                case "Move Forward":
                    yield return dinoCtrl.MoveForward();
                    break;
                case "Move Backward":
                    yield return dinoCtrl.MoveBackward();
                    break;
                case "Turn Left":
                    yield return dinoCtrl.TurnLeft();
                    break;
                case "Turn Right":
                    yield return dinoCtrl.TurnRight();
                    break;
            }

            yield return new WaitForSeconds(0.5f); // jeda antar perintah
        }
    }
}
