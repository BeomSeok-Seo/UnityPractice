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
        Vector3 startPos = transform.position; // ������ ���� ���� ��ġ

        // ���ھ� UI�� ��ũ�� ��ǥ�� �����ɴϴ�. (RectTransform.position�� ĵ������ ���� �ٸ��ϴ�)
        // RectTransformUtility�� ����Ͽ� UI ����� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ�ϴ� ���� ���� ��Ȯ�մϴ�.
        Vector2 uiScreenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, scoreUITransform.position);

        // UI�� Screen Space - Overlay�� ���, UI�� RectTransform.position�� �̹� ��ũ�� ��ǥ�� �������ϴ�.
        // ������ RectTransformUtility�� ����ϴ� ���� �� �����մϴ�.
        // ���⼭�� TextMeshProUGUI�� RectTransform.position�� �ٷ� ����� ���ϴ�.
        // ���� UI�� Scale�� �Ǿ� �ִٸ� RectTransformUtility�� �� ��Ȯ�մϴ�.
        Vector3 targetScreenPos = scoreUITransform.position; // UI ����� ��ũ�� ��ǥ

        // ��ũ�� ��ǥ�� ���� ���� ��ǥ�� ��ȯ�մϴ�.
        // Z ���� 2D ���ӿ��� �߿����� ������, ī�޶��� nearClipPlane���� Ŀ�� �մϴ�.
        // �Ϲ������� 0���� �ΰų�, ī�޶��� Z ���� �����ϰ� �ξ� ����� �����մϴ�.
        Vector3 targetWorldPos = mainCamera.ScreenToWorldPoint(targetScreenPos);
        targetWorldPos.z = startPos.z; // ������ Z ��ġ�� �����Ͽ� 2D ��鿡�� �����̵��� �մϴ�.

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animationDuration;

            transform.position = Vector3.Lerp(startPos, targetWorldPos, progress);

            yield return null; // ���� �����ӱ��� ���
        }

        transform.position = targetWorldPos;
        //gameObject.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ (������� ��)
        Destroy(gameObject);
    }
}
