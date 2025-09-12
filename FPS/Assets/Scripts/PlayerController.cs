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
    // ���� ����
    private float sensitivity = 1f;
    private float verticalRotation = 0f;

    private float mouseX;
    private float mouseY;

    // �̵� ����
    private float moveSpeed = 3f;

    private float horizontalInput;
    private float verticalInput;

    // ���� ����
    private float jumpForce = 5f;
    private bool isGrounded = false;
    private Rigidbody rigidBody;

    // ü�� ����
    private float MaxHp = 200f;
    private float Hp = 200f;

    // �ѱ� ����
    private float range = 25f;
    public GameObject fireEffect;
    public GameObject hitEffect;
    //private int maxBullet = 30;
    //private int remaingBullet = 30;

    // �ѱ� Recoil ����
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

    // ī�޶�
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

        // ���� �ʱ�ȭ
        SetWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ���� ����
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // ���� ����
        transform.Rotate(0f, mouseX, 0f);
        // ���� ����
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        // Ű���� �̵� ����
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // rigidBody�� �̵� �ӵ� ����
        Vector3 moveInput = new Vector3(horizontalInput, 0f, verticalInput);
        Vector3 move = transform.TransformDirection(moveInput) * moveSpeed;

        rigidBody.linearVelocity = new Vector3(move.x, rigidBody.linearVelocity.y, move.z);

        // �̵� �ִϸ��̼� ���
        float speed = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z).magnitude;
        currentWeaponAnimator.SetFloat("speed", speed);

        // ����
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.8f);
        currentWeaponAnimator.SetBool("grounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        // ������
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // �� ��� raycast
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // recoil
        //targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        //currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
        //cameraTransform.localRotation = Quaternion.Euler(currentRotation);

        // ���� ����
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

        // ����� Ÿ�̸� ���̱�
        DebuffUI.SetActive(true);

        // �ʱ� ������
        TakeDamage(damage);

        while (timer < dotEndTime)
        {
            // ���� �ð��� ������
            if (intervalTimer > interval)
            {
                // ������ ����
                TakeDamage(damage);
                intervalTimer = 0;
            }

            timer += Time.deltaTime;
            intervalTimer += Time.deltaTime;

            // ����� Ÿ�̸� UI ȸ�� ǥ��
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
        // ���� �Ѿ��� 0�̸�
        if (remaingBulletList[weaponIndex] == 0)
        {
            return;
        }

        // ���ε� ���� �ִϸ��̼��� ��� ���̸�
        if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("reload"))
        {
            return;
        }

        // �߻� ���� �ִϸ��̼��� ��� ���̸�
        if (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("fire"))
        {
            return;
        }

        // ���� �Ѿ� ����
        remaingBulletList[weaponIndex]--;

        // ���� �Ѿ� UI�� ���
        ShowBulletUI();

        // ��ƼŬ ����Ʈ
        //GameObject fire = Instantiate(fireEffect, transform.position + new Vector3(0, 0.5f, 0) + transform.forward * 2f, Quaternion.identity, transform);
        //Destroy(fire, 0.6f);

        currentWeaponAnimator.SetTrigger("fire");

        // �Ѿ� ��Ʈ��ĵ range �Ÿ�������
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // �Ѿ˿� �´� ��ü�� �±װ� Enemy�� ��
            if (hit.collider.CompareTag("Enemy"))
            {
                // �ǰ� ��ƼŬ ����Ʈ
                GameObject hitfire = Instantiate(hitEffect, hit.point, Quaternion.identity);
                Destroy(hitfire, 0.6f);

                // �ǰ� ������ �Լ� ȣ��
                hit.collider.GetComponent<EnemyController>().TakeDamage(weaponDamageList[weaponIndex]);
            }
        }

        // ApplyRecoil
        //targetRotation += new Vector3(-recoilAmount, UnityEngine.Random.Range(-1f, 1f), 0f);
    }

    // ������
    private void Reload()
    {
        currentWeaponAnimator.SetTrigger("reload");

        remaingBulletList[weaponIndex] = maxBulletList[weaponIndex];
        ShowBulletUI();
    }

    // ���� �Ѿ� ���
    private void ShowBulletUI()
    {
        BulletUI.text = $"{remaingBulletList[weaponIndex]} / {maxBulletList[weaponIndex]}";
    }

    private void ShowHealthUI()
    {
        HealthUI.text = $"{Hp} / {MaxHp}";
    }

    // ���� ����
    private void SetWeapon(int numberIndex)
    {
        // ���� ����� �Է� ���Ⱑ ������ ���� ����
        if (weaponIndex == numberIndex)
        {
            return;
        }

        // ���� ��� ��Ȱ��ȭ
        foreach (var weapon in weaponList)
        {
            weapon.SetActive(false);
        }

        object target = weaponList.GetValue(numberIndex);

        if (target != null)
        {
            weaponIndex = numberIndex;

            // ���� ���� Ȱ��ȭ
            GameObject targetWeapon = target as GameObject;
            targetWeapon.SetActive(true);

            // ���� ���� �ִϸ��̼� ���
            currentWeaponAnimator = targetWeapon.GetComponentInChildren<Animator>();

            currentWeaponAnimator.SetTrigger("selected");
            currentWeaponAnimator.SetFloat("reloadSpeed", 1);
            currentWeaponAnimator.SetFloat("fireSpeed", 1);

            // ���� �Ѿ� ���
            ShowBulletUI();
        }
    }

    private void DisableHitPanel()
    {
        HitPanel.SetActive(false);
    }
}
