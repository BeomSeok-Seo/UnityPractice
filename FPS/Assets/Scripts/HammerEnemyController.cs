using UnityEngine;

public class HammerEnemyController : EnemyController
{
    HammerAttack hammer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        attackDistance = 2.5f;
        viewAngle = 180f;

        attackDelay = 0.1f;
        attackRepeatRate = 1.2f;

        hammer = transform.GetComponentInChildren<HammerAttack>();

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
        hammer.ResetDamage();
        Invoke("StopHammer", 0.8f);
    }

    private void StopHammer()
    {
        hammer.StopDamage();
    }
}
