using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool canAttack = true;
    public float spawnDelay = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            canAttack = false;
            Debug.Log("���� ���� ��!");

            yield return new WaitForSeconds(spawnDelay);

            canAttack = true;
            Debug.Log("�ٽ� ���� �� �غ� �Ϸ�!");
        }
    }
}
