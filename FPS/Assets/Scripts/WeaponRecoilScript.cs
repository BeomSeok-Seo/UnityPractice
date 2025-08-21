using System;
using UnityEngine;

public class WeaponRecoilScript : MonoBehaviour
{
    Vector3 targetRotation;
    Vector3 currentRotation;
    float returnSpeed = 5f;
    float recoilSpeed = 50f;
    float recoilAmount = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ApplyRecoil();
        }

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    private void ApplyRecoil()
    {
        targetRotation += new Vector3(-recoilAmount, UnityEngine.Random.Range(-1f, 1f), 0f);
    }
}
