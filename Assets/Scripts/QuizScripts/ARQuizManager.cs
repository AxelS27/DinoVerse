using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ARQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizQuestion
    {
        public string questionText;
        public string correctAnswer;
        public string[] options;
    }

    public List<QuizQuestion> questions;
    public Camera arCamera;

    public TextMeshPro questionText3D;
    public TextMeshProUGUI countdownTextUI;    // Ganti countdown jadi TMP UI
    public TextMeshPro scoreText3D;

    public AudioClip beepSound;        // Suara beep countdown
    public AudioClip correctSound;     // Suara benar
    public AudioClip wrongSound;       // Suara salah
    public AudioSource audioSource;

    private int currentQuestionIndex = 0;
    private int correctCount = 0;
    private int wrongCount = 0;

    private List<GameObject> spawnedDinos = new List<GameObject>();
    private bool isWaitingForInput = false;

    // Warna yang digunakan
    private Color colorWhite = Color.white;
    private Color colorGreen = Color.green;
    private Color colorRed = Color.red;

    void Start()
    {
        scoreText3D.text = "";
        countdownTextUI.color = colorWhite;  // default putih
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        // Delay 3 detik sebelum mulai quiz
        yield return new WaitForSeconds(3f);
        StartCoroutine(CountdownAndShowQuestion());
    }

    void Update()
    {
        if (!isWaitingForInput) return;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(Input.touches[0].position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Dino"))
                {
                    string selectedName = hit.collider.gameObject.name.Replace("(Clone)", "").Trim();
                    CheckAnswer(selectedName);
                }
            }
        }
    }

    IEnumerator CountdownAndShowQuestion()
    {
        ClearDinos();
        isWaitingForInput = false;

        int countdownStart = 3; // durasi countdown

        audioSource.PlayOneShot(beepSound);
        countdownTextUI.color = colorWhite;  // Warna putih saat countdown

        for (int i = countdownStart; i > 0; i--)
        {
            countdownTextUI.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownTextUI.text = "";

        if (currentQuestionIndex >= questions.Count)
        {
            questionText3D.text = "Quiz Finished!";
            scoreText3D.text = $"Correct: {correctCount} | Wrong: {wrongCount}";
            yield break;
        }

        ShowQuestion();
    }

    void ShowQuestion()
    {
        QuizQuestion q = questions[currentQuestionIndex];
        questionText3D.text = q.questionText;

        Vector3 basePos = arCamera.transform.position + arCamera.transform.forward * .8f;

        for (int i = 0; i < q.options.Length; i++)
        {
            GameObject prefab = Resources.Load<GameObject>("Dinos/" + q.options[i]);
            if (prefab != null)
            {
                Vector3 spawnPos = basePos + new Vector3((i - 1) * 0.6f, -0.3f, 0);
                GameObject dino = Instantiate(prefab, spawnPos, Quaternion.identity);

                dino.name = q.options[i];
                dino.tag = "Dino";
                spawnedDinos.Add(dino);
            }
            else
            {
                Debug.LogError("Missing prefab: " + q.options[i]);
            }
        }

        isWaitingForInput = true;
        scoreText3D.text = $"Correct: {correctCount} | Wrong: {wrongCount}";
    }

    void CheckAnswer(string selectedDino)
    {
        isWaitingForInput = false;

        string correct = questions[currentQuestionIndex].correctAnswer;
        bool isCorrect = selectedDino == correct;

        if (isCorrect)
        {
            correctCount++;
            countdownTextUI.text = "TRUE";
            countdownTextUI.color = colorGreen;  // warna hijau saat benar
            if (audioSource != null && correctSound != null)
                audioSource.PlayOneShot(correctSound);
        }
        else
        {
            wrongCount++;
            countdownTextUI.text = "WRONG";
            countdownTextUI.color = colorRed;    // warna merah saat salah
            if (audioSource != null && wrongSound != null)
                audioSource.PlayOneShot(wrongSound);
        }

        currentQuestionIndex++;

        StartCoroutine(NextAfterDelay());
    }

    IEnumerator NextAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        // Reset warna ke putih saat mulai countdown baru
        countdownTextUI.color = colorWhite;
        StartCoroutine(CountdownAndShowQuestion());
    }

    void ClearDinos()
    {
        foreach (GameObject dino in spawnedDinos)
        {
            Destroy(dino);
        }
        spawnedDinos.Clear();
    }
}
