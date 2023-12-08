using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform spawnPoint;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    void Start()
    {
        // Start spawning obstacles with random intervals
        Invoke("SpawnObstacle", Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    void SpawnObstacle()
    {
        // Instantiate an obstacle at the spawn point
        Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation);

        // Set a new random spawn interval for the next obstacle
        float nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

        // Call SpawnObstacle again with the new interval
        Invoke("SpawnObstacle", nextSpawnInterval);
    }
}
