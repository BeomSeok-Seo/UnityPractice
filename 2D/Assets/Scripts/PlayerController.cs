using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    SpriteRenderer SpriteRenderer;
    float moveSpeed = 5f;

    float jumpForce = 8.8f;
    bool isGrounded = false;
    int groundLayer = -1;
    int usableLayer = -1;

    int jumpCount = 2;

    Vector2 respawnPoint = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();

        SpriteRenderer = GetComponent<SpriteRenderer>();

        groundLayer = LayerMask.GetMask("Platform");
        usableLayer = LayerMask.GetMask("Usable");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) /*&& isGrounded*/)
        {
            jumpCount--;
            if (jumpCount > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.SetTrigger("Jump");
            }
        }
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
    }

    public void HalfJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / 2);
        animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed;

        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);


        bool isWall = Physics2D.Raycast(transform.position, Vector2.left * (-Math.Sign(move)), 0.3f, usableLayer);
        animator.SetBool("PushBox", isWall);
        //Debug.Log(isWall);

        if (move < 0)
        {
            SpriteRenderer.flipX = true;
        }
        else if (move > 0)
        {
            SpriteRenderer.flipX = false;
        }

        //Debug.Log(move);
        animator.SetFloat("Speed", Mathf.Abs(move));

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, usableLayer | groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        if (isGrounded == true)
        {
            jumpCount = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.Instance.IncreasScore(10);

            collision.gameObject.GetComponent<CoinContoller>().StartCoinAnimation();
            //Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DeathZone"))
        {
            Respawn();
        }

        if (collision.CompareTag("StageGoal"))
        {
            GameManager.Instance.CheckStageClear();
        }
    }
}
