using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    NavMeshAgent agent;
    GameObject player;
    PlayerHealthPoint playerHealthPoint;
    //float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");

        playerHealthPoint = player.GetComponent<PlayerHealthPoint>();

        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(agent.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealthPoint.health -= 5;
            //Debug.Log("Ãæµ¹");
        }
    }

    private void FixedUpdate()
    {
        //float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
