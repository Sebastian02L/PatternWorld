using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public Dictionary<string, PieceData> pieces;
    float revenue;

    public Order()
    {
        pieces = new Dictionary<string, PieceData>();
        revenue = 0f;
    }

    public void AddPiece(string type, PieceData piece)
    {
        pieces.Add(type, piece);
        revenue += piece.value;
    }
}
