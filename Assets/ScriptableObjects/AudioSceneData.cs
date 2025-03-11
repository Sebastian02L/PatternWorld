using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSceneData", menuName = "Scriptable Objects/AudioSceneData")]
public class AudioSceneData : ScriptableObject
{
    [Header("Music Needed on This Scene")]
    public List<AudioClip> musicClips;

    [Header("Sound Effects Needed on This Scene")]
    public List<AudioClip> soundEffects;
}
