using UnityEngine;

public class PlayerItemContoller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject targetObj = collision.gameObject;
        if (targetObj.tag == "Item")
        {
            targetObj.SetActive(false);

            Debug.Log("æ∆¿Ã≈€ »πµÊ!!");

            GameManager.Instance.AddItem("∆˜º«");
        }
    }
}
