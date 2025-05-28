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
        if (Vector3.Distance(dino.transform.position, crown.transform.position) < 1f)
        {
            HandleWin();
        }
    }

    void HandleWin()
    {
        Debug.Log("Player Win!");

        winSound.Play();
        Debug.Log("Dimainkan");

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
