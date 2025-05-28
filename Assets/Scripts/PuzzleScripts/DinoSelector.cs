using UnityEngine;
using UnityEngine.SceneManagement;

public class DinoSelector : MonoBehaviour
{
    public void SetDino()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SelectedDino.selectedDinoName = currentSceneName;
    }
}