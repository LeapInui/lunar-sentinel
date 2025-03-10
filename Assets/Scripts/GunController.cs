using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void RotateGun(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)target.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        target.rotation = Quaternion.Euler(0, 0, angle);

        spriteRenderer.flipY = targetPosition.x < target.position.x;
    }
}
