using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;     // Reference to asteroid prefab
    public Transform playerTransform;     // Reference to player's transform
    public float spawnRadius = 500f;      // Radius to spawn asteroids around the player
    public float despawnRadius = 1000f;   // Distance beyond which asteroids are destroyed
    public float minSpawnInterval = 0.5f; // Minimum interval between asteroid spawns
    public float maxSpawnInterval = 2.0f; // Maximum interval between asteroid spawns
    public float minScale = 1f;           // Minimum asteroid size
    public float maxScale = 5f;           // Maximum asteroid size
    public float minVelocity = 1f;        // Minimum asteroid velocity
    public float maxVelocity = 10f;       // Maximum asteroid velocity
    public float minRotation = 1f;        // Minimum asteroid rotation speed
    public float maxRotation = 10f;       // Maximum asteroid rotation speed

    private List<GameObject> asteroids = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnAsteroidsRoutine());
        StartCoroutine(CleanupAsteroidsRoutine());
    }

    IEnumerator SpawnAsteroidsRoutine()
    {
        while (true)
        {
            SpawnSingleAsteroid();

            // Wait a random interval before spawning the next asteroid
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnSingleAsteroid()
    {
        // Generate a random spawn position around the player at spawnRadius distance
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnPosition = playerTransform.position + randomDirection * spawnRadius;

        // Instantiate asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Random.rotation);

        // Randomize asteroid scale
        float scale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = Vector3.one * scale;

        // Ensure asteroid has Rigidbody
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = asteroid.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
        rb.mass = scale;

        // Ensure asteroid has Collider
        Collider col = asteroid.GetComponent<Collider>();
        if (col == null)
        {
            SphereCollider collider = asteroid.AddComponent<SphereCollider>();
            collider.isTrigger = false;
        }

        // Apply random linear velocity in global coordinates towards or around the player
        Vector3 randomVelocity = Random.insideUnitSphere.normalized * Random.Range(minVelocity, maxVelocity);
        rb.velocity = randomVelocity;

        // Apply random angular velocity
        Vector3 randomAngularVelocity = Random.insideUnitSphere.normalized * Random.Range(minRotation, maxRotation);
        rb.angularVelocity = randomAngularVelocity;

        // Add asteroid to the tracking list
        asteroids.Add(asteroid);
    }

    IEnumerator CleanupAsteroidsRoutine()
    {
        // Periodically clean up distant asteroids
        while (true)
        {
            CleanupAsteroids();
            yield return new WaitForSeconds(5f); // Clean up every 5 seconds
        }
    }

    void CleanupAsteroids()
    {
        // Remove asteroids that are far from the player
        for (int i = asteroids.Count - 1; i >= 0; i--)
        {
            if (asteroids[i] == null) // In case already destroyed
            {
                asteroids.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(playerTransform.position, asteroids[i].transform.position);
            if (distance > despawnRadius)
            {
                Destroy(asteroids[i]);
                asteroids.RemoveAt(i);
            }
        }
    }
}
