using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject powerPanel;

    private GameController gameController;
    public bool isPaused = false;
    private GameObject activePanel = null;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    void OnDestroy()
    {
        ServiceLocator.Unregister<PauseManager>();
        Time.timeScale = 1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = ServiceLocator.Get<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isRoundOver || gameController.gameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePanel == null || activePanel == pausePanel)
            {
                Command pauseCommand = new TogglePauseCommand(this);
                pauseCommand.Execute();
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (activePanel == null || activePanel == powerPanel)
            {
                Command powerCommand = new TogglePowerCommand(this);
                powerCommand.Execute();
            }
        }
    }

    // Show or hide respective panel
    public void TogglePanel(string panelType)
    {
        GameObject panel;
        if (panelType == "pause")
        {
            panel = pausePanel;
        }
        else
        {
            panel = powerPanel;
        }

        // Close if clicking the currently open panel
        if (activePanel == panel)
        {
            isPaused = false;
            panel.SetActive(false);
            activePanel = null;
            Time.timeScale = 1f;
            return;
        }

        // Open new panel
        isPaused = true;
        panel.SetActive(true);
        activePanel = panel;
        Time.timeScale = 0f;
    }
}
