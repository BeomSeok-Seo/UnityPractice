using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float speed = 3f;
    protected float hitForce = 4f;

    protected bool hitFlag = false;
    protected int hitCount = 0;

    protected Animator animator;
    protected Collider2D colliderBox;
    protected Rigidbody2D rb;

    protected GameObject player;
    protected PlayerController playerController;

    protected SpriteRenderer spriteRenderer;

    protected int healthPoint = 20;

    protected bool isDeath = false;

    private Vector2 originVelocity;

    ContactPoint2D[] contacts = new ContactPoint2D[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        animator = GetComponent<Animator>();
        colliderBox = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        hitFlag = false;
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

    public IEnumerator Death()
    {
        if (isDeath == false)
        {
            isDeath = true;

            // ���� 100�� �߰�
            GameManager.Instance.IncreasScore(100);

            // �ִϸ��̼� Death�� ���
            animator.SetTrigger("Death");

            // ��ġ�� ������Ŵ
            speed = 0;

            // �� �̻� �������� ��ȣ�ۿ��� ���� ����
            colliderBox.enabled = false;
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

    public void Hit(int sign, int damage)
    {
        StartCoroutine(HitCoroutine(sign, damage));
    }

    private IEnumerator HitCoroutine(int sign, int damage)
    {
        if (hitCount == 0)
        {
            // ���� �ӵ��� ������ ����
            originVelocity = rb.linearVelocity;
        }

        // hit ��� ���� ���϶����� hit == true
        hitFlag = true;
        hitCount++;
        // hit ����� �з������� �ӵ��� ����
        rb.linearVelocity = new Vector2((hitForce * sign) / 2, hitForce);

        // �� hit ��� �ð�
        float hitDuration = 0.65f;
        float timer = 0f;
        while (timer < hitDuration)
        {
            timer += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }


        // hit ��� ����
        hitFlag = false;
        hitCount--;

        if (hitCount == 0)
        {
            // hit ����� ������ ���� �ӵ��� �ǵ���
            rb.linearVelocity = originVelocity;
        }

        // ü���� ��´�
        healthPoint -= damage;

        if (healthPoint <= 0)
        {
            // ü���� 0�� �Ǹ� ���
            StartCoroutine(Death());
        }
    }
}
