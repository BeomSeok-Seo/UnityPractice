using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject doorObject;
    public GameObject clearPanel;

    private GameObject playerHealthUI;
    private CanvasGroup gameOverGroup;

    public int score = 0;
    private int clearScore = 250;

    private int playerHealth = 3;

    public bool stageClear = false;
    public bool gameOver = false;

    TMP_Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        playerHealthUI = transform.Find("UICanvas")?.Find("PlayerHealth")?.gameObject;
        GameObject gameOverUI = transform.Find("GameOver")?.gameObject;
        gameOverGroup = gameOverUI.GetComponent<CanvasGroup>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePlayerHealth(int damage)
    {
        playerHealth += damage;

        Transform healthObjTf;
        Transform hpTf;

        for (int i = 0; i < playerHealthUI.transform.childCount; i++)
        {
            healthObjTf = playerHealthUI.transform.GetChild(i);

            hpTf = healthObjTf.GetChild(0);

            if (i < playerHealth)
            {
                hpTf.gameObject.SetActive(true);
            }
            else
            {
                hpTf.gameObject.SetActive(false);
            }
        }

        if (playerHealth == 0)
        {
            GameOver();
        }
    }

    public void IncreasScore(int score)
    {
        this.score += score;
        if (doorObject != null && this.score >= clearScore)
        {
            //Instantiate(doorObject);
            OpenNextStage();
        }
        scoreText.text = $"Score : {this.score}";
    }

    public void CheckStageClear()
    {
        if (stageClear)
        {
            clearPanel.transform.DOLocalMove(new Vector2(0, 0), 2f)
                .SetEase(Ease.InOutQuad);
        }
    }

    public void GoNextStage()
    {
        Debug.Log("Next!");
        SceneManager.LoadScene("Stage2");
    }

    public void GameOver()
    {
        gameOver = true;

        StartCoroutine(GameOverFadeIn());
    }

    private IEnumerator GameOverFadeIn()
    {
        float fadeDuration = 2f;

        float currentTime = 0f;
        float startAlpha = gameOverGroup.alpha;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startAlpha, 1f, currentTime / fadeDuration);
            gameOverGroup.alpha = newAlpha;
            yield return null; 
        }

        gameOverGroup.alpha = 1f;
    }

    private void OpenNextStage()
    {
        doorObject.GetComponent<DoorController>().DoorOpen();
    }
}
