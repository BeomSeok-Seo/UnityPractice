using UnityEngine;

public class HammerEnemyController : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        attackDistance = 2.5f;
        viewAngle = 180f;

        attackDelay = 0.1f;
        attackRepeatRate = 1.2f;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        animator.SetTrigger("Swing");
    }
}
