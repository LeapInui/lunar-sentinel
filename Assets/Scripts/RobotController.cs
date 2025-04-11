using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameController gameController;
    [SerializeField] private RobotLivesController livesUi;
    [SerializeField] private GameObject shieldPrefab;

    private int maxLives = 2;
    private int currentLives;
    public bool hasShield = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLives = maxLives;
        livesUi.UpdateLives(currentLives);
    }

    public void ActivateShield()
    {
        if (!hasShield)
        {
            hasShield = true;
            shieldPrefab.SetActive(true);
        }
    }

    public void TakeDamage()
    {
        if (hasShield)
        {
            hasShield = false;
            shieldPrefab.SetActive(false);
            return;
        }

        currentLives--;
        livesUi.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            gameController.robotCounter--;
            gameObject.SetActive(false);
        }
    }

    public void EnableRobot()
    {
        gameObject.SetActive(true);
        currentLives = maxLives;
        livesUi.UpdateLives(currentLives);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void Flip(Vector2 targetPosition)
    {
        spriteRenderer.flipX = targetPosition.x < transform.position.x;
    }
}
