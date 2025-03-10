using UnityEngine;
using UnityEngine.UIElements;

public class MeteorController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    GameObject[] structures;

    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        structures = GameObject.FindGameObjectsWithTag("Structures");
        target = structures[Random.Range(0, structures.Length)].transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
