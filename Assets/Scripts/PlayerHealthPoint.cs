using UnityEngine;

public class PlayerHealthPoint : MonoBehaviour
{
    public float maxHealth = 60;

    public float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
