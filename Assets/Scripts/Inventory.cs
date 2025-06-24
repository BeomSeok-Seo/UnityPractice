using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject buttonPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        //GUIContent gUI = gameObject.GetComponent<GUIContent>();

        List<string> items = GameManager.Instance.items;

        foreach (string item in items)
        {
            Instantiate(buttonPrefab, gameObject.transform);
            TMP_Text text = buttonPrefab.GetComponentInChildren<TMP_Text>();

            text.text = item;
        }
    }
}
