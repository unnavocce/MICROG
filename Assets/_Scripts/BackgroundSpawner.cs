using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
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
        // Select a random obstacle prefab from the array
        GameObject selectedObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Instantiate the selected obstacle at the spawn point
        Instantiate(selectedObstacle, spawnPoint.position, spawnPoint.rotation);

        // Set a new random spawn interval for the next obstacle
        float nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

        // Call SpawnObstacle again with the new interval
        Invoke("SpawnObstacle", nextSpawnInterval);
    }
}
