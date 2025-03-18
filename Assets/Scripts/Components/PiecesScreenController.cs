using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecesScreenController : MonoBehaviour
{
    [SerializeField] List<Button> piecesButtons;
    ComponentRoundData minigameData;
    int numberOfPieces = 0;

    public void Setup(ComponentRoundData data)
    {
        minigameData = data;
        numberOfPieces = minigameData.headPieces.Count;
        int counter = 0;

        foreach (Button button in piecesButtons) 
        {
            if (counter++ >= numberOfPieces) 
            { 
                button.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateButtonsImage(string pieceType)
    {
        switch (pieceType) 
        {
            case "Head":
                ChangeImage(minigameData.headPieces);
                break;

            case "Body":
                ChangeImage(minigameData.bodyPieces);
                break;

            case "LArm":
                ChangeImage(minigameData.leftPieces);
                break;

            case "RArm":
                ChangeImage(minigameData.rightPieces);
                break;

            case "Wheel":
                ChangeImage(minigameData.wheelPieces);
                break;
        }
    }

    void ChangeImage(List<PieceData> pieces)
    {
        for (int i = 0; i < numberOfPieces; i++)
        {
            piecesButtons[i].GetComponent<Image>().sprite = pieces[i].render;
        }
    }
}
