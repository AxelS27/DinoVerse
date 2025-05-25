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
    public Button runButton;  // tombol Run yang dikontrol interactivity-nya

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
    private bool isRunningCommands = false;  // flag apakah command sedang dijalankan

    [Header("Crown Animation Settings")]
    public float crownFloatAmplitude = 0.5f;  // tinggi naik turun crown
    public float crownFloatFrequency = 1f;    // kecepatan naik turun crown
    public float crownRotationSpeed = 45f;    // derajat per detik rotasi crown

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

        // Set posisi awal dino
        dino.transform.position = dinoStartPosition;

        // Pastikan tombol run aktif awalnya
        if (runButton != null)
            runButton.interactable = true;
    }

    private void Update()
    {
        AnimateCrown();
    }

    void AnimateCrown()
    {
        // Naik turun sinusoidal
        float newY = crownStartPos.y + Mathf.Sin(Time.time * crownFloatFrequency * Mathf.PI * 2) * crownFloatAmplitude;
        crown.transform.position = new Vector3(crown.transform.position.x, newY, crown.transform.position.z);

        // Rotasi 360 derajat pada sumbu Y
        crown.transform.Rotate(Vector3.up, crownRotationSpeed * Time.deltaTime, Space.World);
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

        hasWon = false; // Otomatis bisa main lagi
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
                crownStartPos.y, // jaga y crown agar tetap di ketinggian awal
                Random.Range(-rangeZ, rangeZ)
            );
            attempt++;
        }
        while (Vector3.Distance(newPos, dino.transform.position) < minDistance && attempt < maxAttempts);

        crown.transform.position = newPos;
        crownStartPos = newPos; // update posisi awal crown untuk animasi
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
            runButton.interactable = false;  // disable tombol run saat jalan

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
            runButton.interactable = true;  // enable tombol run kembali setelah selesai
    }
}
