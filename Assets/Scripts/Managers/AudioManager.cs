using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GlobalAccess<AudioManager>
{
    [Header("Music Needed on This Scene")]
    public List<AudioClip> musicClips;
    private Dictionary<string, AudioClip> musicDict;
    public AudioSource musicAudioSource;

    [Header("Sound Effects Needed on This Scene")]
    public List<AudioClip> soundEffects;
    private Dictionary<string, AudioClip> soundEffectsDict;

    [Header("Sound Effects AudioSources Needed on This Scene")]
    public List<AudioSource> audioSources;
    private Dictionary<string, AudioSource> audioSourcesDict;

    void Start()
    {
        //Dictionarys initialization
        soundEffectsDict = new Dictionary<string, AudioClip>();
        foreach (var clip in soundEffects)
        {
            soundEffectsDict[clip.name] = clip;
        }

        audioSourcesDict = new Dictionary<string, AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSourcesDict[audioSource.name] = audioSource;
        }

        musicDict = new Dictionary<string, AudioClip>();
        foreach (var clip in musicClips)
        {
            musicDict[clip.name] = clip;
        }

        //PlayMusic("SpaceAtmospheric", 1f, false, true);
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
