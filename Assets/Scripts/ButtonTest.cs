using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    Button button;
    Text text;
    TMP_Text textMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        textMP = GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(ChangeText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        Debug.Log("��ư�� ���Ƚ��ϴ�.");
    }

    public void ChangeText()
    {
        text.text = "��ư Ŭ�� ��!!";
        //textMP.text = "��ư Ŭ�� ��!!";
        //new Color((float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255);
    }
}
