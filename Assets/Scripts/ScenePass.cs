using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScenePass : MonoBehaviour
{
    Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(ChangeScene);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Scene10");
    }
}
