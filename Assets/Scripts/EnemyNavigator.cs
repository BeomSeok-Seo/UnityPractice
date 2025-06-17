using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigator : MonoBehaviour
{
    enum EnemyState { Patrol, Chase, Attack }
    EnemyState currentState;

    public Transform[] waypoints;
    int currentIndex = 0;

    GameObject player;
    NavMeshAgent agent;
    float chaseRange = 8f;

    bool waiting = false;
    float waitTimer = 0f;
    float waitTime = 2f;

    MeshCollider site;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        site = GetComponentInChildren<MeshCollider>();

        currentState = EnemyState.Patrol;
        GoToNextPoint();
    }

    // Update is called once per frame
    void Update()
    {

        //if (Vector3.Distance(transform.position, player.transform.position) < chaseRange)
        //{
        //    currentState = EnemyState.Chase;
        //}
        //else
        //{
        //    currentState = EnemyState.Patrol;
        //}

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && 
            Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit) &&
            hit.collider.gameObject.tag == "Player")
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    void Patrol()
    {
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            return;
        }

        if (!waiting)
        {
            waiting = true;
            waitTimer = waitTime;
        }
        else
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer < 0f)
            {
                waiting = false;
                GoToNextPoint();
            }
        }
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0) return;

        agent.destination = waypoints[currentIndex].position;
        currentIndex = (currentIndex + 1) % waypoints.Length;
    }
}
