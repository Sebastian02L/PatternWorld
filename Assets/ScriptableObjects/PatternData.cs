using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PatternData", menuName = "Scriptable Objects/PatternData")]
public class PatternData : ScriptableObject
{
    public string minigameName;
    public Material lockMaterial;
    public Material unlockedMaterial;

    public Sprite explanation;
    public Sprite usages;
    public Sprite diagram;
}
