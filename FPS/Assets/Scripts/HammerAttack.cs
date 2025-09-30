using Unity.VisualScripting;
using UnityEngine;

public class HammerAttack : MonoBehaviour
{
    bool isDamage = false;
    PlayerController pc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetDamage() 
    {
        isDamage = true;
    }

    public void StopDamage()
    {
        isDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDamage)
        {
            return;
        }

        if (other.transform.CompareTag("Player"))
        {

            PlayerController pc = other.transform.GetComponent<PlayerController>();
            pc.TakeDamage(40f);
            isDamage = false;
        }
    }
}
