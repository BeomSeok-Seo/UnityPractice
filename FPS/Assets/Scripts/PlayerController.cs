using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 시점 변수
    private float sensitivity = 1f;
    private float verticalRotation = 0f;

    private float mouseX;
    private float mouseY;

    // 이동 변수
    private float moveSpeed = 5f;

    private float horizontalInput;
    private float verticalInput;

    // 점프 변수
    private float jumpForce = 2f;
    private bool isGrounded = false;
    private Rigidbody rigidBody;

    // 카메라
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

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 점프
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
