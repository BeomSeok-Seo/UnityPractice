using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class UIManager : MonoBehaviour
{
    TMP_Text textMP;
    GameObject player;
    GameObject mainCameraObj;
    Camera camera;
    PlayerHealthPoint playerHealthPoint;

    UnityEngine.UI.Image image;
    float maxImageWidth = 240;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mainCameraObj = GameObject.FindWithTag("MainCamera");
        camera = mainCameraObj.GetComponent<Camera>();
        playerHealthPoint = player.GetComponent<PlayerHealthPoint>();

        image = GetComponentInChildren<UnityEngine.UI.Image>();
        textMP = image.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        image.transform.position = camera.WorldToScreenPoint(player.transform.position + new Vector3(0, 2, 0));

        image.fillAmount = playerHealthPoint.health / playerHealthPoint.maxHealth;
        //float width = ;
        //image.rectTransform.sizeDelta = new Vector2 (width, 20f);
        //image.transform.position -= new Vector3(maxImageWidth - width,0,0);
        //Debug.Log((playerHealthPoint.health * 4) / maxImageWidth);

        textMP.text = $"{playerHealthPoint.health}";
    }
}
