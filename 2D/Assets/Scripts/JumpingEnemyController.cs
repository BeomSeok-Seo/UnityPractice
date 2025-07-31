using UnityEngine;

public class JumpingEnemyController : EnemyController
{
    // ���� �ð� ��
    private float jumpInterval = 2f;
    // ������ ��
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
        // ���Ϳ��� ��Į�� ���� ���� ���⸸ ����
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // �����̳� �������̳� �Ǵ�
        float direction = Mathf.Sign(directionToPlayer.x);

        // ������ ���� ������ ����
        spriteRenderer.flipX = direction > 0 ? false : true;

    }

    private void FixedUpdate()
    {
        if (true)
        {
            // ���� �ð� üũ
            if (jumpTimer < jumpInterval)
            {
                // ���� �� Ÿ�̹��� �ƴϸ� �ð��� ���ϸ鼭 ��ٸ�
                jumpTimer += Time.deltaTime;
            }
            else
            {
                // ���� �� Ÿ�̹�

                // ���� �ִϸ��̼� Ʈ����
                animator.SetTrigger("Jump");

                // Y������ ���� jumpForce ��ŭ�� ������
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // ���� Ÿ�̸� �ʱ�ȭ
                jumpTimer = 0f;
            } 
        }

        // ���� ��Ҵ��� üũ
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        // ���� �����ִϸ��̼� Ʈ����
        animator.SetBool("IsGrounded", isGrounded);
    }
}
