using System;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    float MaxHp = 50;
    float hp = 50;
    protected Transform player;
    protected NavMeshAgent agent;
    protected Animator animator;

    // 시야
    protected float viewAngle = 65f;
    protected float viewDistance = 20f;
    protected float attackDistance = 10f;

    float updateRate = 0.5f;
    float nextUpdateTime = 0f;
    float speed = 2f;

    // 공격
    bool isAttack = false;
    protected float attackDelay = 1f;
    protected float attackRepeatRate = 3f;

    Vector3 destPosition = Vector3.zero;
    protected Transform rightHand;

    Collider colliderBox;
    Rigidbody rb;

    GameObject enemyHealthUI;
    Slider healthBarSlider;

    Camera mainCamera;

    public LayerMask obstacleMask;

    public GameObject HealthBarUI;

    public Action DeadEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        colliderBox = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        enemyHealthUI = GameObject.Find("UI").transform.Find("EnemyHealthUI").gameObject;
        mainCamera = Camera.main;

        //healthBarSlider = Instantiate(HealthBarUI, enemyHealthUI.transform).GetComponent<Slider>();

        agent.speed = speed;

        rightHand = FindChildObjectByName("RightHand", transform);

        //Debug.Log(rightHand.name);
        // obstacleMask = LayerMask.GetMask("Obstacle");

        //Debug.Log(obstacleMask);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            // 플레이어가 보이는지
            if (CheckViewPlayer(out float distance))
            {
                // 플레이어 위치까지 이동
                agent.SetDestination(player.position);

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
                        InvokeRepeating("Attack", attackDelay, attackRepeatRate);
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
                agent.isStopped = false;
                animator.SetFloat("Speed", speed);

                agent.SetDestination(destPosition);

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

        // 체력바 보이기
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


    protected virtual void Attack()
    {
    }

    public void TakeDamage(int damage)
    {
        // 처음 데미지 입을때 체력바 보이기
        if (hp == MaxHp)
        {
            healthBarSlider = Instantiate(HealthBarUI, enemyHealthUI.transform).GetComponent<Slider>();
        }

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
        Destroy(transform.parent.gameObject, 1.5f);
        Destroy(healthBarSlider.gameObject);

        DeadEvent?.Invoke();
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
