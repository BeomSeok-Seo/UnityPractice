using UnityEngine;

public class FireEffect : MonoBehaviour
{
    ParticleSystem ps;

    bool psFlag = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        psFlag = ps.isPlaying;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ps.Play();
            psFlag = true;
        }

        if (psFlag && !ps.isPlaying)
        {
            Destroy(gameObject);
            psFlag = false;
        }
    }
}
