using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentRoundData", menuName = "Scriptable Objects/ComponentRoundData")]
public class ComponentRoundData : ScriptableObject
{
    public List<PieceData> headPieces;
    public List<PieceData> bodyPieces;
    public List<PieceData> leftPieces;
    public List<PieceData> rightPieces;
    public List<PieceData> wheelPieces;
}
