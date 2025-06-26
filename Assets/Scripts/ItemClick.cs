using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    GameObject inventoryUI;
    Transform inventoryCanvas;

    GameObject currentDragObj = null;
    int currentDragIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryUI = GameObject.FindGameObjectWithTag("InventoryUI");
        inventoryCanvas = inventoryUI.transform.parent.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        GameManager.Instance.PlusScore(100);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        string dragString = GetComponentInChildren<TMP_Text>().text;

        currentDragObj = GameManager.Instance.FindItemByName(dragString);

        currentDragIndex = GameManager.Instance.items.IndexOf(currentDragObj);

        transform.SetParent(inventoryCanvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentDragObj.SetActive(true);
        currentDragObj.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);

        currentDragObj = null;
        currentDragIndex = -1;

        Destroy(gameObject);
    }
}
