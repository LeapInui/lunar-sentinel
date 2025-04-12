using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

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
