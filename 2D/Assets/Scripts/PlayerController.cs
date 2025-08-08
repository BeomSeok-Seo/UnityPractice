using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletObject;

    Animator animator;
    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;
    float moveSpeed = 5f;

    float jumpForce = 8.8f;
    int jumpCount = 2;
    bool isGrounded = false;

    bool isCrouch = false;

    float fireCooltime = 0.2f;
    float fireTime = 0;
    bool isFire = false;

    int groundLayer = -1;
    int usableLayer = -1;

    Vector2 respawnPoint = Vector2.zero;

    Vector2 attackInitOffset = new Vector2(1.2f, 0.78f);
    Vector2 attackOffset;
    Vector2 attackSize = new Vector2(3.3f, 0.5f);
    float attackAngle = 0f;
    float frontVertical = 0f;

    int direction = 1;

    bool isHitMotion = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        groundLayer = LayerMask.GetMask("Platform");
        usableLayer = LayerMask.GetMask("Usable");

        attackOffset = attackInitOffset;

        isHitMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) /*&& isGrounded*/)
        {
            jumpCount--;
            if (jumpCount > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.SetTrigger("Jump");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isGrounded)
            {
                StartCoroutine(StaffAttack());

                //OnDrawGizmos();
                animator.SetTrigger("StaffAttack");
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetLayerWeight(1, 1f);
            animator.SetBool("IsGun", true);
            isFire = true;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetLayerWeight(1, 0f);
            animator.SetBool("IsGun", false);
            isFire = false;
        }

        if (isFire == true)
        {
            fireTime += Time.deltaTime;
            if (fireTime > fireCooltime)
            {
                GunFire();
                fireTime = 0f;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �� �信�� �� ������Ʈ�� ���õǾ��� ���� ����� �׸�
        // Gizmos.color�� ������� ������ �����մϴ�.
        // ���� ������ �� ���̵��� �������� ���������� �����غ��Կ�.
        Gizmos.color = new Color(1, 0, 0, 0.5f); // ����, ������

        // BoxCast�� OverlapBox�� �߽����� ũ�⸦ ����մϴ�.
        // Gizmos.DrawWireCube�� DrawWireSphere�� ���� ��ǥ�� ����մϴ�.
        // ���� transform.position�� attackOffset�� �����־�� �մϴ�.
        Vector2 boxCenter = (Vector2)transform.position + attackOffset;

        // Gizmos.matrix�� ����Ͽ� �ڽ��� ȸ���� �����մϴ�.
        // ���� ���� Gizmos.matrix�� �����صΰ�, �Լ� ���� �� �ٽ� �����ϴ� ���� �����ϴ�.
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, attackAngle), Vector3.one);

        // �ڽ� ���¸� �׸��ϴ�.
        // Gizmos.matrix�� �̹� ��ġ�� ȸ���� ����Ǿ����Ƿ�, DrawWireCube���� (0,0,0)�� ũ�⸸ �����մϴ�.
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(attackSize.x, attackSize.y, 0));

        // Gizmos.matrix�� ������� �����մϴ�.
        Gizmos.matrix = originalMatrix;
    }


    public void Respawn()
    {
        transform.position = respawnPoint;
    }

    public void HitAttack()
    {
        StartCoroutine(HitFlash());

        GameManager.Instance.ChangePlayerHealth(-1);
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void HalfJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / 2);
        animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameOver) 
        {
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float move = Input.GetAxis("Horizontal") * moveSpeed;

        if (isHitMotion == true)
        {
            move = 0;
        }

        if (frontVertical == 0 && vertical < 0)
        {
            isCrouch = true;
            animator.SetBool("IsCrouch", true);
        }
        else if (frontVertical < 0 && frontVertical < vertical)
        {
            animator.SetBool("IsCrouch", false);
        }
        else if (vertical >= 0)
        {
            isCrouch = false;
        }

        frontVertical = vertical;

        if (isGrounded == true && isCrouch == true)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);
        }

        int sign = Math.Sign(move);

        bool isWall = Physics2D.Raycast(transform.position, Vector2.left * (-sign), 0.3f, usableLayer);
        animator.SetBool("PushBox", isWall);
        //Debug.Log(Math.Sign(move));

        if (move < 0)
        {
            direction = sign;

            spriteRenderer.flipX = true;
            attackOffset = new Vector2(attackInitOffset.x * sign, attackInitOffset.y);
        }
        else if (move > 0)
        {
            direction = sign;

            spriteRenderer.flipX = false;
            attackOffset = new Vector2(attackInitOffset.x * sign, attackInitOffset.y);
        }

        //Debug.Log(move);
        animator.SetFloat("Speed", Mathf.Abs(move));

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, usableLayer | groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        if (isGrounded == true)
        {
            jumpCount = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.Instance.IncreasScore(10);

            collision.gameObject.GetComponent<CoinContoller>().StartCoinAnimation();
            //Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DeathZone"))
        {
            GameManager.Instance.ChangePlayerHealth(-1);
            Respawn();
        }

        if (collision.CompareTag("StageGoal"))
        {
            GameManager.Instance.CheckStageClear();
        }
    }

    private IEnumerator StaffAttack()
    {
        float animationDuration = 0.41f; // 5 / 12
        float delayTime = 0.16f; // 2 / 12
        float timer = 0;

        bool isHit = false;

        while (timer < animationDuration)
        {
            if (delayTime < timer)
            {
                isHitMotion = true;

                // hit Box�� �߽� ���
                Vector2 boxCenter = (Vector2)transform.position + attackOffset;

                // hit Box�� ��ġ�� Collider2D ��� ��������
                Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, attackSize, 0);
                foreach (Collider2D hit in hits)
                {
                    // �� �߿� tag�� Enemy�� �� �鸸
                    if (hit.CompareTag("Enemy") && isHit == false)
                    {
                        // ��� �� �ѹ� ������ �ش� ��� ���� �߿��� �ߺ� ���� ���� ����
                        isHit = true;

                        // 10 ������ ��ŭ�� ü���� ��´�
                        hit.GetComponent<EnemyController>().Hit(direction, 10);

                        //StartCoroutine(hit.GetComponent<EnemyController>().Death());
                    }
                }
            }

            timer += Time.deltaTime;

            yield return null;
        }

        isHitMotion = false;
    }

    private void GunFire()
    {
        float fireY = 1.5f;
        if (isCrouch == true && isGrounded == true)
        {
            fireY = 0.7f;
        }

        GameObject newBullet = Instantiate(bulletObject, transform.position + new Vector3(0.8f * direction, fireY), Quaternion.identity);

        SpriteRenderer sprite = newBullet.GetComponent<SpriteRenderer>();
        sprite.flipX = direction == 1 ? false : true;

        BulletController bulletScript = newBullet.GetComponent<BulletController>();
        if (bulletScript != null)
        {
            bulletScript.direction = direction;
        }
    }
}
