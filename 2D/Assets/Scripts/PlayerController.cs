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
        // 씬 뷰에서 이 오브젝트가 선택되었을 때만 기즈모를 그림
        // Gizmos.color는 기즈모의 색상을 설정합니다.
        // 공격 범위가 잘 보이도록 반투명한 빨간색으로 설정해볼게요.
        Gizmos.color = new Color(1, 0, 0, 0.5f); // 빨강, 반투명

        // BoxCast와 OverlapBox의 중심점과 크기를 계산합니다.
        // Gizmos.DrawWireCube나 DrawWireSphere는 월드 좌표를 사용합니다.
        // 따라서 transform.position에 attackOffset을 더해주어야 합니다.
        Vector2 boxCenter = (Vector2)transform.position + attackOffset;

        // Gizmos.matrix를 사용하여 박스에 회전을 적용합니다.
        // 먼저 현재 Gizmos.matrix를 저장해두고, 함수 종료 시 다시 복원하는 것이 좋습니다.
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, attackAngle), Vector3.one);

        // 박스 형태를 그립니다.
        // Gizmos.matrix에 이미 위치와 회전이 적용되었으므로, DrawWireCube에는 (0,0,0)과 크기만 전달합니다.
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(attackSize.x, attackSize.y, 0));

        // Gizmos.matrix를 원래대로 복원합니다.
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

                // hit Box의 중심 계산
                Vector2 boxCenter = (Vector2)transform.position + attackOffset;

                // hit Box와 겹치는 Collider2D 목록 가져오기
                Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, attackSize, 0);
                foreach (Collider2D hit in hits)
                {
                    // 그 중에 tag가 Enemy인 것 들만
                    if (hit.CompareTag("Enemy") && isHit == false)
                    {
                        // 모션 중 한번 맞으면 해당 모션 진행 중에는 중복 연산 하지 않음
                        isHit = true;

                        // 10 데미지 만큼만 체력을 깎는다
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
