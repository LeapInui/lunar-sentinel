using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;

    public void StartGame()
    {
        SoundEffectsManager.instance.PlaySfxGlobal(clickSound);
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SoundEffectsManager.instance.PlaySfxGlobal(clickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void Leaderboards()
    {
        SoundEffectsManager.instance.PlaySfxGlobal(clickSound);
        SceneManager.LoadScene("Leaderboards");
    }

    public void Options()
    {
        SoundEffectsManager.instance.PlaySfxGlobal(clickSound);
        SceneManager.LoadScene("Options");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
