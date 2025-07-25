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

                // Normalized로 스칼라 값을 없앰
                Vector2 direction = (contactPoint - center).normalized;

                // y의 크기가 x의 크기보다 크면 세로쪽 충돌이라는 뜻
                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                {
                    // 위에서 충돌
                    if (direction.y > 0)
                    {
                        // 플레이어의 짧은 점프
                        playerController.HalfJump();

                        // Death 코루틴 호출
                        StartCoroutine(Death());

                        // 점수 100점 추가
                        GameManager.Instance.IncreasScore(100);
                    }
                }
                // 가로쪽 충돌
                else
                {
                    playerController.Respawn();
                }
            }
        }
    }

    protected IEnumerator Death()
    {
        // 애니메이션 Death를 재생
        animator.SetTrigger("Death");

        // 위치를 고정시킴
        speed = 0;

        // 더 이상 물리적인 상호작용을 하지 않음
        boxCollider.enabled = false;
        rb.simulated = false;

        // 애니메이션이 재생 완료 될 때 까지 기다림
        float animationDuration = 0.7f;
        float timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        Destroy(gameObject);
    }
}
