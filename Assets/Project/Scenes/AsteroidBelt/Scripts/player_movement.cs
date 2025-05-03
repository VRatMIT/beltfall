using UnityEngine;
using UnityEngine.InputSystem; // (only needed if you want direct keyboard reading)


public class PlayerMovement : MonoBehaviour
{
    private InputManager inputManager;

    // Movement and rotation speeds
    public float pitchSpeed = 10f;  // Up/Down rotation (pitch)
    public float yawSpeed = 10f;    // Left/Right rotation (yaw)
    public float thrustForce = 100f; // Forward movement speed (thrust)
    public float rollSpeed = 10f;   // Roll speed (around forward axis)

    // Damping factors for velocity and angular velocity
    public float velocityDamping = 0.95f; // Decay factor for velocity (linear damping)
    public float angularVelocityDamping = 0.95f; // Decay factor for angular velocity (rotational damping)

    // Reference to the Rigidbody attached to the camera (player)
    private Rigidbody rb;


    public GameObject laserPrefab;  // Laser prefab
    public Transform firePoint;     // Where the laser is fired from (e.g., camera center)
    public GameObject spaceshipModel; // Reference to your 3D spaceship


    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        // Get the Rigidbody attached to the Main Camera
        rb = GetComponent<Rigidbody>();

        // Ensure the camera has no gravity or physics interaction (space movement)
        rb.useGravity = false;
        rb.freezeRotation = false;  // We will handle rotation manually
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
        ApplyDamping();
        HandleLaser();
    }

    void HandleLaser()
    {
        if (inputManager.UpperTriggerPressed)  // Fire the laser when spacebar is pressed
        {
            FireLaser();
        }
    }
    void FireLaser()
    {
        // Instantiate the laser at the firePoint's position and rotation
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        // Ensure the laser moves in the forward direction of the spaceship
        laser.transform.forward = spaceshipModel.transform.forward;
    }

    void HandleRotation()
    {
        float pitchInput = 0f;
        float yawInput = 0f;
        float rollInput = 0f;

        Vector2 move = inputManager.MoveDirection; // ← use the existing inputManager

        // Use MoveDirection for pitch and yaw
        pitchInput = -move.y; // Up on stick (or UpArrow) = negative pitch (look down)
        yawInput = move.x;    // Left/Right on stick (or LeftArrow/RightArrow)

        // Roll is controlled separately by keyboard (A/D) for now
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) rollInput = 1f;   // Roll left
            if (Keyboard.current.dKey.isPressed) rollInput = -1f;  // Roll right
        }

        // Apply rotations
        rb.AddTorque(transform.up * yawInput * yawSpeed * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddTorque(transform.right * pitchInput * pitchSpeed * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddTorque(transform.forward * rollInput * rollSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }

    void HandleMovement()
    {
        // Handle forward movement (thrust)
        if (inputManager.LowerTriggerPressed)
        {
            // Apply forward thrust when shift key is held down
            rb.AddForce(transform.forward * thrustForce * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    void ApplyDamping()
    {
        // Apply linear damping to slow down the ship when no thrust is applied
        if (!inputManager.LowerTriggerPressed)
        {
            // Gradually reduce velocity (opposite force to current velocity)
            rb.velocity *= velocityDamping;
        }

        // Apply angular damping to slow down the rotation when no input is applied
        // if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        // {
        if (!inputManager.JoystickPushed) {
            // Gradually reduce angular velocity (opposite torque to current angular velocity)
            rb.angularVelocity *= angularVelocityDamping;
        }
    }
}