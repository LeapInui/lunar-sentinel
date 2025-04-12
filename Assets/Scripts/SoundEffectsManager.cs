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

    // Play sound effect local to scene
    public void PlaySfx(AudioSource source)
    {
        source.volume = sfxVolume;
        source.Play();
    }

    // Play sound effect globally 
    public void PlaySfxGlobal(AudioSource source)
    {
        GameObject tempAudio = new GameObject("TempSFX");
        AudioSource tempSource = tempAudio.AddComponent<AudioSource>();

        tempSource.clip = source.clip;
        tempSource.volume = sfxVolume;
        tempSource.pitch = source.pitch;
        tempSource.spatialBlend = source.spatialBlend;
        tempSource.loop = false;

        DontDestroyOnLoad(tempAudio);

        tempSource.Play();

        Destroy(tempAudio, tempSource.clip.length);
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
