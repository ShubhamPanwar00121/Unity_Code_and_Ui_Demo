using UnityEngine;

public class RotateContiniously : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3 (0f, 0f, 1f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
