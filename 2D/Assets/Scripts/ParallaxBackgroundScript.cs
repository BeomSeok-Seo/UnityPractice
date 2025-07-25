using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    float parallaxEffectMultiplierX = 0.6f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 메인 카메라
        cameraTransform = Camera.main.transform;

        // 초기 카메라 위치
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라의 X축 이동 계산
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        // float deltaY = cameraTransform.position.y - lastCameraPosition.y; // Y축

        // 배경 오브젝트의 X축 위치를 업데이트
        transform.position += new Vector3(deltaX * parallaxEffectMultiplierX, 0, 0);
        // transform.position += new Vector3(deltaX * parallaxEffectMultiplierX, deltaY * parallaxEffectMultiplierY, 0); // Y축

        // 현재 카메라 위치를 저장
        lastCameraPosition = cameraTransform.position;
    }
}
