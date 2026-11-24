using UnityEngine;

public class MySingularityEvents : MonoBehaviour
{
    // If the cube this script controls is a different object, drag it into this field in the Inspector
    public GameObject cube; 

    public void onConnected()
    {
        Debug.Log("Connected to device!");
    }

   public void onMessageRecieved(string message)
{
    // Debug.Log("Message received! " + message);
    Debug.Log("Length: " + message.Length);
    
    // Parse the CSV message to extract Euler angles

    string[] values = message.Split(',');
    Debug.Log(values.Length);
    // Make sure we have exactly 3 values for x, y, and z
    if (values.Length >= 3)
    {
        // Try to convert the string values to float
        if (float.TryParse(values[0], out float x) && 
            float.TryParse(values[1], out float y) && 
            float.TryParse(values[2], out float z))
        {
            // Apply rotation to the cube
            Debug.Log("Rotating Cube!!!");
            transform.eulerAngles = new Vector3(x, y, z);
        }
    }
}


    public void onError(string errorMessage)
    {
        Debug.LogError("Error with Singularity: " + errorMessage);
    }
}
