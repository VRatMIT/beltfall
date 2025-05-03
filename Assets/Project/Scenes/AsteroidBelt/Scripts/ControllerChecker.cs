using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerChecker : MonoBehaviour
{
    public InputActionProperty leftTrigger;
    public InputActionProperty rightTrigger;

    void Update()
    {
        if (leftTrigger.action != null && leftTrigger.action.WasPressedThisFrame())
        {
            Debug.Log("L");
        }

        if (rightTrigger.action != null && rightTrigger.action.WasPressedThisFrame())
        {
            Debug.Log("R");
        }
    }
}
