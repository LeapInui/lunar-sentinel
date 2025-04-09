using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupType powerupType;
    [SerializeField] private float fallSpeed = 2.5f;

    private bool isDestroyed = false; // Prevents powerup activating multiple times

    private GameController gameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Destroy if it hits building level (y = -4)
        if (transform.position.y <= -4f)
        {
            Destroy(gameObject);
        }
    }

    // Collision handling
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Prevent multiple hits
        if (isDestroyed) return; 

        if (collider.tag == "PlayerExplosion" && !gameController.gameOver)
        {
            isDestroyed = true;
            ApplyPowerup();
            Destroy(gameObject);
        }
    }

    // Apply the powerup depending on the type
    private void ApplyPowerup()
    {
        switch (powerupType)
        {
            case PowerupType.Building:
                RestoreBuilding();
                break;
            case PowerupType.Robot:
                RestoreRobot();
                break;
        }
    }

    // Implementation of building powerup
    private void RestoreBuilding()
    {
        Building[] allBuildings = FindObjectsByType<Building>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Building building in allBuildings)
        {
            if (!building.IsActive())
            {
                building.EnableBuilding();
                break; // Only restores one building
            }
        }
    }

    // Implementation of robot powerup
    private void RestoreRobot()
    {
        RobotController[] allRobots = FindObjectsByType<RobotController>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (RobotController robot in allRobots)
        {
            if (!robot.IsActive())
            {
                robot.EnableRobot();
                gameController.robotCounter++;
                break;
            }
        }
    }
}
