using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTest : MonoBehaviour
{
    Animator panelAnimator;
    Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelAnimator = GetComponent<Animator>();
        button = GetComponentInChildren<Button>();

        button.onClick.AddListener(ClosePanel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ClosePanel()
    {
        panelAnimator.SetTrigger("PanelOut");
    }
}
