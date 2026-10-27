using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private int mouseSens;

    [SerializeField] private float walkSpeed;

    private float verticalRotation;
    [SerializeField] private float upDownRange;

    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity = 9.81f;

    private Vector3 currentMovement = Vector3.zero;
    public Camera mainCamera;
    public GameObject cameraHolder;
    public Animator weaponHolderAnimator;
    void Start()
    {
        mainCamera = Camera.main;
        cameraHolder = GameObject.Find("CameraHolder");
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
    }

    private void HandleMovement()
    {
        float verticalSpeed = Input.GetAxis("Vertical");
        float horizontalSpeed = Input.GetAxis("Horizontal");

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);

        if (horizontalMovement.magnitude > 1)
        {
            horizontalMovement.Normalize();
        }

        horizontalMovement = transform.rotation * horizontalMovement * walkSpeed;       

        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;

        characterController.Move(currentMovement * Time.deltaTime);

        if (characterController.isGrounded)
        {
            weaponHolderAnimator.SetFloat("PlayerSpeed", horizontalMovement.magnitude);
        }
        else
        {
            weaponHolderAnimator.SetFloat("PlayerSpeed", 0);
        }

    }

    private void HandleRotation()
    {
        float mouseXrotation = Input.GetAxis("Mouse X") * mouseSens;
        transform.Rotate(0,mouseXrotation,0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSens;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cameraHolder.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentMovement.y = jumpForce;
            }
        }

        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }
}
