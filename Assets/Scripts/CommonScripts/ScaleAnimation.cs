using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] private float m_Scale = 1.2f, scaleSpeed = 0.1f;
    private bool increasing = true;

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        Vector3 currentScale = transform.localScale;

        if (increasing)
        {
            currentScale += Vector3.one * scaleSpeed * Time.deltaTime;

            if (currentScale.x >= m_Scale)
            {
                currentScale = Vector3.one * m_Scale;
                increasing = false;
            }
        }
        else
        {
            currentScale -= Vector3.one * scaleSpeed * Time.deltaTime;

            if (currentScale.x <= 1f)
            {
                currentScale = Vector3.one;
                increasing = true;
            }
        }

        transform.localScale = currentScale;
    }

    private void OnDisable()
    {
       transform.localScale = Vector3.one;
    }
}
