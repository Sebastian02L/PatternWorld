using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiecesScreenController : MonoBehaviour
{
    //UI Elements
    [SerializeField] List<PieceButtonController> piecesButtons;
    [SerializeField] TextMeshProUGUI pieceMakerText;
    [SerializeField] TextMeshProUGUI pieceModelVersionText;
    [SerializeField] Button buildButton;
    [SerializeField] Button removeButton;

    ComponentRoundData minigameData;
    int numberOfPieces = 0;

    //Variables
    PieceData selectedPiece;
    string currentPieceType;

    //Android reference and build in pieces
    Dictionary<string, PieceData> buildedPieces;
    AndroidController android;

    //Setups the left monitor with the round data received from the right monitor
    public void Setup(ComponentRoundData data)
    {
        buildedPieces = new Dictionary<string, PieceData>();
        android = GameObject.FindFirstObjectByType<AndroidController>();

        minigameData = data;
        numberOfPieces = minigameData.headPieces.Count;
        int counter = 0;

        //Turn off the innecesary UI buttons
        foreach (PieceButtonController button in piecesButtons) 
        {
            if (counter++ >= numberOfPieces) 
            { 
                button.gameObject.SetActive(false);
            }
        }

        buildButton.onClick.AddListener(BuildPiece);
        removeButton.onClick.AddListener(RemoveLastPiece);
    }

    //Clear the information text when the panel is Off
    private void OnDisable()
    {
        buildButton.interactable = false;
        removeButton.interactable = false;
        pieceMakerText.text = string.Empty;
        pieceModelVersionText.text = string.Empty;
    }

    private void OnDestroy()
    {
        buildButton.onClick.RemoveListener(BuildPiece);
        removeButton.onClick.RemoveListener(RemoveLastPiece);
    }

    //Invoked from the UI Buttons
    public void UpdateButtonsImage(string pieceType)
    {
        switch (pieceType) 
        {
            case "Head":
                SetPiecesList(minigameData.headPieces);
                break;

            case "Body":
                SetPiecesList(minigameData.bodyPieces);
                break;

            case "LArm":
                SetPiecesList(minigameData.leftPieces);
                break;

            case "RArm":
                SetPiecesList(minigameData.rightPieces);
                break;

            case "Wheel":
                SetPiecesList(minigameData.wheelPieces);
                break;
        }
    }

    //Update the UI buttons with the correct pieces according with the selected type
    void SetPiecesList(List<PieceData> pieces)
    {
        currentPieceType = pieces[0].type;
        CheckCanRemove();

        for (int i = 0; i < numberOfPieces; i++)
        {
            piecesButtons[i].SetUpButton(pieces[i]);
        }
    }

    //Invoked when the player click's the pieces button
    public void ShowPieceInformation(PieceData piece)
    {
        selectedPiece = piece;
        pieceMakerText.text = piece.maker;
        pieceModelVersionText.text = piece.modelVersion;
        CheckCanBuild();
    }

    void BuildPiece()
    {
        android.UpdateAndroidVisuals(currentPieceType, selectedPiece);
        buildedPieces.Add(selectedPiece.type, selectedPiece);
        CheckCanBuild();
    }
    void RemoveLastPiece()
    {
        android.UpdateAndroidVisuals(currentPieceType);
        buildedPieces.Remove(currentPieceType);
        CheckCanRemove();
    }

    void CheckCanBuild()
    {
        if(!buildedPieces.ContainsKey(currentPieceType)) buildButton.interactable = true;
        else buildButton.interactable = false;
    }
    void CheckCanRemove()
    {
        if (buildedPieces.ContainsKey(currentPieceType)) removeButton.interactable = true;
        else removeButton.interactable = false;
    }
}
