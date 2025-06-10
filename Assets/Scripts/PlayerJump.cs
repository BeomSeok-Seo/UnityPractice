using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            //rb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
            //rb.AddForce(Vector3.up * 5f, ForceMode.Force);
            //rb.AddForce(Vector3.up * 5f, ForceMode.Acceleration);
        }
    }
}
