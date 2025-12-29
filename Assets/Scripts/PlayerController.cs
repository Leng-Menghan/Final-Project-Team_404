using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Cinemachine Setup")]
    public Transform cameraRoot; 
    public float mouseSensitivity = 2f;
    public float lookXLimit = 85f;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 3f;
    public LayerMask interactLayer; // IMPORTANT: Set this to "Default" or create an "Interactable" layer

    [Header("Movement Settings")]
    public float walkMoveSpeed = 3.0f;
    public float runMoveSpeed = 6.0f;
    public float gravity = -9.81f;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Animation")]
    public Animator characterAnimator; // Drag your Mixamo body here

    // Internal Variables
    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;
    private Camera mainCam;
    public float jumpHeight = 1.5f;
    public KeyCode jumpKey = KeyCode.Space;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main; // Automatically finds your Main Camera
        
        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction(); // Checks for "E" key
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Debug 1: Did we click?
            Debug.Log("E Key Pressed"); 

            if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
            {
                // Debug 2: What did we hit?
                Debug.Log("I hit: " + hit.collider.name); 

                // OLD LINE (Delete this)
// DoorInteract door = hit.collider.GetComponent<DoorInteract>();

// NEW LINE (Use this)
                DoorInteract door = hit.collider.GetComponentInParent<DoorInteract>();
                
                if (door != null)
                {
                    // Debug 3: Found the script!
                    Debug.Log("Found Door Script. Opening..."); 
                    door.ToggleDoor();
                }
                else
                {
                    Debug.LogError("I hit the object, but it has no 'DoorInteract' script!");
                }
            }
            else 
            {
                Debug.Log("I missed. Too far away or wrong layer.");
            }
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit);
        
        if (cameraRoot != null)
        {
            cameraRoot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        // 1. Determine speed (Run vs Walk)
        float currentSpeed = Input.GetKey(runKey) ? runMoveSpeed : walkMoveSpeed;
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity Logic
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
//         
//         // --- REPLACE YOUR GRAVITY SECTION WITH THIS ---
//
// // 1. Reset gravity if on the ground
//         if (controller.isGrounded)
//         {
//             if (velocity.y < 0) velocity.y = -2f;
//
//             // 2. Check for Jump Input
//             if (Input.GetKeyDown(jumpKey))
//             {
//                 // Physics math to jump the exact height
//                 velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//
//                 // Trigger the animation
//                 if (characterAnimator != null)
//                 {
//                     characterAnimator.SetTrigger("Jump");
//                 }
//             }
//         }
//
// // 3. Apply Gravity
//         velocity.y += gravity * Time.deltaTime;
//         controller.Move(velocity * Time.deltaTime);

        // --- ANIMATION FIX START ---
        // --- ANIMATION FIX START ---
        if (characterAnimator != null)
        {
            float animValue = 0f;
            
            // Check if we are moving (magnitude > small number)
            if (new Vector2(x, z).sqrMagnitude > 0.01f)
            {
                // Priority 1: Are we moving BACKWARD? (z is negative)
                if (z < -0.1f)
                {
                    animValue = -1.0f; // This triggers the Backward animation
                }
                // Priority 2: Are we Running Forward?
                else if (Input.GetKey(runKey))
                {
                    animValue = 1.0f; // Run
                }
                // Priority 3: Walking Forward
                else
                {
                    animValue = 0.5f; // Walk
                }
            }

            // Send "Speed" to the Animator
            // -1 = Back, 0 = Idle, 0.5 = Walk, 1 = Run
            characterAnimator.SetFloat("Speed", animValue, 0.1f, Time.deltaTime);
        }
        // --- ANIMATION FIX END ---
        // --- ANIMATION FIX END ---
    }
}