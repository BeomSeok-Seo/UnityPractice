using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject UIObject;
    private bool activeUI = false;

    private EnemySpawnController enemySpawner;
    private TMP_Text WaveUI;

    private int currentStage = 0;

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

        enemySpawner = GameObject.Find("SpawnPoints")?.GetComponent<EnemySpawnController>();
        WaveUI = GameObject.Find("UI").transform.Find("WaveUI").GetComponent<TMP_Text>();

        GoNextStage();
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

    public void GoNextStage()
    {
        currentStage++;

        StartCoroutine(WaveUIAnimation());


        StartCoroutine(WaveStart(currentStage));
    }

    private IEnumerator WaveStart(int stage)
    {
        yield return new WaitForSeconds(2.0f);
        
        enemySpawner.SpawnEnemies( 5 + (3 * stage));
    }

    private IEnumerator WaveUIAnimation()
    {
        float animationDuration = 2.5f; // �ִϸ��̼� �� �ð�
        float startScale = 2.5f; // ������ ���� ũ��
        float endScale = 1.0f; // ���� ũ��

        float elapsedTime = 0f;

        WaveUI.text = "Wave " + currentStage;

        WaveUI.rectTransform.localScale = new Vector3(startScale, startScale, startScale);

        // �ִϸ��̼� ����
        while (elapsedTime < animationDuration)
        {
            // Lerp �Լ��� ����Ͽ� ���� ũ�� ���
            float currentScale = Mathf.Lerp(startScale, endScale, elapsedTime / animationDuration);

            // �ؽ�Ʈ�� ũ�� ������Ʈ
            WaveUI.rectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);

            // ������ ���
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ִϸ��̼� ���� �� ���� ũ��� ����
        WaveUI.rectTransform.localScale = new Vector3(endScale, endScale, endScale);
    }
}
