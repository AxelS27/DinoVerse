using UnityEngine;

public class DinoCollisionHandler : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crown"))
        {
            gameManager?.HandleWinFromDino(); // Tambahkan fungsi publik di GameManager
        }
    }
}
