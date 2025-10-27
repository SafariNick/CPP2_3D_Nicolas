using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
   
    [Header("Mouse Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private Rigidbody rb;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        LookAround();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // keeps player grounded

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //if player presses left shift, increase move speed by 50%
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * moveSpeed * 1.5f * Time.deltaTime);
        }
        //if player jumps while sprinting, increase jump height by 50%
        if (Input.GetButtonDown("Jump") && controller.isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 1.5f * -2f * gravity);
        }
        //if player collides with the slope limit, remove it while allow player to slowly climb the slope
        if (controller.collisionFlags == CollisionFlags.Sides)
        {
            controller.slopeLimit = 90f;
            //change movement to be slower while climbing the slope
            controller.Move(move * (moveSpeed / 2) * Time.deltaTime);
        }
        else
        {
            controller.slopeLimit = 45f;
            //change movement back to normal speed
            controller.Move(move * moveSpeed * Time.deltaTime);
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    //if player collides with an object tagged "PickUp", destroy the object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
        }
    }
}