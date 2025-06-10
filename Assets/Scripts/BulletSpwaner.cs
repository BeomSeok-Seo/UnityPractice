using UnityEngine;

public class BulletSpwaner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Instantiate(bulletPrefab, firePoint.position + new Vector3(1.1f, 0, 0), firePoint.rotation * new Quaternion(0, 30, 0, 0));

            Instantiate(bulletPrefab, firePoint.position + new Vector3(-0.7f, 0, 0), firePoint.rotation);
        }
    }
}
