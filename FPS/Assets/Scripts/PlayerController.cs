using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // 시점 변수
    private float sensitivity = 1f;
    private float verticalRotation = 0f;

    private float mouseX;
    private float mouseY;

    // 이동 변수
    private float moveSpeed = 3f;

    private float horizontalInput;
    private float verticalInput;

    // 점프 변수
    private float jumpForce = 5f;
    private bool isGrounded = false;
    private Rigidbody rigidBody;

    // 체력 변수
    private float MaxHp = 200f;
    private float Hp = 200f;

    // 총기 변수
    private float range = 25f;
    public GameObject fireEffect;
    public GameObject hitEffect;
    //private int maxBullet = 30;
    //private int remaingBullet = 30;

    // 총기 Recoil 변수
    Vector3 targetRotation;
    Vector3 currentRotation;
    float returnSpeed = 5f;
    float recoilSpeed = 5f;
    float recoilAmount = 10f;

    public GameObject[] weaponList;
    private int weaponIndex = -1;
    private int[] maxBulletList = new int[] { 30, 6, 1 };
    private List<int> remaingBulletList = new List<int>() { 30, 6, 1 };
    private int[] weaponDamageList = new int[] { 20, 40, 120 };
    private Animator currentWeaponAnimator;

    // 카메라
    private Transform cameraTransform;

    // UI
    private TMP_Text BulletUI;
    private TMP_Text HealthUI;
    private GameObject HitPanel;
    private GameObject DebuffUI;
    private UnityEngine.UI.Image DebuffUIUpper;

    private float dotEndTime = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        cameraTransform = transform.Find("Main Camera");
        BulletUI = GameObject.Find("UI").transform.Find("BulletUI").GetComponent<TMP_Text>();
        HealthUI = GameObject.Find("UI").transform.Find("HealthUI").GetComponent<TMP_Text>();
        HitPanel = GameObject.Find("UI").transform.Find("HitPanel").gameObject;
        DebuffUI = GameObject.Find("UI").transform.Find("Debuff").gameObject;
        DebuffUIUpper = GameObject.Find("UI").transform.Find("Debuff").Find("Upper").GetComponent<UnityEngine.UI.Image>();

        HitPanel.SetActive(false);

        // 무기 초기화
        SetWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 시점 조절
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 수평 조절
        transform.Rotate(0f, mouseX, 0f);
        // 수직 조절
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        // 키보드 이동 조절
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // rigidBody에 이동 속도 지정
        Vector3 moveInput = new Vector3(horizontalInput, 0f, verticalInput);
        Vector3 move = transform.TransformDirection(moveInput) * moveSpeed;

        rigidBody.linearVelocity = new Vector3(move.x, rigidBody.linearVelocity.y, move.z);

        // 이동 애니메이션 계산
        float speed = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z).magnitude;
        currentWeaponAnimator.SetFloat("speed", speed);

        // 점프
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.8f);
        currentWeaponAnimator.SetBool("grounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // 총 쏘기 raycast
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // recoil
        //targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        //currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
        //cameraTransform.localRotation = Quaternion.Euler(currentRotation);

        // 무기 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(2);
        }
    }

    private void FixedUpdate()
    {

        
    }


    public void TakeDamage(float damage)
    {
        HitPanel.SetActive(true);
        Invoke("DisableHitPanel", 0.1f);

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
        }

        ShowHealthUI();
    }

    public void TakeDotDamage(float damage, float duration, float interval)
    {
        StartCoroutine(DotDamage(damage, duration, interval));
    }

    private IEnumerator DotDamage(float damage, float duration, float interval)
    {
        float timer = 0;
        float intervalTimer = 0;
        dotEndTime = duration;

        // 디버프 타이머 보이기
        DebuffUI.SetActive(true);

        // 초기 데미지
        TakeDamage(damage);

        while (timer < dotEndTime)
        {
            // 일정 시간이 지나면
            if (intervalTimer > interval)
            {
                // 데미지 받음
                TakeDamage(damage);
                intervalTimer = 0;
            }

            timer += Time.deltaTime;
            intervalTimer += Time.deltaTime;

            // 디버프 타이머 UI 회전 표시
            DebuffUIUpper.fillAmount = 1 - (timer / duration);

            yield return null;
        }

        DebuffUI.SetActive(false);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        //Debug.Log("Slider Value Changed: " + sensitivity);
        this.sensitivity = sensitivity;
    }

    private void Fire()
    {
        // 남은 총알이 0이면
        if (remaingBulletList[weaponIndex] == 0)
        {
            return;
        }

        // 리로드 중인 애니메이션이 재생 중이면
        if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("reload"))
        {
            return;
        }

        // 발사 중인 애니메이션이 재생 중이면
        if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("fire"))
        {
            return;
        }

        // 남은 총알 감소
        remaingBulletList[weaponIndex]--;

        // 남은 총알 UI에 출력
        ShowBulletUI();

        // 파티클 이펙트
        //GameObject fire = Instantiate(fireEffect, transform.position + new Vector3(0, 0.5f, 0) + transform.forward * 2f, Quaternion.identity, transform);
        //Destroy(fire, 0.6f);

        currentWeaponAnimator.SetTrigger("fire");

        // 총알 히트스캔 range 거리까지만
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // 총알에 맞는 객체의 태그가 Enemy일 때
            if (hit.collider.CompareTag("Enemy"))
            {
                // 피격 파티클 이펙트
                GameObject hitfire = Instantiate(hitEffect, hit.point, Quaternion.identity);
                Destroy(hitfire, 0.6f);

                // 피격 데미지 함수 호출
                hit.collider.GetComponent<EnemyController>().TakeDamage(weaponDamageList[weaponIndex]);
            }
        }

        // ApplyRecoil
        //targetRotation += new Vector3(-recoilAmount, UnityEngine.Random.Range(-1f, 1f), 0f);
    }

    // 재장전
    private void Reload()
    {
        currentWeaponAnimator.SetTrigger("reload");

        remaingBulletList[weaponIndex] = maxBulletList[weaponIndex];
        ShowBulletUI();
    }

    // 남은 총알 출력
    private void ShowBulletUI()
    {
        BulletUI.text = $"{remaingBulletList[weaponIndex]} / {maxBulletList[weaponIndex]}";
    }

    private void ShowHealthUI()
    {
        HealthUI.text = $"{Hp} / {MaxHp}";
    }

    // 무기 변경
    private void SetWeapon(int numberIndex)
    {
        // 현재 무기와 입력 무기가 같으면 동작 안함
        if (weaponIndex == numberIndex)
        {
            return;
        }

        // 무기 모두 비활성화
        foreach (var weapon in weaponList)
        {
            weapon.SetActive(false);
        }

        object target = weaponList.GetValue(numberIndex);

        if (target != null)
        {
            weaponIndex = numberIndex;

            // 선택 무기 활성화
            GameObject targetWeapon = target as GameObject;
            targetWeapon.SetActive(true);

            // 무기 변경 애니메이션 출력
            currentWeaponAnimator = targetWeapon.GetComponentInChildren<Animator>();

            currentWeaponAnimator.SetTrigger("selected");
            currentWeaponAnimator.SetFloat("reloadSpeed", 1);
            currentWeaponAnimator.SetFloat("fireSpeed", 1);

            // 남은 총알 출력
            ShowBulletUI();
        }
    }

    private void DisableHitPanel()
    {
        HitPanel.SetActive(false);
    }
}
