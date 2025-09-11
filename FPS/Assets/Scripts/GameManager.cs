using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject UIObject;
    private bool activeUI = false;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIObject = GameObject.Find("SettingUI");
        UIObject.SetActive(activeUI);

        // To-Do : 시작할 때 몹을 스폰하도록 넣어보기
        // 그 이후에는 웨이브개념 추가
        // 종료까지

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activeUI = !activeUI;
            UIObject.SetActive(activeUI);
        }
    }

}
