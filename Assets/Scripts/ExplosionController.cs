using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    [SerializeField] private float destroyTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
