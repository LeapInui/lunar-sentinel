using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float paddingY = 1f;

    private float minX, maxX;

    public int meteorCount = 10;
    public float spawnDelay = 0.75f;

    float valueY;

    void Awake()
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).x;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;
        valueY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnMeteor()
    {
        while (meteorCount > 0)
        {
            float randomX = Random.Range(minX, maxX);

            Instantiate(meteorPrefab, new Vector3(randomX, valueY + paddingY, 0), Quaternion.identity);

            meteorCount--;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
