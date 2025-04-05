using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject roundEndPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject highscorePanel;

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

    // Game UI
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ammoCountText;

    // Round end panel
    [SerializeField] private TextMeshProUGUI ammoRemainingBonusText;
    [SerializeField] private TextMeshProUGUI buildingsRemainingText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI countdownText;

    // Game over panel
    [SerializeField] private TextMeshProUGUI endScoreText;

    // New highscore panel
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI endScoreText2;
    [SerializeField] private TMP_InputField usernameInput;

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
        if (robotCounter <= 0 && !gameOver) {
            gameOver = true;
            StartCoroutine(GameOver());
        }

        if (meteorsLeftCount <= 0 && !isRoundOver && !gameOver)
        {
            isRoundOver = true;
            StartCoroutine(EndRound());
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

    private IEnumerator FlashHighscore()
    {
        while (true)
        {
            highscoreText.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            highscoreText.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator EndRound()
    {
        int ammoRemainingScore = ammoCount * ammoRemainingBonus;

        Building[] buildings = FindObjectsByType<Building>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        int bulidingsRemainingScore = buildings.Length * bulidingsRemainingBonus;

        int totalBonus = ammoRemainingScore + bulidingsRemainingScore;

        ammoRemainingBonusText.text = "Ammunition Remaining Bonus: " + ammoRemainingScore;
        buildingsRemainingText.text = "Buildings Remaining Bonus: " + bulidingsRemainingScore;
        totalScoreText.text = "Total Score: " + score + " + " + totalBonus;

        score += totalBonus;
        UpdateScoreText();

        yield return new WaitForSeconds(0.5f);
        roundEndPanel.SetActive(true);

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
        int ammoRemainingScore = ammoCount * ammoRemainingBonus;

        Building[] buildings = FindObjectsByType<Building>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        int bulidingsRemainingScore = buildings.Length * bulidingsRemainingBonus;

        int totalBonus = ammoRemainingScore + bulidingsRemainingScore;

        score += totalBonus;
        UpdateScoreText();

        yield return new WaitForSeconds(0.5f);

        if (LeaderboardManager.isHighscore(score))
        {
            highscorePanel.SetActive(true);
            StartCoroutine(FlashHighscore());
            
            endScoreText2.text = "Total Score: " + score + " + " + totalBonus;
        }
        else
        {
            gameOverPanel.SetActive(true);

            endScoreText.text = "Total Score: " + score + " + " + totalBonus;
        }
    }

    public void SubmitButton()
    {
        LeaderboardManager.SaveScore(usernameInput.text, score);

        // Temporarily store new entry for new highscore effect
        PlayerPrefs.SetString("NewEntryName", usernameInput.text);
        PlayerPrefs.SetInt("NewEntryScore", score);

        SceneManager.LoadScene("Leaderboards");
    }
}
