using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material skyboxMaterialPrefab;
    public float rotationSpeed = 1f;

    private Material runtimeSkybox;

    void Start()
    {
        // Buat instansi material dari prefab untuk menghindari mengubah asset asli
        runtimeSkybox = Instantiate(skyboxMaterialPrefab);
        RenderSettings.skybox = runtimeSkybox;
    }

    void Update()
    {
        if (runtimeSkybox != null)
        {
            float currentRotation = runtimeSkybox.GetFloat("_Rotation");
            runtimeSkybox.SetFloat("_Rotation", currentRotation + rotationSpeed * Time.deltaTime);
        }
    }
}
