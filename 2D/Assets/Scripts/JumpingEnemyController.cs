using UnityEngine;

public class JumpingEnemyController : EnemyController
{
    // 점프 시간 텀
    private float jumpInterval = 2f;
    // 점프의 힘
    private float jumpForce = 7f;

    bool isGrounded = false;
    int groundLayer = -1;

    float jumpTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        healthPoint = 50;

        groundLayer = LayerMask.GetMask("Platform");

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // 벡터에서 스칼라 값을 빼고 방향만 본다
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // 왼쪽이냐 오른쪽이냐 판단
        float direction = Mathf.Sign(directionToPlayer.x);

        // 렌더링 왼쪽 오른쪽 구분
        spriteRenderer.flipX = direction > 0 ? false : true;

    }

    private void FixedUpdate()
    {
        if (true)
        {
            // 점프 시간 체크
            if (jumpTimer < jumpInterval)
            {
                // 점프 할 타이밍이 아니면 시간을 더하면서 기다림
                jumpTimer += Time.deltaTime;
            }
            else
            {
                // 점프 할 타이밍

                // 점프 애니메이션 트리거
                animator.SetTrigger("Jump");

                // Y축으로 점프 jumpForce 만큼의 힘으로
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // 점프 타이머 초기화
                jumpTimer = 0f;
            } 
        }

        // 땅에 닿았는지 체크
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        // 땅에 닿은애니메이션 트리거
        animator.SetBool("IsGrounded", isGrounded);
    }
}
