using UnityEngine;
using UnityEngine.UI;

public class WarningController : MonoBehaviour
{
    private Image panelImage;

    private Color startColor;
    private Color endColor;

    private float fadeDuration = 1.0f; 
    private float elapsedTime = 0f;
    private bool isFadingIn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            startColor = panelImage.color;
            endColor = startColor;
            endColor.a = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / fadeDuration;

        if (isFadingIn)
        {
            panelImage.color = Color.Lerp(endColor, startColor, t);
        }
        else
        {
            panelImage.color = Color.Lerp(startColor, endColor, t);
        }

        if (t >= 1.0f)
        {
            isFadingIn = !isFadingIn;
            elapsedTime = 0f;
        }
    }
}
