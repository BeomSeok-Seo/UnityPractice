using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private Rigidbody rb;

    private MeshRenderer MeshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + "과 충돌!");

        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        //int random = Random.Range(0, 255);
        //Debug.Log(random);

        MeshRenderer.material.color = new Color((float)Random.Range(0, 255)/255 , (float)Random.Range(0, 255)/255, (float)Random.Range(0, 255)/255);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "이(가) 트리거 진입!");
    }
}
