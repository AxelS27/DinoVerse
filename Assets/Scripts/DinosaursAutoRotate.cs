using UnityEngine;

public class DinosaurAutoRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
