using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Flip(Vector2 targetPosition)
    {
        spriteRenderer.flipX = targetPosition.x < transform.position.x;
    }
}
