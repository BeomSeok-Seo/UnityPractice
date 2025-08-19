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
    private float moveSpeed = 5f;

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

    public GameObject[] weaponList;
    private int weaponIndex = 0;
    private int[] maxBulletList = new int[] { 30, 6, 1 };
    private List<int> remaingBulletList = new List<int>() { 30, 6, 1 };
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

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // ����
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.8f);
        currentWeaponAnimator.SetBool("grounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }



        // �� ��� raycast
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

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

        remaingBulletList[weaponIndex]--;

        ShowBulletUI();

        // ��ƼŬ ����Ʈ
        //GameObject fire = Instantiate(fireEffect, transform.position + new Vector3(0, 0.5f, 0) + transform.forward * 2f, Quaternion.identity, transform);
        //Destroy(fire, 0.6f);

        currentWeaponAnimator.SetTrigger("fire");

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                GameObject hitfire = Instantiate(hitEffect, hit.point, Quaternion.identity);

                Destroy(hitfire, 0.6f);

                hit.collider.GetComponent<EnemyController>().TakeDamage(10);
            }
        }
    }

    private void Reload()
    {
        currentWeaponAnimator.SetTrigger("reload");

        remaingBulletList[weaponIndex] = maxBulletList[weaponIndex];
        ShowBulletUI();
    }

    private void ShowBulletUI()
    {
        BulletUI.text = $"{remaingBulletList[weaponIndex]} / {maxBulletList[weaponIndex]}";
    }

    private void SetWeapon(int numberIndex)
    {
        foreach (var weapon in weaponList)
        {
            weapon.SetActive(false);
        }

        object target = weaponList.GetValue(numberIndex);
        if (target != null)
        {
            weaponIndex = numberIndex;

            GameObject targetWeapon = target as GameObject;
            targetWeapon.SetActive(true);

            currentWeaponAnimator = targetWeapon.GetComponentInChildren<Animator>();

            currentWeaponAnimator.SetTrigger("selected");
            currentWeaponAnimator.SetFloat("reloadSpeed", 1);
            currentWeaponAnimator.SetFloat("fireSpeed", 1);

            ShowBulletUI();
        }
    }
}
