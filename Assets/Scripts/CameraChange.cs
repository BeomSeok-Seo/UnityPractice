using Unity.Cinemachine;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    CinemachineCamera cam1;
    CinemachineCamera cam2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("CineCamera");

        cam1 = cameras[0].GetComponent<CinemachineCamera>();
        cam2 = cameras[1].GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            cam1.Priority = 20;
            cam2.Priority = 10;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            cam1.Priority = 10;
            cam2.Priority = 20;
        }
    }
}
