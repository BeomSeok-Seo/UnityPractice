using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    Rigidbody rb;
    MeshRenderer meshRenderer;

    Slider slider;

    float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();

        slider = FindFirstObjectByType<Slider>();

        slider.onValueChanged.AddListener((float value) =>
        {
            speed = value;
        });

        speed = slider.value;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, 1, 0), -1f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 1, 0), 1f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 5f, 0), ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            meshRenderer.material.color = new Color((float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255);
        }
    }
}
