//
// This script executes players movement relative to the camera's orientation.Accelerate and decelerate the easing of player when stopping and moving off. Allows to speed up when pressing SHIFT button.
//
// To mae this script work on a player, create an object that is going to be a main player and drop this script into inspector of the player object.
// In script menu set the values for the walk/run speed,how smooth the easing is, and relative camera for the direction  of the player.
//
//
using UnityEngine;


public class Walking : MonoBehaviour, IHealth
{
    private Animator animator;
    public float walkSpeed = 8f; // walking speed
    public float runSpeed = 8f; // running speed
    public float smoothTime = 0.05f; // how smooth the easing in and out when moving out and stopping
    public float rotationSpeed = 12f;

    public Transform cameraTransform; // reference to the camera for movement direction

    private Vector3 currentVelocity; // current velocity of the player
    private Vector3 velocitySmoothRef;
    public bool isAttacking; //


    public float maxHealth = 100f;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    void Update()
    {


        if (isAttacking)
        {
            // Completely stop movement while attacking
            currentVelocity = Vector3.zero;
            velocitySmoothRef = Vector3.zero;
            return;
        }

        HandleMovement();

    }
    void HandleMovement()
    {
        float horizontal = 0f;
        float vertical = 0f;
        // old and modified WASD input
        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        // Camera-relative directions
        Vector3 camForward = cameraTransform.forward; // foward direction eg. vertical
        Vector3 camRight = cameraTransform.right; // right direction eg. horizontal

        // Flatten camera vectors -setting camera forward vector's y components to 0 so that the player won't move up and down when the camera is looking up and down, same for right vector
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputDir = (camForward * vertical + camRight * horizontal).normalized;

        float animationSpeed = inputDir.magnitude;

        if (animationSpeed < 0.1f)
            animationSpeed = 0f;

        animator.SetFloat("Speed", animationSpeed, 0.1f, Time.deltaTime);

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // Shift to run else walk using ? operator

        Vector3 targetVelocity = inputDir * targetSpeed;

        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        currentVelocity = Vector3.SmoothDamp( // Smooth acceleration / deceleration of a player
            currentVelocity,
            targetVelocity,
            ref velocitySmoothRef,
            smoothTime
        );


        transform.Translate(currentVelocity * Time.deltaTime, Space.World); // Move
    }
    public void SetCamera(Transform newCamera)
    {
        cameraTransform = newCamera; // set new camera for movement
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}

