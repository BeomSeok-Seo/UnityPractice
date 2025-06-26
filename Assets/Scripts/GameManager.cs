using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float score = 100f;
    public float Score { 
        get 
        {
            return score;
        } 
        set
        {
            score = value;
            GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>().text = "Score : " + score;
        } 
    }
    public List<GameObject> items = new List<GameObject>();

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
        Score = PlayerPrefs.GetFloat("Score");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public GameObject FindItemByName(string name)
    {
        return items.Find((prod) => prod.name == name);
    }

    public void PlusScore(float scoere)
    {
        Score += scoere;

        PlayerPrefs.SetFloat("Score", Score);
    }
}
