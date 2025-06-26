using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject buttonPrefab;
    private GameObject invenView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invenView = GameObject.Find("InventoryView");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        if (invenView.activeInHierarchy)
        {
            invenView.SetActive(false);
            return;
        }
        else
        {
            invenView.SetActive(true);
        }
    }

    public void AddItem(string item)
    {
        GameObject obj = Instantiate(buttonPrefab, gameObject.transform);
        TMP_Text text = obj.GetComponentInChildren<TMP_Text>();

        //Debug.Log(item);
        text.text = item;
    }
}
