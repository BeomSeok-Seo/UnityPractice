using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float speed = 3f;

    protected Animator animator;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb;

    protected GameObject player;
    protected PlayerController playerController;

    protected SpriteRenderer spriteRenderer;

    ContactPoint2D[] contacts = new ContactPoint2D[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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

    protected IEnumerator Death()
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
