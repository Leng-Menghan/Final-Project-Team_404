using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    public float lookXLimit = 80f; // Stops head from spinning 360

    [Header("Animation (Optional)")]
    public Animator characterAnimator; // Drag Mixamo Animator here

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Calculate Movement (WASD)
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");
        
        // Keep the Y velocity (jumping/gravity) separate
        float movementDirectionY = moveDirection.y;
        
        // Combine directions
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // 2. Jumping Logic
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // 3. Apply Gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        // 4. Move the Controller
        characterController.Move(moveDirection * Time.deltaTime);

        // 5. Camera Rotation (Up/Down)
        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // 6. Body Rotation (Left/Right)
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);

        // 7. Update Animator (If you have one)
        if (characterAnimator != null)
        {
            // Calculate speed for animation (ignoring up/down movement)
            float horizontalSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
            characterAnimator.SetFloat("Speed", horizontalSpeed);
        }
    }
}