using UnityEngine;
using UnityEngine.Rendering;

public class RayCastTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.Log(hit.collider.gameObject.name + "¿¡ ºÎµúÈû!");
        }

        Debug.DrawRay(transform.position, transform.forward + new Vector3(0f, 0f, 9f));
    }
}
