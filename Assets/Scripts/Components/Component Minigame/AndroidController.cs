using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    //Android Components
    IRenderer headRenderer;
    IRenderer bodyRenderer;
    IRenderer rightRenderer;
    IRenderer leftRenderer;
    IRenderer wheelRenderer;

    void Start()
    {
        IRenderer[] components = gameObject.GetComponentsInChildren<IRenderer>();

        headRenderer = components[0];
        bodyRenderer = components[1];
        rightRenderer = components[2];
        leftRenderer = components[3];
        wheelRenderer = components[4];
    }

    void HeadPiece(PieceData newPiece = null)
    {
        headRenderer.ApplyTexture(newPiece?.texture);
    }
    void BodyPiece(PieceData newPiece = null)
    {
        bodyRenderer.ApplyTexture(newPiece?.texture);
    }
    void RightPiece(PieceData newPiece = null) 
    { 
        rightRenderer.ApplyTexture(newPiece?.texture);
    }
    void LeftPiece(PieceData newPiece = null)
    {
        leftRenderer.ApplyTexture(newPiece?.texture);
    }
    void WheelPiece(PieceData newPiece = null)
    {
        wheelRenderer.ApplyTexture(newPiece?.texture);
    }

    //Invokes the correct method for the pieces. If piece is null hat means that the piece was removed
    public void UpdateAndroidVisuals(string pieceType, PieceData piece = null)
    {
        switch (pieceType)
        {
            case "Head":
                HeadPiece(piece);
                break;

            case "Body":
                BodyPiece(piece);
                break;

            case "LArm":
                LeftPiece(piece);
                break;

            case "RArm":
                RightPiece(piece);
                break;

            case "Wheel":
                WheelPiece(piece);
                break;
        }
    }

    public void ResetAndroid()
    {
        HeadPiece();
        BodyPiece();
        RightPiece();
        LeftPiece();
        WheelPiece();
    }
}
