using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject doorObject;
    public GameObject clearPanel;

    public int score = 0;
    private int clearScore = 250;

    public bool stageClear = false;

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

    private void OpenNextStage()
    {
        doorObject.GetComponent<DoorController>().DoorOpen();
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
}
