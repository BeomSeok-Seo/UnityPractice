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

    TMP_Text scoreText;

    float jumpForce = 8.8f;
    bool isGrounded = false;
    int groundLayer = -1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        SpriteRenderer = GetComponent<SpriteRenderer>();

        groundLayer = LayerMask.GetMask("Platform");

        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed;

        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

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

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.Instance.score += 10;
            scoreText.text = $"Score : {GameManager.Instance.score}";
            Destroy(collision.gameObject);
        }
    }
}
