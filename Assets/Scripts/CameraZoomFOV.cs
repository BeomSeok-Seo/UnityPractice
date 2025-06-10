using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomFOV : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public float zoomSpeed = 10f;
    public float minFOV = 20f;
    public float maxFOV = 60f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float fov = virtualCamera.Lens.FieldOfView;
            fov -= scroll * zoomSpeed;
            fov = Mathf.Clamp(fov, minFOV, maxFOV);
            virtualCamera.Lens.FieldOfView = fov;
        }
    }
}
