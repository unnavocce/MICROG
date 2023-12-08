using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float obstacleSpeed = 3f;

    void Update()
    {
        // Move the obstacle forward
        transform.Translate(Vector3.back * obstacleSpeed * Time.deltaTime);

        // Destroy the obstacle when it goes out of bounds
        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }
}
