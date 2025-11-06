using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab;

    [Header("Spawn Points (5 locations)")]
    public Transform[] spawnPoints = new Transform[5];

    private GameObject currentPlayer;

    // --- Exception Control ---
    private static int exceptionCount = 0;
    private const int maxExceptions = 2;

    void Start()
    {
        try
        {
            SpawnPlayerAtRandomLocation();
        }
        catch (System.Exception ex)
        {
            HandleException(ex);
        }
    }

    void Update()
    {
        // Example: respawn with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            try
            {
                SpawnPlayerAtRandomLocation();
            }
            catch (System.Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    public void SpawnPlayerAtRandomLocation()
    {
        if (playerPrefab == null)
            throw new System.Exception("Player prefab not assigned in PlayerSpawner!");

        if (spawnPoints == null || spawnPoints.Length == 0)
            throw new System.Exception("No spawn points assigned in PlayerSpawner!");

        // Choose a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawn = spawnPoints[randomIndex];

        if (chosenSpawn == null)
            throw new System.Exception($"Spawn point at index {randomIndex} is null!");

        // Destroy old player (if any)
        if (currentPlayer != null)
            Destroy(currentPlayer);

        // Spawn the player
        currentPlayer = Instantiate(playerPrefab, chosenSpawn.position, chosenSpawn.rotation);
    }

    private void HandleException(System.Exception ex)
    {
        exceptionCount++;

        if (exceptionCount <= maxExceptions)
        {
            Debug.LogError($"[PlayerSpawner Exception {exceptionCount}/{maxExceptions}] {ex.Message}");
        }
        else
        {
            // Suppress further exceptions
            Debug.LogWarning("Additional exceptions suppressed to avoid console spam.");
        }
    }
}