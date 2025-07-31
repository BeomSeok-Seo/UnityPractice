using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletObject;

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

    Vector2 attackInitOffset = new Vector2(1.2f, 0.78f);
    Vector2 attackOffset;
    Vector2 attackSize = new Vector2(3.3f, 0.5f);
    float attackAngle = 0f;

    int viewSign = 0;

    bool isHitMotion = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();

        SpriteRenderer = GetComponent<SpriteRenderer>();

        groundLayer = LayerMask.GetMask("Platform");
        usableLayer = LayerMask.GetMask("Usable");

        attackOffset = attackInitOffset;

        isHitMotion = false;
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
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isGrounded)
            {
                StartCoroutine(StaffAttack());

                //OnDrawGizmos();
                animator.SetTrigger("StaffAttack");
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("IsGun", true);
            Instantiate(bulletObject, transform);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetBool("IsGun", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �� �信�� �� ������Ʈ�� ���õǾ��� ���� ����� �׸�
        // Gizmos.color�� ������� ������ �����մϴ�.
        // ���� ������ �� ���̵��� �������� ���������� �����غ��Կ�.
        Gizmos.color = new Color(1, 0, 0, 0.5f); // ����, ������

        // BoxCast�� OverlapBox�� �߽����� ũ�⸦ ����մϴ�.
        // Gizmos.DrawWireCube�� DrawWireSphere�� ���� ��ǥ�� ����մϴ�.
        // ���� transform.position�� attackOffset�� �����־�� �մϴ�.
        Vector2 boxCenter = (Vector2)transform.position + attackOffset;

        // Gizmos.matrix�� ����Ͽ� �ڽ��� ȸ���� �����մϴ�.
        // ���� ���� Gizmos.matrix�� �����صΰ�, �Լ� ���� �� �ٽ� �����ϴ� ���� �����ϴ�.
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, attackAngle), Vector3.one);

        // �ڽ� ���¸� �׸��ϴ�.
        // Gizmos.matrix�� �̹� ��ġ�� ȸ���� ����Ǿ����Ƿ�, DrawWireCube���� (0,0,0)�� ũ�⸸ �����մϴ�.
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(attackSize.x, attackSize.y, 0));

        // Gizmos.matrix�� ������� �����մϴ�.
        Gizmos.matrix = originalMatrix;
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

        if (isHitMotion == true)
        {
            move = 0;
        }

        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        int sign = Math.Sign(move);

        bool isWall = Physics2D.Raycast(transform.position, Vector2.left * (-sign), 0.3f, usableLayer);
        animator.SetBool("PushBox", isWall);
        //Debug.Log(Math.Sign(move));

        if (move < 0)
        {
            viewSign = sign;

            SpriteRenderer.flipX = true;
            attackOffset = new Vector2(attackInitOffset.x * sign, attackInitOffset.y);
        }
        else if (move > 0)
        {
            viewSign = sign;

            SpriteRenderer.flipX = false;
            attackOffset = new Vector2(attackInitOffset.x * sign, attackInitOffset.y);
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

    private IEnumerator StaffAttack()
    {
        float animationDuration = 0.41f; // 5 / 12
        float delayTime = 0.16f; // 2 / 12
        float timer = 0;

        bool isHit = false;

        while (timer < animationDuration)
        {
            if (delayTime < timer)
            {
                isHitMotion = true;

                // hit Box�� �߽� ���
                Vector2 boxCenter = (Vector2)transform.position + attackOffset;

                // hit Box�� ��ġ�� Collider2D ��� ��������
                Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, attackSize, 0);
                foreach (Collider2D hit in hits)
                {
                    // �� �߿� tag�� Enemy�� �� �鸸
                    if (hit.CompareTag("Enemy") && isHit == false)
                    {
                        // ��� �� �ѹ� ������ �ش� ��� ���� �߿��� �ߺ� ���� ���� ����
                        isHit = true;

                        // 10 ������ ��ŭ�� ü���� ��´�
                        hit.GetComponent<EnemyController>().Hit(viewSign, 10);

                        //StartCoroutine(hit.GetComponent<EnemyController>().Death());
                    }
                }
            }

            timer += Time.deltaTime;

            yield return null;
        }

        isHitMotion = false;
    }
}
