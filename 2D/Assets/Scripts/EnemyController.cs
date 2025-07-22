using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float direction = 1f;
    float speed = 3f;

    Animator animator;
    SpriteRenderer SpriteRenderer;

    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    PlayerController playerController;

    ContactPoint2D[] contacts = new ContactPoint2D[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        SpriteRenderer = GetComponent<SpriteRenderer>();

        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // �̵�
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        animator.SetFloat("Speed", speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurnPoint"))
        {
            // ���� ��ȯ
            direction *= -1;
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("Player!!");
            int numContacts = collision.GetContacts(contacts);

            if (numContacts > 0)
            {
                Vector2 contactPoint = contacts[0].point;
                Vector2 center = transform.position;

                // Normalized�� ��Į�� ���� ����
                Vector2 direction = (contactPoint - center).normalized;

                // y�� ũ�Ⱑ x�� ũ�⺸�� ũ�� ������ �浹�̶�� ��
                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                {
                    // ������ �浹
                    if (direction.y > 0)
                    {
                        // �÷��̾��� ª�� ����
                        playerController.HalfJump();

                        // Death �ڷ�ƾ ȣ��
                        StartCoroutine(Death());

                        // ���� 100�� �߰�
                        GameManager.Instance.IncreasScore(100);
                    }
                }
                // ������ �浹
                else
                {
                    playerController.Respawn();
                }
            }
        }
    }
    private IEnumerator Death()
    {
        // �ִϸ��̼� Death�� ���
        animator.SetTrigger("Death");

        // ��ġ�� ������Ŵ
        speed = 0;

        // �� �̻� �������� ��ȣ�ۿ��� ���� ����
        boxCollider.enabled = false;
        rb.simulated = false;

        // �ִϸ��̼��� ��� �Ϸ� �� �� ���� ��ٸ�
        float animationDuration = 0.7f;
        float timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        Destroy(gameObject);
    }
}
