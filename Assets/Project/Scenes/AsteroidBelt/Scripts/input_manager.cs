using UnityEngine;
using UnityEngine.InputSystem; // Keyboard input
using UnityEngine.XR;          // XRNode (for controller stuff)
using UnityEngine.XR.Interaction.Toolkit; // If using XR Interaction Toolkit

public class InputManager : MonoBehaviour
{
    // Public actions your game can use
    
    public bool JoystickPushed { get; private set; }
    public bool UpperTriggerPressed { get; private set; }
    public bool LowerTriggerPressed { get; private set; }
    public Vector2 MoveDirection { get; private set; }

    // InputAction references for VR (drag from Input Actions in Inspector)
    public InputActionProperty rightJoystick;    // Stick movement
    public InputActionProperty rightTrigger;     // Main trigger (upper trigger)
    public InputActionProperty rightGrip;        // Grip trigger (lower trigger)

    void Update()
    {
        // Reset every frame
        JoystickPushed = false;
        UpperTriggerPressed = false;
        LowerTriggerPressed = false;
        MoveDirection = Vector2.zero;
        Debug.Log($"Grip value: {rightGrip.action.ReadValue<float>()}");


        // ---------------- Keyboard Mapping ----------------
        if (Keyboard.current != null)
        {
            // Arrow keys map to right joystick
            if (Keyboard.current.upArrowKey.isPressed)
                MoveDirection += Vector2.up;
            if (Keyboard.current.downArrowKey.isPressed)
                MoveDirection += Vector2.down;
            if (Keyboard.current.leftArrowKey.isPressed)
                MoveDirection += Vector2.left;
            if (Keyboard.current.rightArrowKey.isPressed)
                MoveDirection += Vector2.right;

            // Space bar maps to upper trigger
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                UpperTriggerPressed = true;

            // Left Shift maps to lower trigger
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
                LowerTriggerPressed = true;
        }

        // ---------------- VR Controller Mapping ----------------
        if (rightJoystick.action != null)
        {
            JoystickPushed = true;
            MoveDirection += rightJoystick.action.ReadValue<Vector2>();
        }

        if (rightTrigger.action != null && rightTrigger.action.ReadValue<float>() > 0.5f)
        {
            UpperTriggerPressed = true;
        }

        if (rightGrip.action != null && rightGrip.action.ReadValue<float>() > 0.5f)
        {
            LowerTriggerPressed = true;
            Debug.Log("pressed");
        }
    }

    public void esp_message_received(string message)
    {
        try
        {
            string[] parts = message.Split(',');
            if (parts.Length != 5)
            {
                Debug.LogWarning($"ESP message has incorrect format: {message}");
                return;
            }

            // Parse gyro data (currently unused but parsed in case you want it later)
            float gyroX = float.Parse(parts[0]);
            float gyroY = float.Parse(parts[1]);
            float gyroZ = float.Parse(parts[2]);

            // Parse buttons
            int shootButton = int.Parse(parts[3]);
            int accelButton = int.Parse(parts[4]);

            // Set inputs
            UpperTriggerPressed = (shootButton == 1);
            LowerTriggerPressed = (accelButton == 1);

            // You could also use gyro data to simulate joystick movement if you want
            // For now, example: map gyro x/y to MoveDirection
            MoveDirection = new Vector2(gyroX, gyroY);

            // Since ESP is sending input, assume joystick is active
            JoystickPushed = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing ESP message: {message} - {ex.Message}");
        }
    }

    public void onConnected()
{
    Debug.Log("Connected to ESP32!");
}

public void onMessageReceived(string message) // <<< Spelling fixed here!
{
    Debug.Log("ESP32 Message received: " + message);
    esp_message_received(message); 
}

public void onError(string errorMessage)
{
    Debug.LogError("Error with ESP32 connection: " + errorMessage);
}
}
