using UnityEngine;
using System.Collections;

public class DinoController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float rotateAngle = 90f;
    public float speed = 2f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Awake()
    {
        // Simpan posisi dan rotasi awal
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Start()
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    public IEnumerator MoveForward()
    {
        Vector3 target = transform.position + transform.forward * moveDistance;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator MoveBackward()
    {
        Vector3 target = transform.position - transform.forward * moveDistance;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator TurnLeft()
    {
        Quaternion target = Quaternion.Euler(0, transform.eulerAngles.y - rotateAngle, 0);
        while (Quaternion.Angle(transform.rotation, target) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed * 60 * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator TurnRight()
    {
        Quaternion target = Quaternion.Euler(0, transform.eulerAngles.y + rotateAngle, 0);
        while (Quaternion.Angle(transform.rotation, target) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed * 60 * Time.deltaTime);
            yield return null;
        }
    }
}
