using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    public GameObject healthPackPrefab;
    public GameObject ammoPrefab;
    public GameObject powerUpPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f; // Time between spawns
    public int maxItems = 10; // Max number of items in the scene
    public Vector3 spawnAreaSize = new Vector3(10f, 1f, 10f);

    private float spawnTimer;
    private int currentItemCount;

    // Exception control
    private int exceptionCount = 0;
    private const int maxExceptions = 2;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnItem();
            spawnTimer = spawnInterval;
        }
    }

    void TrySpawnItem()
    {
        try
        {
            // Example intentional error condition (simulate gameplay exception)
            if (Random.value < 0.1f) // 10% chance to test exception handling
                ThrowControlledException("Random test exception in TrySpawnItem");

            currentItemCount = GameObject.FindGameObjectsWithTag("PickUp").Length;

            if (currentItemCount < maxItems)
            {
                SpawnRandomItem();
            }
        }
        catch (System.Exception ex)
        {
            HandleException(ex);
        }
    }

    void SpawnRandomItem()
    {
        try
        {
            // Choose random item type
            int randomChoice = Random.Range(0, 3); // 0 = health, 1 = ammo, 2 = power-up
            GameObject itemToSpawn = null;

            switch (randomChoice)
            {
                case 0:
                    itemToSpawn = healthPackPrefab;
                    break;
                case 1:
                    itemToSpawn = ammoPrefab;
                    break;
                case 2:
                    itemToSpawn = powerUpPrefab;
                    break;
            }

            // Simulate another intentional exception (for testing)
            if (itemToSpawn == null)
                ThrowControlledException("Attempted to spawn a null item prefab!");

            // Random position within spawn area
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            Vector3 spawnPos = transform.position + randomPos;

            // Spawn the item
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
        }
        catch (System.Exception ex)
        {
            HandleException(ex);
        }
    }

    void ThrowControlledException(string message)
    {
        throw new System.Exception(message);
    }

    void HandleException(System.Exception ex)
    {
        // Only allow up to two printed exceptions
        if (exceptionCount < maxExceptions)
        {
            exceptionCount++;
            Debug.LogError($"[Controlled Exception #{exceptionCount}] {ex.Message}");
        }
        else
        {
            // Suppress any further exception spam
            Debug.LogWarning("Further exceptions suppressed to prevent console spam.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}