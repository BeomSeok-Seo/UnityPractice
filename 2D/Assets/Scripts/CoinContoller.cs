using System.Collections;
using UnityEngine;

public class CoinContoller : MonoBehaviour
{
    public float animationDuration = 0.5f;

    private Camera mainCamera;
    private Transform scoreUITransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        scoreUITransform = GameObject.FindGameObjectWithTag("ScoreText").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCoinAnimation()
    {
        StartCoroutine(AnimateCoinToUI());
    }

    private IEnumerator AnimateCoinToUI()
    {
        float timer = 0f;
        Vector3 startPos = transform.position; // 동전의 현재 월드 위치

        // 스코어 UI의 스크린 좌표를 가져옵니다. (RectTransform.position은 캔버스에 따라 다릅니다)
        // RectTransformUtility를 사용하여 UI 요소의 스크린 좌표를 월드 좌표로 변환하는 것이 가장 정확합니다.
        Vector2 uiScreenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, scoreUITransform.position);

        // UI가 Screen Space - Overlay인 경우, UI의 RectTransform.position은 이미 스크린 좌표에 가깝습니다.
        // 하지만 RectTransformUtility를 사용하는 것이 더 안전합니다.
        // 여기서는 TextMeshProUGUI의 RectTransform.position을 바로 사용해 봅니다.
        // 만약 UI가 Scale이 되어 있다면 RectTransformUtility가 더 정확합니다.
        Vector3 targetScreenPos = scoreUITransform.position; // UI 요소의 스크린 좌표

        // 스크린 좌표를 게임 월드 좌표로 변환합니다.
        // Z 값은 2D 게임에서 중요하지 않지만, 카메라의 nearClipPlane보다 커야 합니다.
        // 일반적으로 0으로 두거나, 카메라의 Z 값과 동일하게 두어 평면을 유지합니다.
        Vector3 targetWorldPos = mainCamera.ScreenToWorldPoint(targetScreenPos);
        targetWorldPos.z = startPos.z; // 동전의 Z 위치를 유지하여 2D 평면에서 움직이도록 합니다.

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animationDuration;

            transform.position = Vector3.Lerp(startPos, targetWorldPos, progress);

            yield return null; // 다음 프레임까지 대기
        }

        transform.position = targetWorldPos;
        //gameObject.SetActive(false); // 동전 오브젝트 비활성화 (사라지게 함)
        Destroy(gameObject);
    }
}
