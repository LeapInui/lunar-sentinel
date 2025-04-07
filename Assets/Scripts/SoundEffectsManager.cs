using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    private float sfxVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
    }

    public void PlaySfx(AudioSource sound)
    {
        sound.volume = sfxVolume;
        sound.Play();
    }

    // Sets the sound effects volume and saves it in playerprefs
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SfxVolume", volume);
        PlayerPrefs.Save();
    }

    // Getter for sound effects volume value
    public float GetSfxVolume()
    {
        return sfxVolume;
    }
}
