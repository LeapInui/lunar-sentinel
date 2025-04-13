using UnityEngine;

public abstract class Command
{
    public abstract void Execute();
}

public class ShootCommand : Command
{
    private ShootingContoller shootingContoller;

    public ShootCommand(ShootingContoller shootingContoller)
    {
        this.shootingContoller = shootingContoller;
    }

    public override void Execute()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shootingContoller.Fire(mousePos);
    }
}

public class TogglePauseCommand : Command
{
    private PauseManager pauseManager;

    public TogglePauseCommand(PauseManager pauseManager)
    {
        this.pauseManager = pauseManager;
    }

    public override void Execute()
    {
        pauseManager.TogglePanel("pause");
    }
}

public class TogglePowerCommand : Command
{
    private PauseManager pauseManager;

    public TogglePowerCommand(PauseManager pauseManager)
    {
        this.pauseManager = pauseManager;
    }

    public override void Execute()
    {
        pauseManager.TogglePanel("power");
    }
}
