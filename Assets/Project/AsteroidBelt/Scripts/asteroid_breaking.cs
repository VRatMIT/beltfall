using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float minSize = 0.5f;

    public int minFragments = 3;
    public int maxFragments = 5;
    public float breakForce = 10f;
    public GameObject explosionEffect = null;
    private bool hasExploded = false;

    public void BreakIntoSmallerAsteroids()
    {
        if (hasExploded) return;
        hasExploded = true;

        Explode();

        float currentSize = transform.localScale.x;

        if (currentSize > minSize)
        {
            for (int i = 0; i < maxFragments; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                GameObject smallAsteroid = Instantiate(gameObject, transform.position, Random.rotation);
                float newScale = currentSize * (1.0f/ Mathf.Pow((float) maxFragments, 1f / 3f));
                smallAsteroid.transform.localScale = new Vector3(newScale, newScale, newScale);

                Rigidbody rb = smallAsteroid.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = randomDirection * breakForce;
                }

            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            BreakIntoSmallerAsteroids();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}
