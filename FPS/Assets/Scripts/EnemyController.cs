using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int hp = 50;
    Transform player;
    float viewAngle = 45f;
    float viewDistance = 20f;

    public LayerMask obstacleMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
       // obstacleMask = LayerMask.GetMask("Obstacle");

        Debug.Log(obstacleMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckViewPlayer())
        {
            Debug.Log("I See You");
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log($"Hit!! damage : {damage}, HP : {hp}");
    }

    private bool CheckViewPlayer()
    {
        // 상대가 각도 안에 있는가
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > viewAngle / 2)
        {
            return false;
        }

        // 상대가 최대 사거리 안쪽에 있는가
        float distance = Vector3.Distance(transform.position, player.position);

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
}
