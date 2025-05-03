using UnityEngine;

public class PlayerLaserController : MonoBehaviour
{
    public GameObject laserPrefab;  // Laser prefab
    public Transform firePoint;     // Where the laser is fired from (e.g., camera center)
    public Camera playerCamera;     // The camera to get forward direction

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Fire the laser when spacebar is pressed
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        // Instantiate the laser at the firePoint's position and rotation
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        // Ensure the laser moves in the forward direction of the camera
        laser.transform.forward = playerCamera.transform.forward;
    }
}
