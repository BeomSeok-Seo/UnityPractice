using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    TMP_Text textMP;
    GameObject player;
    GameObject mainCameraObj;
    Camera camera;
    PlayerHealthPoint playerHealthPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMP = GetComponentInChildren<TMP_Text>();
        player = GameObject.FindWithTag("Player");
        mainCameraObj = GameObject.FindWithTag("MainCamera");
        camera = mainCameraObj.GetComponent<Camera>();
        playerHealthPoint = player.GetComponent<PlayerHealthPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        textMP.transform.position = camera.WorldToScreenPoint(player.transform.position + new Vector3(0, 2, 0));

        textMP.text = $"HP : {playerHealthPoint.health} / {playerHealthPoint.maxHealth}";
    }
}
