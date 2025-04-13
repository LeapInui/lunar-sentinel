using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 hotspot;

    [SerializeField] private ShootingContoller shootingContoller;

    private GameController gameController;
    private PauseManager pauseManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        pauseManager = FindFirstObjectByType<PauseManager>();

        hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f); // Set cursor hotspot to middle of texture
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !pauseManager.isPaused && gameController.ammoCount > 0 && !gameController.gameOver)
        {
            Command shootCommand = new ShootCommand(shootingContoller);
            shootCommand.Execute();
            
            gameController.ammoCount--;
            gameController.UpdateAmmoCountText();
        }
    }
}
