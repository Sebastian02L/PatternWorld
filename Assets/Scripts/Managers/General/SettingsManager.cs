using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour 
{
    [SerializeField] AudioMixerGroup masterAudioMixer;
    [SerializeField] AudioMixerGroup soundEffectsMixer;
    [SerializeField] AudioMixerGroup musicMixer;

    public Slider masterSlider;
    public Slider soundEffectSlider;
    public Slider musicSlider;

    public void Start()
    {
        //Set target frame rate to 60 FPS
        Application.targetFrameRate = 60;
        UpdateSlidersValue();
        gameObject.SetActive(false);
    }

    public void OnGlobalVolumeChange(float value)
    {
        masterAudioMixer.audioMixer.SetFloat("MasterVolume", value);
        PlayerDataManager.Instance.SetGlobalVolume(value);
    }

    public void OnSoundEffectsVolumeChange(float value)
    {
        soundEffectsMixer.audioMixer.SetFloat("SoundEffectsVolume", value);
        PlayerDataManager.Instance.SetSoundEffectsVolume(value);
    }

    public void OnMusicVolumeChange(float value)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", value);
        PlayerDataManager.Instance.SetMusicVolume(value);
    }

    void UpdateSlidersValue()
    {
        masterSlider.value = PlayerDataManager.Instance.GetGlobalVolume;
        masterAudioMixer.audioMixer.SetFloat("MasterVolume", masterSlider.value);

        soundEffectSlider.value = PlayerDataManager.Instance.GetSoundEffectsVolume;
        soundEffectsMixer.audioMixer.SetFloat("SoundEffectsVolume", soundEffectSlider.value);

        musicSlider.value = PlayerDataManager.Instance.GetMusicVolume;
        musicMixer.audioMixer.SetFloat("MusicVolume", musicSlider.value);
    }
}
