using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject doorPrefab;

    public int score = 0;

    TMP_Text scoreText;

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
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreasScore(int score)
    {
        this.score += score;
        if (doorPrefab != null && this.score >= 50)
        {
            Instantiate(doorPrefab);
        }
        scoreText.text = $"Score : {this.score}";
    }
}
