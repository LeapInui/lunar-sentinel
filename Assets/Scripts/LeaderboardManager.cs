using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

[System.Serializable]
public class Leaderboard
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    public Leaderboard(List<LeaderboardEntry> entries)
    {
        this.entries = entries;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    // UI elements
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TextMeshProUGUI[] leaderboardTextFields;

    private const string LeaderboardKey = "Leaderboard";
    
    [SerializeField] private AudioSource highscoreSound;
    [SerializeField] private AudioSource clickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<LeaderboardEntry> leaderboard = LoadLeaderboard();
        
        DisplayLeaderboard();
        StartCoroutine(FlashNewHighscore());
    }

    // Deletes temporary keys so new highscore effect doesn't persist
    void OnDestroy()
    {
        // Clear the "new entry" data when the scene is unloaded
        PlayerPrefs.DeleteKey("NewEntryName");
        PlayerPrefs.DeleteKey("NewEntryScore"); 
    }

    // Displays the leaderboard onto the actual UI
    public void DisplayLeaderboard()
    {
        List<LeaderboardEntry> leaderboard = LoadLeaderboard();
        
        // Loop through text fields and populates the leaderboard
        for (int i = 0; i < leaderboardTextFields.Length; i++)
        {
            if (i < leaderboard.Count)
            {
                leaderboardTextFields[i].text = leaderboard[i].playerName;
                TextMeshProUGUI scoreText = leaderboardTextFields[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                scoreText.text = leaderboard[i].score.ToString();
            }
            else
            {
                // Clear remaining fields if fewer than 5 entries
                leaderboardTextFields[i].text = "";
                TextMeshProUGUI scoreText = leaderboardTextFields[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                scoreText.text = "";
            }
        }
    }


    // New highscore effect
    private IEnumerator FlashNewHighscore()
    {
        if (!PlayerPrefs.HasKey("NewEntryName") || !PlayerPrefs.HasKey("NewEntryScore")) yield break;

        string newName = PlayerPrefs.GetString("NewEntryName");
        int newScore = PlayerPrefs.GetInt("NewEntryScore");

        List<LeaderboardEntry> leaderboard = LoadLeaderboard();

        SoundEffectsManager.instance.PlaySfxGlobal(highscoreSound);

        for (int i = 0; i < leaderboard.Count; i++)
        {
            if (leaderboard[i].playerName == newName && leaderboard[i].score == newScore)
            {
                TextMeshProUGUI nameField = leaderboardTextFields[i];
                TextMeshProUGUI scoreField = leaderboardTextFields[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                while (true)
                {
                    nameField.color = Color.yellow;
                    scoreField.color = Color.yellow;
                    yield return new WaitForSeconds(0.5f);

                    nameField.color = Color.white;
                    scoreField.color = Color.white;
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }

    // Function that returns a boolean if the score is on the leaderboards
    public static bool isHighscore(int score)
    {
        List<LeaderboardEntry> leaderboard = LoadLeaderboard();

        // If there are fewer than 5 entries, then it will be on leaderboards
        if (leaderboard.Count < 5)
        {
            return true;
        }

        // Loop through and check if current score is higher than any existing scores
        for (int i = 0; i < leaderboard.Count; i++)
        {
            if (score > leaderboard[i].score)
            {
                return true;
            }
        }
        return false;
    }

    // Loads leaderboard from PlayerPrefs
    public static List<LeaderboardEntry> LoadLeaderboard()
    {
        if (!PlayerPrefs.HasKey(LeaderboardKey)) return new List<LeaderboardEntry>();

        string json = PlayerPrefs.GetString(LeaderboardKey);
        return JsonUtility.FromJson<Leaderboard>(json).entries;
    }

    // Add score to leaderboard
    public static void SaveScore(string playerName, int score)
    {
        List<LeaderboardEntry> leaderboard = LoadLeaderboard();
        leaderboard.Add(new LeaderboardEntry(playerName, score));

        // Sort by highest score and keep only top 5
        leaderboard = leaderboard.OrderByDescending(entry => entry.score).Take(5).ToList();

        string json = JsonUtility.ToJson(new Leaderboard(leaderboard));
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    // Displays confirm panel to clear scores
    public void ConfirmPanel()
    {
        SoundEffectsManager.instance.PlaySfx(clickSound);
        confirmPanel.SetActive(true);
    }

    public void ConfirmBack()
    {
        SoundEffectsManager.instance.PlaySfx(clickSound);
        confirmPanel.SetActive(false);
    }

    public void ConfirmClear()
    {
        SoundEffectsManager.instance.PlaySfx(clickSound);
        ClearLeaderboard();
        confirmPanel.SetActive(false);
    }

    // Clears leaderboard from PlayerPrefs
    private void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey(LeaderboardKey);
        PlayerPrefs.Save();
        DisplayLeaderboard();
    }
}
