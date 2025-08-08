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
    private float jumpForce = 2f;
    private bool isGrounded = false;
    private Rigidbody rigidBody;

    // ī�޶�
    private Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        cameraTransform = transform.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
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

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        Debug.Log("Slider Value Changed: " + sensitivity);
        this.sensitivity = sensitivity;
    }
}
