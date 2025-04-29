using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Untuk mengakses Text pada tombol
using System.Collections;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _startingSceneTransition;
    [SerializeField] private GameObject _endingSceneTransition;
    [SerializeField] private Animator endingAnimator;
    [SerializeField] private GameObject inspectorButton; // Tombol yang memiliki text untuk scene 'InspectorWorld'

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene") // Kalau di scene AR
        {
            StartCoroutine(PlayEndingAnimDirectly());
        }
        else
        {
            StartCoroutine(NormalStartTransition());
        }
    }

    IEnumerator NormalStartTransition()
    {
        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(true);

        yield return new WaitForSeconds(1f); // Dipersingkat dari 5 detik jadi 1 detik

        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(false);
    }

    IEnumerator PlayEndingAnimDirectly()
    {
        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(true);

        yield return new WaitForSeconds(0.5f); // Delay kecil saja buat transisi halus

        if (endingAnimator != null)
        {
            endingAnimator.Play("EndingTransition"); // Mainkan animasi ending
        }

        yield return new WaitForSeconds(1.5f); // Cepatkan tunggu animasi

        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(false);
    }

    public void PlayGame()
    {
        if (_endingSceneTransition != null)
            _endingSceneTransition.SetActive(true);

        StartCoroutine(WaitAndLoadScene("SampleScene")); // Menggunakan nama scene
    }

    public void BackToMenu()
    {
        if (_endingSceneTransition != null)
            _endingSceneTransition.SetActive(true);

        StartCoroutine(WaitAndLoadScene("MainMenu")); // Menggunakan nama scene
    }

    // Fungsi InspectorWorld untuk pindah ke scene berdasarkan teks tombol
    public void InspectorWorld()
    {
        if (_endingSceneTransition != null)
            _endingSceneTransition.SetActive(true); // Aktifkan transisi mulai

        // Membaca teks tombol untuk menentukan scene yang akan dimuat
        string buttonText = inspectorButton.GetComponentInChildren<TextMeshProUGUI>().text;
        StartCoroutine(WaitAndLoadScene(buttonText)); // Pindah ke scene berdasarkan teks tombol
    }

    IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f); // Loading cepat, 1 detik
        SceneManager.LoadScene(sceneName); // Muat scene berdasarkan nama
    }
}
