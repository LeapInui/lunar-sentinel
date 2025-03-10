using UnityEngine;

public class MeteorController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject bulletExplosion;
    [SerializeField] private GameObject meteorExplosion;
    GameObject[] structures;

    Vector3 target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        structures = GameObject.FindGameObjectsWithTag("Structures");
        target = structures[Random.Range(0, structures.Length)].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (target - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Structures")
        {
            Instantiate(meteorExplosion, collider.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(collider.gameObject);
        } else if (collider.tag == "Explosions")
        {
            Instantiate(bulletExplosion, collider.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(collider.gameObject);
        }
    }
}
