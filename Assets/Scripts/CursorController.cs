using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 hotspot;

    [SerializeField] private ShootingContoller shootingContoller;

    private GameController gameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();

        hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && gameController.ammoCount > 0 && !gameController.gameOver)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shootingContoller.Fire(mousePos);
            gameController.ammoCount--;
            gameController.UpdateAmmoCountText();
        }
    }
}
