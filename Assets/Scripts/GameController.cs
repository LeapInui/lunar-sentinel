using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject roundEndPanel;
    MeteorSpawner meteorSpawner;

    public int score = 0;
    public int level = 1;
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

    private bool isRoundOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meteorSpawner = GameObject.FindFirstObjectByType<MeteorSpawner>();

        UpdateScoreText();
        UpdateLevelText();
        UpdateAmmoCountText();

        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (meteorsLeftCount <= 0 && !isRoundOver)
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
        MeteorDestroyed();
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
        yield return new WaitForSeconds(.5f);
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

        StartRound();
        UpdateLevelText();
        UpdateAmmoCountText();
    }
}
