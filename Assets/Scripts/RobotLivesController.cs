using UnityEngine;

public class RobotLivesController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer lifeImage;
    [SerializeField] private Sprite life2Sprite;
    [SerializeField] private Sprite life1Sprite;

    // This method will update the UI based on the number of lives
    public void UpdateLives(int currentLives)
    {
        if (currentLives == 2)
        {
            lifeImage.sprite = life2Sprite;
        }
        else
        {
            lifeImage.sprite = life1Sprite;
        }
    }
}
