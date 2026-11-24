using UnityEngine;

public class ESPReceiver : MonoBehaviour
{
    public void onConnected()
    {
        Debug.Log("Connected!");
    }

    public void onMessageRecieved(string message)
    {
        Debug.Log("Message received: " + message);
    }

    public void onError(string errorMessage)
    {
        Debug.LogError("Error: " + errorMessage);
    }
}
