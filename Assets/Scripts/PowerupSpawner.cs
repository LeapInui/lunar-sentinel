using System.Collections;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PowerupPrefab
    {
        public GameObject prefab;
        public PowerupType type;
    }

    [SerializeField] private PowerupPrefab[] powerupPrefabs;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnChance = 0.2f;
    [SerializeField] private float paddingY = 1f;

    private float minX, maxX;
    private float valueY; 

    private int totalBuildingCount;
    private int totalRobotCount;

    void Awake()
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).x;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;
        valueY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalBuildingCount = FindObjectsByType<Building>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length;
        totalRobotCount = FindObjectsByType<RobotController>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length;

        StartCoroutine(SpawnPowerups());
    }

    public IEnumerator SpawnPowerups()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Skip spawning powerups if round is over or game is over
            GameController gameController = ServiceLocator.Get<GameController>();
            if (gameController.gameOver || gameController.isRoundOver)
            {
                continue;
            }

            int currentBuildingCount = FindObjectsByType<Building>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
            int currentRobotCount = FindObjectsByType<RobotController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;

            if (Random.value <= spawnChance)
            {
                // List to store available powerups
                var availablePowerups = new System.Collections.Generic.List<GameObject>();

                foreach (var powerup in powerupPrefabs)
                {
                    bool isAvailable = true;
                    
                    // Don't spawn building powerup if all buildlings are active
                    if (powerup.type == PowerupType.Building && currentBuildingCount >= totalBuildingCount) 
                    {
                        isAvailable = false;
                    }
                    // Don't spawn robot powerup if all robots are active
                    if (powerup.type == PowerupType.Robot && currentRobotCount >= totalRobotCount)
                    {
                        isAvailable = false;
                    }
                    // Add other powerups
                    if (isAvailable)
                    {
                        availablePowerups.Add(powerup.prefab);
                    }
                }

                if (availablePowerups.Count > 0)
                {
                    float randomX = Random.Range(minX, maxX);
                    int randomIndex = Random.Range(0, availablePowerups.Count);

                    Instantiate(availablePowerups[randomIndex], new Vector3(randomX, valueY + paddingY, -0.5f), Quaternion.identity);
                }
            }
        }
    }
}
