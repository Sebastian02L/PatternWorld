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
    //private Dictionary<string, AudioSource> audioSourcesDict;
    //AudioSource musicAudioSource;

    void Start()
    {
        soundEffectsDict = new Dictionary<string, AudioClip>();
        //audioSourcesDict = new Dictionary<string, AudioSource>();
        musicDict = new Dictionary<string, AudioClip>();

        SceneManager.sceneLoaded += SetUp;
        SetUp(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    //Method that set up the AudioManager to work on the current Scene.
    void SetUp(Scene arg0, LoadSceneMode arg1)
    {
        soundEffectsDict.Clear();
        //audioSourcesDict.Clear();
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

        //AudioSource[] sceneAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        //// Mostrar en la consola los nombres de los objetos encontrados
        //foreach (AudioSource audioSource in sceneAudioSources)
        //{
        //    audioSourcesDict[audioSource.name] = audioSource;
        //}

        //musicAudioSource = audioSourcesDict["AS_Music"];
    }

    //The sound effects played by this method cant be replayed or stopped.
    public void PlayOneShotSoundEffect(AudioSource audioSource, string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (soundEffectsDict.TryGetValue(clipName, out AudioClip clip))
        {
            if (audioSource.isPlaying && waitFinish) return;

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
    public void PlaySoundEffect(AudioSource audioSource, string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (soundEffectsDict.TryGetValue(clipName, out AudioClip clip))
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
    public void PlayOneShotMusic(AudioSource audioSource, string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (musicDict.TryGetValue(clipName, out AudioClip clip))
        {
            if (audioSource.isPlaying && waitFinish) return;

            audioSource.volume = volume;
            audioSource.loop = needToLoop;
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("No Clip or AudioSource with that name was found.");
        }
    }

    //The music played by this method can be replayed or stopped.
    public void PlayMusic(AudioSource audioSource, string clipName, float volume, bool waitFinish = false, bool needToLoop = false)
    {
        if (musicDict.TryGetValue(clipName, out AudioClip clip))
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

    public AudioClip GetAudioClip(string clipName)
    {
        foreach(AudioClip clip in soundEffectsDict.Values)
        {
            if(clip.name.Equals(clipName)) return clip;
        }

        foreach (AudioClip clip in musicDict.Values)
        {
            if (clip.name.Equals(clipName)) return clip;
        }

        return null;
    }
    //Stop the sound effect or the music played by the given audiosource.
    public void StopAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
