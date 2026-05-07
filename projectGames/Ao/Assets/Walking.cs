//
// This script executes players movement relative to the camera's orientation.Accelerate and decelerate the easing of player when stopping and moving off. Allows to speed up when pressing SHIFT button.
//
// To make this script work on a player, create an object that is going to be a main player and drop this script into inspector of the player object.
// In script menu set the values for the walk/run speed,how smooth the easing is, and relative camera for the direction  of the player.
//
//
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class Walking : MonoBehaviour, IHealth
{
    private Animator animator;
    private float walkSpeed = 8f; // walking speed
    private float runSpeed = 8f; // running speed
    private float smoothTime = 0.05f; // how smooth the easing in and out when moving out and stopping
    private float rotationSpeed = 12f;

    public Transform cameraTransform; // reference to the camera for movement direction

    public Vector3 currentVelocity; // current velocity of the player
    public Vector3 velocitySmoothRef;
    public bool isAttacking; //

    Collider target;

    public float maxHealth = 100f;
    public float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public PostProcessVolume postProcessVolume;

    private Vignette vignette;

    public float burstSpeed = 13f;
    public float burstDuration = 0.5f;
    public float cooldown = 0.8f;

    private float burstTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isBursting = false;

    float doubleTapTime = 0.3f;

    float lastWTime, lastATime, lastSTime, lastDTime;

    Vector3 burstDirection;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        postProcessVolume.profile.TryGetSettings(out vignette);
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] AllObjects = Physics.OverlapSphere(transform.position+transform.forward, 2f);
            foreach (Collider col in AllObjects)
            {
                IPickUp pickUp = col.GetComponent<IPickUp>();
                if (pickUp != null)
                {
                    
                    if (pickUp is Food)
                    {
                       (pickUp as Food).Eat();
                        RestoreHealth(15f);
                    }
                    else if (pickUp is PowerUp)
                    {
                        (pickUp as PowerUp).UsePowerUp();
                    }
                    pickUp.PickUp();
                }
            }
        }

        CheckDoubleTap();
        HandleBurst();
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

        float targetSpeed = isBursting ? burstSpeed : walkSpeed; // Shift to run else walk using ? operator
        Vector3 moveDir = isBursting ? burstDirection : inputDir;
        Vector3 targetVelocity = moveDir * targetSpeed;

        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime
            );
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);

        }


        currentVelocity = Vector3.SmoothDamp(currentVelocity,targetVelocity,ref velocitySmoothRef,smoothTime);// Smooth acceleration / deceleration of a player


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

    public void RestoreHealth(float healthAdd)
    {
        currentHealth += healthAdd;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void StartBurst(Vector3 dir)
    {
        isBursting = true;
        burstTimer = burstDuration;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        burstDirection = (camForward * dir.z + camRight * dir.x).normalized;

        animator.SetTrigger("Sprint");
    }
    public void CheckDoubleTap()
    {
        int keyCount = 0;

        if (Input.GetKey(KeyCode.W)) keyCount++;
        if (Input.GetKey(KeyCode.A)) keyCount++;
        if (Input.GetKey(KeyCode.S)) keyCount++;
        if (Input.GetKey(KeyCode.D)) keyCount++;

        if (keyCount != 1) return;

        if (cooldownTimer > 0f || isBursting) 
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastWTime < doubleTapTime)
                StartBurst(Vector3.forward);
            lastWTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.time - lastSTime < doubleTapTime)
                StartBurst(Vector3.back);
            lastSTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastATime < doubleTapTime)
                StartBurst(Vector3.left);
            lastATime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastDTime < doubleTapTime)
                StartBurst(Vector3.right);
            lastDTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(5);
        }
    }
    public void HandleBurst()
    {

        if (isBursting) // While bursting
        {
            burstTimer -= Time.deltaTime;

            if (burstTimer <= 0f)
            {
                isBursting = false;
                cooldownTimer = cooldown;
            }
        }
        else
        {
            
            if (cooldownTimer > 0f)  // Cooldown ticking
                cooldownTimer -= Time.deltaTime;
        }
    }
}

