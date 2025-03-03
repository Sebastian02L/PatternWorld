using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] AudioMixerGroup masterAudioMixer;
    [SerializeField] AudioMixerGroup soundEffectsMixer;
    [SerializeField] AudioMixerGroup musicMixer;

    public Slider masterSlider;
    public Slider soundEffectSlider;
    public Slider musicSlider;

    public void Start()
    {
        PlayerDataManager.Instance.LoadSaveSettings();
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
}
