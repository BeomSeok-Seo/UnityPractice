using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    float MaxHp = 50;
    float hp = 50;
    Transform player;
    NavMeshAgent agent;
    Animator animator;

    float viewAngle = 65f;
    float viewDistance = 20f;
    float attackDistance = 10f;

    float updateRate = 0.5f;
    float nextUpdateTime = 0f;
    float speed = 2f;

    float throwHeight = 2f;

    bool isAttack = false;

    Vector3 destPosition = Vector3.zero;
    Transform rightHand;

    Collider colliderBox;
    Rigidbody rb;

    GameObject enemyHealthUI;
    Slider healthBarSlider;

    Camera mainCamera;

    public LayerMask obstacleMask;
    public GameObject LocalGrenade;
    public GameObject WorldGrenade;

    public GameObject HealthBarUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        colliderBox = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        enemyHealthUI = GameObject.Find("UI").transform.Find("EnemyHealthUI").gameObject;
        mainCamera = Camera.main;

        healthBarSlider = Instantiate(HealthBarUI, enemyHealthUI.transform).GetComponent<Slider>();

        agent.speed = speed;

        rightHand = FindChildObjectByName("RightHand", transform);

        //Debug.Log(rightHand.name);
        // obstacleMask = LayerMask.GetMask("Obstacle");

        //Debug.Log(obstacleMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            // �÷��̾ ���̴���
            if (CheckViewPlayer(out float distance))
            {
                // �÷��̾� ��ġ���� �̵�
                agent.SetDestination(player.position);

                animator.SetBool("LookAt", true);

                // ���� ��Ÿ� �����̸�
                if (distance < attackDistance) 
                {
                    // �̵����� �ʰ�
                    agent.isStopped = true;
                    animator.SetFloat("Speed", 0);

                    // ����
                    if (isAttack == false)
                    {
                        isAttack = true;
                        InvokeRepeating("Attack", 1f, 3f);
                    }
                }
                else
                {
                    // ���� ��Ÿ� �ٱ������� ���̱� �ϸ�
                    // �̵��Ͽ� ������ �ٰ���
                    agent.isStopped = false;
                    animator.SetFloat("Speed", speed);

                    // ������ �ߴ�
                    CancelInvoke("Attack");
                    if (isAttack == true)
                    {
                        isAttack = false;
                    }
                }

                // �������� ������ �÷��̾� ��ġ ���
                destPosition = player.position;
            }
            else
            {
                // �÷��̾ �þ߿��� �������
                // ������ ���� ��ġ���� �̵�
                agent.SetDestination(destPosition);

                animator.SetBool("LookAt", false);

                // ������ �ߴ�
                CancelInvoke("Attack");
                if (isAttack == true)
                {
                    isAttack = false;
                }
            }

            float destDistance = Vector3.Distance(transform.position, destPosition);

            if (destDistance < 0.3)
            {
                agent.isStopped = true;
                animator.SetFloat("Speed", 0);
            }

            nextUpdateTime = Time.time + updateRate;
        }

        if (healthBarSlider != null && mainCamera != null)
        {
            Vector3 worldPosition = transform.position + new Vector3(0, 2, 0);
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPosition);

            healthBarSlider.transform.position = screenPoint;

            if (screenPoint.z > 0)
            {
                healthBarSlider.gameObject.SetActive(true);
            }
            else
            {
                healthBarSlider.gameObject.SetActive(false);
            }
        }

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        healthBarSlider.value = hp / MaxHp;

        if (hp <= 0)
        {
            Die();
        }

        //Debug.Log($"Hit!! damage : {damage}, HP : {hp}");
    }

    private void Die()
    {
        animator.SetTrigger("Die");

        agent.isStopped = true;
        
        // ���� ��ȣ�ۿ� ��Ȱ��ȭ
        colliderBox.enabled = false;
        rb.isKinematic = true;

        // ���ֱ�
        Destroy(gameObject, 1.5f);
        Destroy(healthBarSlider.gameObject);
    }

    private void Attack()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);
        animator.SetTrigger("Attack");

        GameObject weapon = Instantiate(LocalGrenade, rightHand);
        Destroy(weapon, 0.5f);

        Invoke("InstantGrenade", 0.4f);
    }

    private void InstantGrenade()
    {
        GameObject bomb = Instantiate(WorldGrenade, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();

        Vector3 startPosition = bomb.transform.position;
        Vector3 endPosition = player.position - new Vector3(0, 1, 0);

        // ����Ÿ�, �����Ÿ� ���
        Vector3 toTarget = endPosition - startPosition;
        float horizontalDistance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;
        float verticalDistance = toTarget.y;

        // ������ ������ ���� ������ �ð� ���
        float throwTime = Mathf.Sqrt(2 * throwHeight / Mathf.Abs(Physics.gravity.y)) +
                          Mathf.Sqrt(2 * (Mathf.Abs(verticalDistance) + throwHeight) / Mathf.Abs(Physics.gravity.y));

        // ����ӵ�, �����ӵ�
        float horizontalVelocity = horizontalDistance / throwTime;
        float verticalVelocity = (verticalDistance + throwHeight) / throwTime + 0.5f * Mathf.Abs(Physics.gravity.y) * throwTime;

        // �ʱ� �ӵ� ���� ����
        Vector3 direction = new Vector3(toTarget.x, 0, toTarget.z).normalized;
        Vector3 initialVelocity = direction * horizontalVelocity + Vector3.up * verticalVelocity;

        bombRb.linearVelocity = initialVelocity; // AddForce ��� velocity�� ���� ����
    }

    private bool CheckViewPlayer(out float distance)
    {
        // ��밡 ���� �ȿ� �ִ°�
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > viewAngle / 2)
        {
            distance = 0;
            return false;
        }

        // ��밡 �ִ� ��Ÿ� ���ʿ� �ִ°�
        distance = Vector3.Distance(transform.position, player.position);

        if (distance > viewDistance)
        {
            return false;
        }

        // ���� ���� ���̿� ��ֹ��� �ִ°�
        if (Physics.Raycast(transform.position, dirToPlayer, distance, obstacleMask))
        {
            return false;
        }

        return true;
    }

    private Transform FindChildObjectByName(string name, Transform parent)
    {
        int count = parent.childCount;
        Transform child = null;

        for (int i = 0; i < count; i++)
        {
            child = parent.GetChild(i);
            if (child.name.Contains(name))
            {
                return child;
            }
            else
            {
                child = FindChildObjectByName(name, child);
                if (child != null) 
                {
                    return child;
                }
            }
        }

        return null;
    }
}
