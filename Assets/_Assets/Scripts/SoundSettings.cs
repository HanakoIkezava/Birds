using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider volumeSlider;

    private void Awake()
    {
        InitializeVolume();
    }

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
    }

    private void InitializeVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 100f);
        volumeSlider.value = savedVolume;
        ApplyVolume(savedVolume / 100f);
    }

    private void OnVolumeChange()
    {
        float sliderValue = volumeSlider.value;
        float newVolume = sliderValue / 100f;
        ApplyVolume(newVolume);

        PlayerPrefs.SetFloat("Volume", sliderValue);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
