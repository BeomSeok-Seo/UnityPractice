using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float maxLifeTime = 3f;
    private float liftTime = 0f;

    private float velocity = 9.5f;

    public int direction = 1;
    Rigidbody2D rb;
    Collider2D colliderBox;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderBox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        rb.linearVelocity = new Vector2(direction * velocity, rb.linearVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        liftTime += Time.deltaTime;

        if (liftTime > maxLifeTime)
        {
            // 사라짐
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            enemy.Hit(direction, 5);

            // 피격 애니메이션 표출 후 사라짐
            StartCoroutine(DestroyBullet());
        }
        else if (collision.transform.CompareTag("Platform"))
        {
            // 피격 애니메이션 표출 후 사라짐
            StartCoroutine(DestroyBullet());
        }
    }

    IEnumerator DestroyBullet()
    {
        rb.linearVelocity = new Vector2(0, 0);

        // 더 이상 물리적인 상호작용을 하지 않음
        colliderBox.enabled = false;
        rb.simulated = false;

        animator.SetTrigger("Destroy");

        float animationDuration = 0.33f;
        float timer = 0;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
