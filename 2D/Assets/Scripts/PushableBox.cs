using UnityEngine;

public class PushableBox : MonoBehaviour
{
    [Range(0f, 1f)]
    public float decelerationRateX = 0.95f; // X축 감속률
    public float stopThresholdX = 0.05f; // X축 정지 임계값

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 currentVelocity = rb.linearVelocity; // 현재 속도를 가져옵니다.

        // X축 속도에 대해서만 감속 로직 적용
        if (Mathf.Abs(currentVelocity.x) > stopThresholdX) // X축 속도의 절댓값이 임계값보다 크면
        {
            // X축 속도에 감속률을 곱하여 속도 줄이기
            currentVelocity.x *= decelerationRateX;

            // X축 속도가 임계값 이하로 떨어지면 완전히 정지
            if (Mathf.Abs(currentVelocity.x) <= stopThresholdX)
            {
                currentVelocity.x = 0; // X축 속도를 0으로 설정하여 완전히 멈춤
            }
        }
        else
        {
            // 이미 X축이 정지 상태라면 0으로 유지 (정확한 정지를 위해)
            currentVelocity.x = 0;
        }

        // Y축 속도는 그대로 유지하면서 새로운 속도 적용
        rb.linearVelocity = currentVelocity;
    }
}
