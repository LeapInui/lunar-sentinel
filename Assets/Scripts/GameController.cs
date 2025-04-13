using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Initialise panels
    [SerializeField] private GameObject roundEndPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject highscorePanel;

    // Round end panel contents
    [SerializeField] private TextMeshProUGUI ammoRemainingBonusText;
    [SerializeField] private TextMeshProUGUI buildingsRemainingText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI countdownText;

    // Game over panel contents
    [SerializeField] private TextMeshProUGUI endScoreText;

    // New highscore panel contents
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI endScoreText2;
    [SerializeField] private TMP_InputField usernameInput;

    // Game UI
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ammoCountText;

    // Score Values
    private int meteorDestroyPoints = 50;
    [SerializeField] private int ammoRemainingBonus = 5;
    [SerializeField] private int bulidingsRemainingBonus = 100;

    MeteorSpawner meteorSpawner;

    // Game state variables
    public int score = 0;
    public int level = 1;
    public float meteorSpeed = 2f;
    [SerializeField] private float meteorSpeedMultiplier = 0.05f;

    public int robotCounter;
    private int initialAmmoCount = 20;
    public int ammoCount;
    private int totalMeteorCount = 10;
    private int meteorsLeftCount = 0;

    public bool isRoundOver = false;
    public bool gameOver = false;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meteorSpawner = ServiceLocator.Get<MeteorSpawner>();
        robotCounter = FindObjectsByType<RobotController>(FindObjectsSortMode.None).Length;
        ammoCount = initialAmmoCount;

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

    // Update functions for UI, separate so it doesn't update every frame - saves performance
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

    // Handles round start
    private void StartRound()
    {
        meteorSpawner.meteorCount = totalMeteorCount;
        meteorsLeftCount = totalMeteorCount;
        meteorSpawner.StartCoroutine(meteorSpawner.SpawnMeteor());
    }

    // Countdown effect for round over panel
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

    // Flash effect for highscore panel
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

    // Handles round end
    public IEnumerator EndRound()
    {
        int ammoRemainingScore = ammoCount * ammoRemainingBonus;

        // Count remaining buildings to calculate bonus
        Building[] buildings = FindObjectsByType<Building>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        int bulidingsRemainingScore = buildings.Length * bulidingsRemainingBonus;

        int totalBonus = ammoRemainingScore + bulidingsRemainingScore;

        // Updates UI
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

        // Increasing difficulty of each level
        ammoCount = initialAmmoCount + (3 * level);
        meteorSpeed *= 1f + meteorSpeedMultiplier;
        totalMeteorCount += 2;
        level++;

        StartRound();
        UpdateLevelText();
        UpdateAmmoCountText();
    }

    // Handles game over
    public IEnumerator GameOver()
    {
        int ammoRemainingScore = ammoCount * ammoRemainingBonus;

        Building[] buildings = FindObjectsByType<Building>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        int bulidingsRemainingScore = buildings.Length * bulidingsRemainingBonus;

        int totalBonus = ammoRemainingScore + bulidingsRemainingScore;

        yield return new WaitForSeconds(0.5f);

        int scoreBeforeBonus = score;
        score += totalBonus;

        // Check if the score is a highscore - to allow users to submit a username
        // If not show game over panel
        if (LeaderboardManager.isHighscore(score))
        {
            highscorePanel.SetActive(true);
            StartCoroutine(FlashHighscore());
            
            endScoreText2.text = "Total Score: " + scoreBeforeBonus + " + " + totalBonus;
            UpdateScoreText();
        }
        else
        {
            gameOverPanel.SetActive(true);

            UpdateScoreText();
            endScoreText.text = "Total Score: " + score;
        }
    }

    // Submits score to leaderboard
    public void SubmitButton()
    {
        LeaderboardManager.SaveScore(usernameInput.text, score);

        // Temporarily store new entry for new highscore effect
        PlayerPrefs.SetString("NewEntryName", usernameInput.text);
        PlayerPrefs.SetInt("NewEntryScore", score);

        SceneManager.LoadScene("Leaderboards");
    }
}
