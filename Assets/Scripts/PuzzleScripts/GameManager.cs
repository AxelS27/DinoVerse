using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject dino;
    public GameObject crown;
    public AudioSource winSound;

    [Header("UI Buttons")]
    public Button runButton;

    [Header("Dino Start Position")]
    public Vector3 dinoStartPosition;

    [Header("Command Slots")]
    public CommandSlot[] slots;

    [Header("Random Position Range")]
    public float rangeX = 5f;
    public float rangeZ = 5f;

    [Header("Command Feedback")]
    public Color highlightColor = Color.yellow;
    public Color normalColor = Color.white;

    public AudioClip commandSound;
    private AudioSource audioSource;

    private bool hasWon = false;
    private bool isRunningCommands = false;

    private Vector3 crownStartPos;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        crownStartPos = crown.transform.position;

        RandomizeCrownPosition();
        hasWon = false;

        dino.transform.position = dinoStartPosition;

        if (runButton != null)
            runButton.interactable = true;
    }

    public void HandleWinFromDino()
    {
        if (hasWon) return;
        StartCoroutine(HandleWinCoroutine());
    }

    private IEnumerator HandleWinCoroutine()
    {
        hasWon = true;

        Debug.Log("Player Win!");

        if (winSound != null)
            winSound.Play();

        yield return new WaitForSeconds(3f);

        dino.transform.position = dinoStartPosition;
        dino.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        ResetCommands();
        RandomizeCrownPosition();

        hasWon = false;
    }

    void RandomizeCrownPosition()
    {
        float minDistance = 2f;

        Vector3 newPos;
        int maxAttempts = 20;
        int attempt = 0;

        do
        {
            newPos = new Vector3(
                Random.Range(-rangeX, rangeX),
                crownStartPos.y,
                Random.Range(-rangeZ, rangeZ)
            );
            attempt++;
        }
        while (Vector3.Distance(newPos, dino.transform.position) < minDistance && attempt < maxAttempts);

        crown.transform.position = newPos;
        crownStartPos = newPos;
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

            slot.currentCommand = "";

            dino.transform.position = dinoStartPosition;
            dino.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            var img = slot.GetComponent<Image>();
            if (img != null) img.color = normalColor;
        }
    }

    public void ResetWinState()
    {
        hasWon = false;
    }

    public void RunCommands()
    {
        if (hasWon || isRunningCommands) return;

        if (runButton != null)
            runButton.interactable = false;

        dino.transform.position = dinoStartPosition;
        dino.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        StartCoroutine(ExecuteCommands());
    }

    private IEnumerator ExecuteCommands()
    {
        isRunningCommands = true;

        var dinoCtrl = dino.GetComponent<DinoController>();

        foreach (var slot in slots)
        {
            if (string.IsNullOrEmpty(slot.currentCommand)) continue;

            var slotImage = slot.GetComponent<Image>();
            if (slotImage != null)
                slotImage.color = highlightColor;

            if (commandSound != null)
                audioSource.PlayOneShot(commandSound);

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

            yield return new WaitForSeconds(0.5f);

            if (slotImage != null)
                slotImage.color = normalColor;
        }

        isRunningCommands = false;

        if (runButton != null)
            runButton.interactable = true;
    }
}
