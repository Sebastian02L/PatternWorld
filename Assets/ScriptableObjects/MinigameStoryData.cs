using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameStoryData", menuName = "Scriptable Objects/MinigameStoryData")]
public class MinigameStoryData : ScriptableObject
{
    [Header("Story Elements")]
    public List<Texture2D> strips;
    public List<TextAsset> stripsText;
}
