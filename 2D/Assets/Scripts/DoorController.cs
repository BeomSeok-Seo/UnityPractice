using DG.Tweening;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorOpen()
    {
        transform.DOMoveY(4.5f, 3f)
            .SetEase(Ease.InOutQuart)
            .OnComplete(() => {
                spriteRenderer.enabled = false;
                GameManager.Instance.stageClear = true;
                });

    }
}
