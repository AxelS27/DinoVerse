using UnityEngine;

public class DinoSpawner : MonoBehaviour
{
    public GameObject pteranodonPrefab;
    public GameObject trexPrefab;
    public GameObject spinosaurusPrefab;
    public GameObject stegosaurusPrefab;
    public GameObject triceratopsPrefab;
    public GameObject ankylosaurusPrefab;
    public GameObject mossasaurusPrefab;
    public GameObject komododragonPrefab;
    public GameObject megalodonPrefab;

    public Transform player;

    void Start()
    {
        string dinoName = SelectedDino.selectedDinoName;

        if (string.IsNullOrEmpty(dinoName))
        {
            dinoName = "T-REX";
        }

        dinoName = dinoName.ToUpper();

        GameObject dinoToSpawn = null;

        switch (dinoName)
        {
            case "PTERANODON":
                dinoToSpawn = Instantiate(pteranodonPrefab, player);
                break;
            case "T-REX":
                dinoToSpawn = Instantiate(trexPrefab, player);
                break;
            case "SPINOSAURUS":
                dinoToSpawn = Instantiate(spinosaurusPrefab, player);
                break;
            case "STEGOSAURUS":
                dinoToSpawn = Instantiate(stegosaurusPrefab, player);
                break;
            case "TRICERATOPS":
                dinoToSpawn = Instantiate(triceratopsPrefab, player);
                break;
            case "ANKYLOSAURUS":
                dinoToSpawn = Instantiate(ankylosaurusPrefab, player);
                break;
            case "MOSSASAURUS":
                dinoToSpawn = Instantiate(mossasaurusPrefab, player);
                break;
            case "KOMODODRAGON":
                dinoToSpawn = Instantiate(komododragonPrefab, player);
                break;
            case "MEGALODON":
                dinoToSpawn = Instantiate(megalodonPrefab, player);
                break;
            default:
                Debug.LogWarning("Nama dino tidak dikenali: " + dinoName);
                break;
        }

        if (dinoToSpawn != null)
        {
            dinoToSpawn.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            dinoToSpawn.transform.localPosition = Vector3.zero;

            MonoBehaviour[] scripts = dinoToSpawn.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                Destroy(script);
            }
        }
    }
}
