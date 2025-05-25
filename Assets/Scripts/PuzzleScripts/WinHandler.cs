using UnityEngine;

public class WinHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject dino;
    public GameObject crown;
    public AudioSource winSound;

    [Header("Random Position Range")]
    public float rangeX = 5f;
    public float rangeZ = 5f;

    private void Start()
    {
        RandomizeCrownPosition();
    }

    private void Update()
    {
        // Cek jarak antar objek, jika overlap maka dianggap menang (bisa diganti pakai collider trigger jika lebih akurat)
        if (Vector3.Distance(dino.transform.position, crown.transform.position) < 1f) // kamu bisa sesuaikan threshold
        {
            HandleWin();
        }
    }

    void HandleWin()
    {
        Debug.Log("Player Win!");

        // Play sound

        winSound.Play();
        Debug.Log("Dimainkan");

        // Reset posisi dino
        var dinoController = dino.GetComponent<DinoController>();
        if (dinoController != null)
        {
            //dinoController.ResetPosition();
        }

        // Random ulang posisi crown
        RandomizeCrownPosition();
    }

    void RandomizeCrownPosition()
    {
        Vector3 newPos = new Vector3(
            Random.Range(-rangeX, rangeX),
            crown.transform.position.y,
            Random.Range(-rangeZ, rangeZ)
        );

        crown.transform.position = newPos;
    }
}
