using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    float parallaxEffectMultiplierX = 0.6f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� ī�޶�
        cameraTransform = Camera.main.transform;

        // �ʱ� ī�޶� ��ġ
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶��� X�� �̵� ���
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        // float deltaY = cameraTransform.position.y - lastCameraPosition.y; // Y��

        // ��� ������Ʈ�� X�� ��ġ�� ������Ʈ
        transform.position += new Vector3(deltaX * parallaxEffectMultiplierX, 0, 0);
        // transform.position += new Vector3(deltaX * parallaxEffectMultiplierX, deltaY * parallaxEffectMultiplierY, 0); // Y��

        // ���� ī�޶� ��ġ�� ����
        lastCameraPosition = cameraTransform.position;
    }
}
