using UnityEngine;
using UnityEngine.SceneManagement;

public class DinoSelector : MonoBehaviour
{
    public void SetDino()
    {
        // Ambil nama scene aktif saat ini
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Simpan ke static class
        SelectedDino.selectedDinoName = currentSceneName;
    }
}