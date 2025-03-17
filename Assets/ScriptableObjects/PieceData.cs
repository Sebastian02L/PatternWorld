using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Objects/PieceData")]
public class PieceData : ScriptableObject
{
    public string maker;
    public string modelVersion;
    public Texture texture;
    public float value;
    public Sprite render;
}
