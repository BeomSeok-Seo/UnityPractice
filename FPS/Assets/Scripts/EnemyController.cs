using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int hp = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log($"Hit!! damage : {damage}, HP : {hp}");
    }
}
