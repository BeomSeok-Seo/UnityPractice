using UnityEngine;

public class BulletImpulse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForce(Vector3.forward * 50f, ForceMode.Impulse);

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
