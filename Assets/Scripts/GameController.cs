using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject roundEndPanel;
    [SerializeField] private GameObject gameOverPanel;
    MeteorSpawner meteorSpawner;

    public int score = 0;
    public int level = 1;

    public float meteorSpeed = 2f;
    [SerializeField] private float meteorSpeedMultiplier = 0.1f;

    public int robotCounter;
    public int ammoCount = 30;
    private int totalMeteorCount = 10;
    private int meteorsLeftCount = 0;

    // Score Values
    private int meteorDestroyPoints = 50;
    [SerializeField] private int ammoRemainingBonus = 5;
    [SerializeField] private int bulidingsRemainingBonus = 100;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ammoCountText;

    [SerializeField] private TextMeshProUGUI ammoRemainingBonusText;
    [SerializeField] private TextMeshProUGUI buildingsRemainingText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private TextMeshProUGUI endScoreText;

    private bool isRoundOver = false;
    public bool gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meteorSpawner = FindFirstObjectByType<MeteorSpawner>();
        robotCounter = FindObjectsByType<RobotController>(FindObjectsSortMode.None).Length;

        UpdateScoreText();
        UpdateLevelText();
        UpdateAmmoCountText();

        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (meteorsLeftCount <= 0 && !isRoundOver && !gameOver)
        {
            isRoundOver = true;
            StartCoroutine(EndRound());
        }

        if (robotCounter <= 0) {
            gameOver = true;
            StartCoroutine(GameOver());
        }
    }

    // Doesn't update every frame
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateLevelText()
    {
        levelText.text = "Level: " + level;
    }

    public void UpdateAmmoCountText()
    {
        ammoCountText.text = "Ammunition: " + ammoCount;
    }

    public void UpdateScorePoints()
    {
        score += meteorDestroyPoints;
        UpdateScoreText();
    }

    public void MeteorDestroyed()
    {
        meteorsLeftCount--;
    }

    private void StartRound()
    {
        meteorSpawner.meteorCount = totalMeteorCount;
        meteorsLeftCount = totalMeteorCount;
        meteorSpawner.StartCoroutine(meteorSpawner.SpawnMeteor());
    }

    private IEnumerator Countdown()
    {
        countdownText.text = "5";
        yield return new WaitForSeconds(1f);
        countdownText.text = "4";
        yield return new WaitForSeconds(1f);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator EndRound()
    {
        yield return new WaitForSeconds(0.5f);
        roundEndPanel.SetActive(true);

        int ammoRemainingScore = ammoCount * ammoRemainingBonus;

        Building[] buildings = FindObjectsByType<Building>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        int bulidingsRemainingScore = buildings.Length * bulidingsRemainingBonus;

        int totalBonus = ammoRemainingScore + bulidingsRemainingScore;

        ammoRemainingBonusText.text = "Ammunition Remaining Bonus: " + ammoRemainingScore;
        buildingsRemainingText.text = "Buildings Remaining Bonus: " + bulidingsRemainingScore;
        totalScoreText.text = "Total Score: " + score + " + " + totalBonus;

        score += totalBonus;
        UpdateScoreText();

        yield return StartCoroutine(Countdown());
        roundEndPanel.SetActive(false);
        isRoundOver = false;

        ammoCount = 30;
        meteorSpeed *= 1f + meteorSpeedMultiplier;
        level++;

        StartRound();
        UpdateLevelText();
        UpdateAmmoCountText();
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        gameOverPanel.SetActive(true);
    }
}
