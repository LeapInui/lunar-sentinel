using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int score = 0;
    public int level = 1;
    public int ammoCount = 30;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ammoCountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore();
        UpdateLevel();
        UpdateAmmoCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Doesn't update every frame
    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateLevel()
    {
        levelText.text = "Level: " + level;
    }

    public void UpdateAmmoCount()
    {
        ammoCountText.text = "Ammunition: " + ammoCount;
    }
}
