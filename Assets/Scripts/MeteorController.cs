using System.Runtime.InteropServices;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bulletExplosion;
    [SerializeField] private GameObject meteorExplosion;
    GameObject[] structures;

    private GameController gameController;

    Vector3 target;
    private bool isDestroyed = false; // Prevents multiple scoring

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        structures = GameObject.FindGameObjectsWithTag("Structures");
        target = structures[Random.Range(0, structures.Length)].transform.position;

        speed = gameController.meteorSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (target - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Threshold used to check if meteor reached the target due to floating point inaccuracies
        if (Vector2.Distance(transform.position, target) < 0.1f) 
        {
            Instantiate(meteorExplosion, transform.position, Quaternion.identity);
            DestroyMeteor();
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // When the meteor collides
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Prevent multiple hits
        if (isDestroyed) return; 

        // Buildings + robots
        if (collider.tag == "Structures")
        {
            Vector3 explosionPosition = collider.transform.position + new Vector3(0, 0, -0.5f);
            Instantiate(meteorExplosion, explosionPosition, Quaternion.identity);
            DestroyMeteor();

            RobotController robotController = collider.GetComponent<RobotController>();
            if (collider.GetComponent<RobotController>() != null)
            {
                robotController.TakeDamage();
                return;
            }
            
            Destroy(collider.gameObject);
        } 
        // The bullet explosions fired by the player
        else if (collider.tag == "Explosions")
        {
            isDestroyed = true;
            gameController.UpdateScorePoints();
            Instantiate(bulletExplosion, collider.transform.position, Quaternion.identity);
            DestroyMeteor();
        }
    }

    private void DestroyMeteor()
    {
        gameController.MeteorDestroyed();
        Destroy(gameObject);
    }
}
