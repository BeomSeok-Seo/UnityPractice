using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject SettingUI;
    private bool activeUI = false;

    // ���̺�, ���� ����
    private EnemySpawnController enemySpawner;
    private TMP_Text WaveUI;
    private int currentStage = 0;

    // �޺� ����
    private TMP_Text ComboUI;
    private float comboMaxTime = 3f;
    private int comboCount = 0;
    private float comboTimer = 0f;

    public bool waveStartWait = false;

    CinemachineCamera cineCamMain;
    CinemachineCamera cineCam1;
    CinemachineCamera cineCam2;
    CinemachineCamera cineCam3;
    Camera mainCamera;
    Camera WeaponCamera;
    CanvasGroup UICanvas;

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
        SettingUI = GameObject.Find("SettingUI");
        SettingUI.SetActive(activeUI);

        enemySpawner = GameObject.Find("SpawnPoints")?.GetComponent<EnemySpawnController>();
        WaveUI = GameObject.Find("UI").transform.Find("WaveUI").GetComponent<TMP_Text>();

        ComboUI = GameObject.Find("UI").transform.Find("ComboUI").GetComponent<TMP_Text>();

        cineCamMain = GameObject.Find("CinemachineCameraMain").GetComponent<CinemachineCamera>();
        cineCam1 = GameObject.Find("CinemachineCamera1").GetComponent<CinemachineCamera>();
        cineCam2 = GameObject.Find("CinemachineCamera2").GetComponent<CinemachineCamera>();
        cineCam3 = GameObject.Find("CinemachineCamera3").GetComponent<CinemachineCamera>();

        mainCamera = Camera.main;
        WeaponCamera = GameObject.Find("Weapon Camera").GetComponent<Camera>();
        UICanvas = GameObject.Find("UI").GetComponent<CanvasGroup>();

        GoNextStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activeUI = !activeUI;
            SettingUI.SetActive(activeUI);
        }
    }

    public void GoNextStage()
    {
        currentStage++;



        StartCoroutine(WaveStart(currentStage));

        StartCoroutine(WaveUIAnimation());
    }

    private IEnumerator WaveStart(int stage)
    {
        int cullingMask = WeaponCamera.cullingMask;
        WeaponCamera.cullingMask = 0;

        UICanvas.alpha = 0;

        waveStartWait = true;

        cineCam3.Priority = 3;
        cineCam2.Priority = 2;
        cineCam1.Priority = 1;

        yield return new WaitForSeconds(1.0f);

        enemySpawner.SpawnEnemies( 5 + (3 * stage));

        cineCam3.Priority = -1;
        yield return new WaitForSeconds(1.0f);

        cineCam2.Priority = -1;

        yield return new WaitForSeconds(1.0f);

        cineCam1.Priority = -1;

        yield return new WaitForSeconds(2.0f);

        WeaponCamera.cullingMask = cullingMask;

        UICanvas.alpha = 1;

        waveStartWait = false;

    }

    private IEnumerator WaveUIAnimation()
    {
        float animationDuration = 2.5f; // �ִϸ��̼� �� �ð�
        float startScale = 2.5f; // ������ ���� ũ��
        float endScale = 1.0f; // ���� ũ��

        float elapsedTime = 0f;

        while (waveStartWait)
        {
            yield return null;
        }

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

    public void ComboEnable()
    {
        comboCount++;
        comboTimer += comboMaxTime;

        if (comboCount == 1)
        {
            ComboUI.gameObject.SetActive(true);
            ComboUI.text = "Combo " + comboCount;

            StartCoroutine(ComboCounter());
        }
        else
        {
            ComboUI.text = "Combo " + comboCount;
        }
    }

    private IEnumerator ComboCounter()
    {
        float time = 0f;

        while (time < comboTimer)
        {
            time += Time.deltaTime;
            yield return null;
        }

        comboTimer = 0f;
        comboCount = 0;

        ComboUI.gameObject.SetActive(false);
    }
}
