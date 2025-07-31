using UnityEngine;

public class TrackingEnemyController : EnemyController
{
    private float detectionRange = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        healthPoint = 40;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // �÷��̾���� �Ÿ� ���
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        // �Ÿ��� ���غ��� ������
        if (distancePlayer < detectionRange && hitFlag == false)
        {
            // ���Ϳ��� ��Į�� ���� ���� ���⸸ ����
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            // �����̳� �������̳� �Ǵ�
            float direction = Mathf.Sign(directionToPlayer.x);

            // �̵�
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            // ������ ���� ������ ����
            spriteRenderer.flipX = direction > 0 ? false : true;

            // �޸��� �ִϸ��̼� Ʈ����
            animator.SetFloat("Speed", speed);
        }
        else
        {
            // ���߱� �ִϸ��̼� Ʈ����
            animator.SetFloat("Speed", 0);
        }
    }
}
