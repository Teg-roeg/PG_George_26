//
// This script executes players movement relative to the camera's orientation.Accelerate and decelerate the easing of player when stopping and moving off. Allows to speed up when pressing SHIFT button.
//
// To mae this script work on a player, create an object that is going to be a main player and drop this script into inspector of the player object.
// In script menu set the values for the walk/run speed,how smooth the easing is, and relative camera for the direction  of the player.
//
//
using UnityEngine;


public class Walking : MonoBehaviour
{
    public float walkSpeed = 6f; // walking speed
    public float runSpeed = 8f; // running speed
    public float smoothTime = 0.05f; // how smooth the easing in and out when moving out and stopping

    public Transform cameraTransform; // reference to the camera for movement direction

    private Vector3 currentVelocity; // current velocity of the player
    private Vector3 velocitySmoothRef; 

    void Update()
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

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // Shift to run else walk using ? operator

        Vector3 targetVelocity = inputDir * targetSpeed;

        
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
}
