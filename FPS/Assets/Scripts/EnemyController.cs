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
            // 플레이어가 보이는지
            if (CheckViewPlayer(out float distance))
            {
                // 플레이어 위치까지 이동
                agent.SetDestination(player.position);

                animator.SetBool("LookAt", true);

                // 공격 사거리 안쪽이면
                if (distance < attackDistance) 
                {
                    // 이동하지 않고
                    agent.isStopped = true;
                    animator.SetFloat("Speed", 0);

                    // 공격
                    if (isAttack == false)
                    {
                        isAttack = true;
                        InvokeRepeating("Attack", 1f, 3f);
                    }
                }
                else
                {
                    // 공격 사거리 바깥이지만 보이긴 하면
                    // 이동하여 가까이 다가감
                    agent.isStopped = false;
                    animator.SetFloat("Speed", speed);

                    // 공격은 중단
                    CancelInvoke("Attack");
                    if (isAttack == true)
                    {
                        isAttack = false;
                    }
                }

                // 보였을때 마지막 플레이어 위치 기억
                destPosition = player.position;
            }
            else
            {
                // 플레이어가 시야에서 사라지면
                // 마지막 보인 위치까지 이동
                agent.SetDestination(destPosition);

                animator.SetBool("LookAt", false);

                // 공격은 중단
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
        
        // 물리 상호작용 비활성화
        colliderBox.enabled = false;
        rb.isKinematic = true;

        // 없애기
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

        // 수평거리, 수직거리 계산
        Vector3 toTarget = endPosition - startPosition;
        float horizontalDistance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;
        float verticalDistance = toTarget.y;

        // 포물선 궤적을 위한 던지는 시간 계산
        float throwTime = Mathf.Sqrt(2 * throwHeight / Mathf.Abs(Physics.gravity.y)) +
                          Mathf.Sqrt(2 * (Mathf.Abs(verticalDistance) + throwHeight) / Mathf.Abs(Physics.gravity.y));

        // 수평속도, 수직속도
        float horizontalVelocity = horizontalDistance / throwTime;
        float verticalVelocity = (verticalDistance + throwHeight) / throwTime + 0.5f * Mathf.Abs(Physics.gravity.y) * throwTime;

        // 초기 속도 벡터 생성
        Vector3 direction = new Vector3(toTarget.x, 0, toTarget.z).normalized;
        Vector3 initialVelocity = direction * horizontalVelocity + Vector3.up * verticalVelocity;

        bombRb.linearVelocity = initialVelocity; // AddForce 대신 velocity를 직접 설정
    }

    private bool CheckViewPlayer(out float distance)
    {
        // 상대가 각도 안에 있는가
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > viewAngle / 2)
        {
            distance = 0;
            return false;
        }

        // 상대가 최대 사거리 안쪽에 있는가
        distance = Vector3.Distance(transform.position, player.position);

        if (distance > viewDistance)
        {
            return false;
        }

        // 상대와 나의 사이에 장애물이 있는가
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
