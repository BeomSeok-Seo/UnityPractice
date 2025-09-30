using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    bool isDamage = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 3f);

        if (isDamage)
        {
            return;
        }

        if (collision.transform.CompareTag("Player"))
        {
            PlayerController pc = collision.transform.GetComponent<PlayerController>();
            //pc.TakeDamage(20);
            pc.TakeDotDamage(5f, 2f, 0.5f);
            isDamage = true;
        }
    }
}
