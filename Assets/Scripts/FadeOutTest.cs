using System.Collections;
using UnityEngine;

public class FadeOutTest : MonoBehaviour
{
    CanvasGroup panel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel = GetComponent<CanvasGroup>();

        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(panel, 0f, 1f));
    }

    public IEnumerator Fade(CanvasGroup cg, float from, float to)
    {
        float t = 0f;
        float duringTime = 5f;
        while (t < duringTime)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duringTime);

            yield return null;
        }

        cg.interactable = to > 0.5f;
        cg.blocksRaycasts = to > 0.5;
    }
}
