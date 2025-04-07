using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicVolumeSlider.value = MusicManager.instance.GetMusicVolume();
        sfxVolumeSlider.value = SoundEffectsManager.instance.GetSfxVolume();

        UpdateVolumeTexts();

        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnMusicVolumeChanged(float value)
    {
        MusicManager.instance.SetMusicVolume(value);
        UpdateVolumeTexts();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundEffectsManager.instance.SetSfxVolume(value);
        UpdateVolumeTexts();
    }

    // Update volume texts for music and sound effects
    private void UpdateVolumeTexts()
    {
        musicVolumeText.text = Mathf.Round(musicVolumeSlider.value * 100).ToString();
        sfxVolumeText.text = Mathf.Round(sfxVolumeSlider.value * 100).ToString();
    }
}
