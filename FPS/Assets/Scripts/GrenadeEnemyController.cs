using Unity.Services.Analytics;
using UnityEngine;

public class GrenadeEnemyController : EnemyController
{
    public GameObject LocalGrenade;
    public GameObject WorldGrenade;

    float throwHeight = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        attackDelay = 1f;
        attackRepeatRate = 3f;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
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

}
