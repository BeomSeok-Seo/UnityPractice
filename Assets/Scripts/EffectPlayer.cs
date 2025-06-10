using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    Rigidbody rb;
    ParticleSystem effect;
    float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        effect = GetComponentInChildren<ParticleSystem>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Horizontal") * 1f);

        Vector3 moveInput = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //Vector3 move = moveInput * moveSpeed;
        Vector3 move = Input.GetAxis("Vertical") * transform.forward * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + "°ú Ãæµ¹!");

        //effect.Play();

        //StartCoroutine(DestroyEffect());

        ParticleSystem par = Instantiate(effect);
        par.transform.SetParent(gameObject.transform);
        par.transform.localPosition = new Vector3(0, 0, 0);
        par.transform.localRotation = Quaternion.identity;
        par.Play();

        Destroy(par.gameObject, 2f);
    }

    //IEnumerator DestroyEffect()
    //{
    //    while (true)
    //    {
    //        ParticleSystem par = Instantiate(effect);
    //        par.Play();

    //        yield return new WaitForSeconds(2f);

    //        Destroy(par);
    //    }
    //}
}
