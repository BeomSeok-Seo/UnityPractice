using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    float moveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed;

        rb.velocity = new Vector2(move, rb.velocity.y);

        //Debug.Log(move);
        animator.SetFloat("Speed", Mathf.Abs(move));
    }
}
