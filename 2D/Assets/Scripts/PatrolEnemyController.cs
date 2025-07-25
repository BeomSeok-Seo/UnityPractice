using System.Collections;
using UnityEngine;

public class PatrolEnemyController : EnemyController
{
    float direction = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 이동
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        animator.SetFloat("Speed", speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurnPoint"))
        {
            // 방향 전환
            direction *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

}
