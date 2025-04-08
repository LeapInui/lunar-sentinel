using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameController gameController;
    [SerializeField] private RobotLivesController livesUi;

    private int maxLives = 2;
    private int currentLives;
    private bool isUpdating = false; // Flag to check update status

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLives = maxLives;
        livesUi.UpdateLives(currentLives);
    }

    public void TakeDamage()
    {
        if (isUpdating) return;
        isUpdating = true;

        currentLives--;
        livesUi.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            gameController.robotCounter--;
            gameObject.SetActive(false);
        }

        isUpdating = false;
    }

    public void EnableRobot()
    {
        gameObject.SetActive(true);
        currentLives = maxLives;
        livesUi.UpdateLives(currentLives);
    }

    public void Flip(Vector2 targetPosition)
    {
        if (isUpdating) return;
        spriteRenderer.flipX = targetPosition.x < transform.position.x;
    }
}
