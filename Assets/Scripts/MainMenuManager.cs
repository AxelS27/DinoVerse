using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _startingSceneTransition;
    [SerializeField] private GameObject _endingSceneTransition;
    [SerializeField] private Animator endingAnimator;
    [SerializeField] private GameObject inspectorButton;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
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

        yield return new WaitForSeconds(1f);

        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(false);
    }

    IEnumerator PlayEndingAnimDirectly()
    {
        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        if (endingAnimator != null)
        {
            endingAnimator.Play("EndingTransition");
        }

        yield return new WaitForSeconds(1.5f);

        if (_startingSceneTransition != null)
            _startingSceneTransition.SetActive(false);
    }

    public void PlayGame()
    {
        if (_endingSceneTransition != null)
            _endingSceneTransition.SetActive(true);

        StartCoroutine(WaitAndLoadScene("SampleScene"));
    }

    public void BackToMenu()
    {
        if (_endingSceneTransition != null)
            _endingSceneTransition.SetActive(true);

        StartCoroutine(WaitAndLoadScene("MainMenu"));
    }

    public void InspectorWorld()
    {
        if (_endingSceneTransition != null)
            _endingSceneTransition.SetActive(true);

        string buttonText = inspectorButton.GetComponentInChildren<TextMeshProUGUI>().text;
        StartCoroutine(WaitAndLoadScene(buttonText));
    }

    IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
