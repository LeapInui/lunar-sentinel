using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private GameController gameController;
    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameController.gameOver)
        {
            ToggleMenu();
        }
    }

    // Ensures time scale is reset when this object is destroyed
    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    // Show or hide the pause menu when esc is clicked
    private void ToggleMenu()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
