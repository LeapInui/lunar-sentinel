using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float paddingY = 1f;

    private float minX, maxX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).x;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;

        float randomX = Random.Range(minX, maxX);
        float valueY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        Instantiate(meteorPrefab, new Vector3(randomX, valueY + paddingY, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
