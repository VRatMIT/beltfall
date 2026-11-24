using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 20f; // Laser speed
    public float lifetime = 5f; // How long the laser lasts before disappearing
    public GameObject asteroidFragmentPrefab; // Prefab for the smaller asteroids after collision
    public int numberOfFragments = 3; // Number of smaller asteroids spawned on collision

    private void Start()
    {
        // Destroy the laser after the specified lifetime
        Destroy(gameObject, lifetime);

        // Get the spaceship's collider
        Collider spaceshipCollider = GameObject.FindWithTag("Spaceship").GetComponent<Collider>();

        // Ignore collision between the laser and the spaceship
        Physics.IgnoreCollision(GetComponent<Collider>(), spaceshipCollider);
    }

    void Update()
    {
        // Move the laser forward along its direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if the laser hit an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Destroy the laser on collision
            Destroy(gameObject);
        }
    }
}
