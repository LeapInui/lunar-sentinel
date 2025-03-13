using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void RotateGun(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        spriteRenderer.flipY = targetPosition.x < transform.position.x;
    }
}
