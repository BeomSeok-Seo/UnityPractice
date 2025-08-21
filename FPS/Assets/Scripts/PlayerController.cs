using Mono.Cecil.Cil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private float jumpForce = 8f;
    private bool isGrounded = false;
    private Rigidbody rigidBody;

    // �ѱ� ����
    private float range = 10f;
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
    private int[] weaponDamageList = new int[] { 5, 15, 30 };
    private Animator currentWeaponAnimator;

    // ī�޶�
    private Transform cameraTransform;

    // UI
    private TMP_Text BulletUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        cameraTransform = transform.Find("Main Camera");
        BulletUI = GameObject.Find("UI").transform.Find("BulletUI").GetComponent<TMP_Text>();

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
        Vector3 move = moveInput * moveSpeed;

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
}
