using UnityEngine;

[ExecuteAlways]
public class CanvasAdjuster : MonoBehaviour
{
    public float HeightFactor = 1.0f;

    private void OnEnable()
    {
        AdjustCanvas();
    }

    private void OnValidate()
    {
        AdjustCanvas();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            AdjustCanvas();
        }
#endif
    }

    private void AdjustCanvas()
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (rect == null)
            return;

        float panelHeight = rect.sizeDelta.y;
        float hRatio = (Screen.height / (float)Screen.width);
        float yPos = Mathf.Pow(hRatio, 7) * HeightFactor;

        rect.anchoredPosition = new Vector2(0, -(panelHeight/2 + yPos));
    }
}