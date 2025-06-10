using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    float moveSpeed = 5f;
    float jumpForce = 5f;

    float sensitivity = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(transform.forward * speedThreshold * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(-transform.forward * speedThreshold * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(-transform.right * speedThreshold * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(transform.right * speedThreshold * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Rotate(new Vector3(0, 1, 0), -1f);
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    transform.Rotate(new Vector3(0, 1, 0), 1f);
        //}

        //float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        //animator.SetFloat("Speed", speed);

        //Debug.Log(speed);

        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(0, mouseX * sensitivity, 0);

        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("IsGround"))
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Horizontal") * 3f);

        Vector3 moveInput = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //Vector3 move = moveInput * moveSpeed;
        Vector3 move = Input.GetAxis("Vertical") * transform.forward * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        

        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        animator.SetFloat("Speed", speed);

        //Debug.Log(speed);

        Ray ray = new Ray(transform.position, Vector3.down);
        bool grounded = Physics.Raycast(ray, 0.5f);
        animator.SetBool("IsGround", grounded);

        //Debug.Log(grounded);
    }
}
