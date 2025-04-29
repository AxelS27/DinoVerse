using UnityEngine;

public class DinosaurAutoRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // Kecepatan rotasi dalam derajat per detik

    // Update is called once per frame
    void Update()
    {
        // Rotasi dinosaurus di sekitar sumbu Y (horizontal)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World); // Rotate terus menerus
    }
}
