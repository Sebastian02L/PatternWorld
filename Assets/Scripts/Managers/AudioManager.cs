using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    AudioSceneData currentSceneAudioData;
    private Dictionary<string, AudioClip> musicDict;
    private Dictionary<string, AudioClip> soundEffectsDict;
    private Dictionary<string, AudioSource> audioSourcesDict;
    AudioSource musicAudioSource;

    void Start()
    {
        soundEffectsDict = new Dictionary<string, AudioClip>();
        audioSourcesDict = new Dictionary<string, AudioSource>();
        musicDict = new Dictionary<string, AudioClip>();

        SceneManager.sceneLoaded += SetUp;
        SetUp(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    //Method that set up the AudioManager to work on the current Scene.
    void SetUp(Scene arg0, LoadSceneMode arg1)
    {
        soundEffectsDict.Clear();
        audioSourcesDict.Clear();
        musicDict.Clear();

        currentSceneAudioData = Resources.Load<AudioSceneData>("Audio/" + SceneManager.GetActiveScene().name);

        //Dictionarys initialization
        foreach (var clip in currentSceneAudioData.soundEffects)
        {
            soundEffectsDict[clip.name] = clip;
        }

        foreach (var clip in currentSceneAudioData.musicClips)
        {
            musicDict[clip.name] = clip;
        }

        AudioSource[] sceneAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        // Mostrar en la consola los nombres de los objetos encontrados
        foreach (AudioSource audioSource in sceneAudioSources)
        {
            audioSourcesDict[audioSource.name] = audioSource;
        }

        musicAudioSource = audioSourcesDict["AS_Music"];
    }

    //The sound effects played by this method cant be replayed or stopped.
    public void PlayOneShotSoundEffect(string audioSourceName, string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (soundEffectsDict.TryGetValue(clipName, out AudioClip clip) && audioSourcesDict.TryGetValue(audioSourceName, out AudioSource audioSource))
        {
            if(audioSource.isPlaying && waitFinish) return;

            audioSource.volume = volume;
            audioSource.loop = needToLoop;
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("No Clip or AudioSource with that name was found.");
        }
    }

    //The sound effects played by this method can be replayed or stopped.
    public void PlaySoundEffect(string audioSourceName, string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (soundEffectsDict.TryGetValue(clipName, out AudioClip clip) && audioSourcesDict.TryGetValue(audioSourceName, out AudioSource audioSource))
        {
            if (audioSource.isPlaying && waitFinish) return;

            audioSource.volume = volume;
            audioSource.clip = clip;
            audioSource.loop = needToLoop;
            audioSource.Play();
        }
        else
        {
            Debug.Log("No Clip or AudioSource with that name was found.");
        }
    }

    //The music played by this method cant be replayed or stopped.
    public void PlayOneShotMusic(string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (musicDict.TryGetValue(clipName, out AudioClip clip) && musicAudioSource != null)
        {
            if (musicAudioSource.isPlaying && waitFinish) return;

            musicAudioSource.volume = volume;
            musicAudioSource.loop = needToLoop;
            musicAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("No Clip or AudioSource with that name was found.");
        }
    }

    //The music played by this method can be replayed or stopped.
    public void PlayMusic(string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (musicDict.TryGetValue(clipName, out AudioClip clip) && musicAudioSource != null)
        {
            if (musicAudioSource.isPlaying && waitFinish) return;

            musicAudioSource.volume = volume;
            musicAudioSource.clip = clip;
            musicAudioSource.loop = needToLoop;
            musicAudioSource.Play();
        }
        else
        {
            Debug.Log("No Clip or AudioSource with that name was found.");
        }
    } 

    //Stop the sound effect or the music played by the given audiosource.
    public void StopAudioSource(string audioSourceName)
    {
        if (audioSourcesDict.TryGetValue(audioSourceName, out AudioSource audioSource))
        {
            audioSource.Stop();
        }
        else
        {
            Debug.Log("No AudioSource with that name was found.");
        }
    }
}
