using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public int maxJumpCount = 2;
    private int jumpCount;

    [Header("Sprint Settings")]
    public float sprintMultiplier = 1.5f;
    public float slopeClimbSpeed = 0.5f;
    public float defaultSlopeLimit = 45f;

    [Header("Mouse Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;

    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip stompSound;
    public AudioClip deathSound;

    private CharacterController controller;
    private AudioSource audioSource;
    private Vector3 velocity;
    private float xRotation = 0f;
    private Coroutine jumpForceChange = null;
    private float currentJumpHeight;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentJumpHeight = jumpHeight;
        jumpCount = 0;
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
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Movement direction (based on camera orientation)
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float speed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
        controller.Move(move * speed * Time.deltaTime);

        // Ground check
        if (controller.isGrounded)
        {
            velocity.y = -2f;
            jumpCount = 0; // Reset jumps when grounded
        }

        // Jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity);
            jumpCount++;
            PlaySound(jumpSound);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Slope handling
        if (controller.collisionFlags == CollisionFlags.Sides)
        {
            controller.slopeLimit = 90f;
            controller.Move(move * (moveSpeed * slopeClimbSpeed) * Time.deltaTime);
        }
        else
        {
            controller.slopeLimit = defaultSlopeLimit;
        }

        // Special: sprint jump boost
        if (Input.GetButtonDown("Jump") && controller.isGrounded && isSprinting)
        {
            velocity.y = Mathf.Sqrt(currentJumpHeight * 1.5f * -2f * gravity);
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

    public void ActivateJumpForceChange()
    {
        if (jumpForceChange != null)
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            currentJumpHeight = jumpHeight;
        }

        jumpForceChange = StartCoroutine(ChangeJumpForce());
    }

    private IEnumerator ChangeJumpForce()
    {
        currentJumpHeight = jumpHeight * 2f;
        Debug.Log($"Jump force temporarily increased to {currentJumpHeight} at {Time.time}");
        yield return new WaitForSeconds(5f);
        currentJumpHeight = jumpHeight;
        Debug.Log($"Jump force reset to {jumpHeight} at {Time.time}");
        jumpForceChange = null;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.lives--;
            PlaySound(deathSound);
        }

        if (other.CompareTag("Squish"))
        {
            PlaySound(stompSound);
            velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.Victory();
        }
    }
}