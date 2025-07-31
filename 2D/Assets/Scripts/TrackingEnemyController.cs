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
        // 플레이어와의 거리 재기
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        // 거리가 기준보다 가까우면
        if (distancePlayer < detectionRange && hitFlag == false)
        {
            // 벡터에서 스칼라 값을 빼고 방향만 본다
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            // 왼쪽이냐 오른쪽이냐 판단
            float direction = Mathf.Sign(directionToPlayer.x);

            // 이동
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            // 렌더링 왼쪽 오른쪽 구분
            spriteRenderer.flipX = direction > 0 ? false : true;

            // 달리기 애니메이션 트리거
            animator.SetFloat("Speed", speed);
        }
        else
        {
            // 멈추기 애니메이션 트리거
            animator.SetFloat("Speed", 0);
        }
    }
}
