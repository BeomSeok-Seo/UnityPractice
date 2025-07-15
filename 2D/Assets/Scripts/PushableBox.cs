using UnityEngine;

public class PushableBox : MonoBehaviour
{
    [Range(0f, 1f)]
    public float decelerationRateX = 0.95f; // X�� ���ӷ�
    public float stopThresholdX = 0.05f; // X�� ���� �Ӱ谪

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
        Vector2 currentVelocity = rb.linearVelocity; // ���� �ӵ��� �����ɴϴ�.

        // X�� �ӵ��� ���ؼ��� ���� ���� ����
        if (Mathf.Abs(currentVelocity.x) > stopThresholdX) // X�� �ӵ��� ������ �Ӱ谪���� ũ��
        {
            // X�� �ӵ��� ���ӷ��� ���Ͽ� �ӵ� ���̱�
            currentVelocity.x *= decelerationRateX;

            // X�� �ӵ��� �Ӱ谪 ���Ϸ� �������� ������ ����
            if (Mathf.Abs(currentVelocity.x) <= stopThresholdX)
            {
                currentVelocity.x = 0; // X�� �ӵ��� 0���� �����Ͽ� ������ ����
            }
        }
        else
        {
            // �̹� X���� ���� ���¶�� 0���� ���� (��Ȯ�� ������ ����)
            currentVelocity.x = 0;
        }

        // Y�� �ӵ��� �״�� �����ϸ鼭 ���ο� �ӵ� ����
        rb.linearVelocity = currentVelocity;
    }
}
