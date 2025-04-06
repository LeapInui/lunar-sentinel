using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    private Vector2 target;
    [SerializeField] private GameObject bulletExplosion;
    [SerializeField] private float speed = 5f;

    public void SetTarget(Vector2 newTarget)
    {
        target = newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == (Vector3)target)
        {
            Instantiate(bulletExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }   
}
