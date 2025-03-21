using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiecesScreenController : MonoBehaviour
{
    [Header("Piece Select Panel Elements")]
    [SerializeField] List<PieceButtonController> piecesButtons;
    [SerializeField] TextMeshProUGUI pieceMakerText;
    [SerializeField] TextMeshProUGUI pieceModelVersionText;
    [SerializeField] TextMeshProUGUI pieceTitle;
    [SerializeField] Button buildPieceButton;
    [SerializeField] Button removePieceButton;

    [Header("Builder Panel Elements")]
    [SerializeField] Button buildAndroidButton;

    ComponentRoundData minigameData;
    int numberOfPieces = 0;

    //Auxiliar Variables
    PieceData selectedPiece;
    string currentPieceType;

    //Android reference and build in pieces
    Dictionary<string, PieceData> buildedPieces;

    [Header("References to other Scripts")]
    [SerializeField] AndroidController android;
    [SerializeField] OrderScreenController orderScreenController;
    [SerializeField] EarningsScreenController earningsScreenController;
    [SerializeField] OrderManager orderManager;

    //Setups the left monitor with the round data received from the right monitor
    public void Setup(ComponentRoundData data)
    {
        buildedPieces = new Dictionary<string, PieceData>();
        //android = GameObject.FindAnyObjectByType<AndroidController>();
        //earningsScreenController = GameObject.FindAnyObjectByType<EarningsScreenController>();
        //orderScreenController = GameObject.FindAnyObjectByType<OrderScreenController>();
        //orderManager = GameObject.FindAnyObjectByType<OrderManager>();

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

        buildPieceButton.onClick.AddListener(BuildPiece);
        removePieceButton.onClick.AddListener(RemoveLastPiece);
        buildAndroidButton.onClick.AddListener(BuildAndroid);
    }

    //Clear the information text when the panel is Off
    private void OnDisable()
    {
        buildPieceButton.interactable = false;
        removePieceButton.interactable = false;
        pieceMakerText.text = string.Empty;
        pieceModelVersionText.text = string.Empty;
    }

    private void OnDestroy()
    {
        buildPieceButton.onClick.RemoveListener(BuildPiece);
        removePieceButton.onClick.RemoveListener(RemoveLastPiece);
        buildAndroidButton.onClick.RemoveListener(BuildAndroid);
    }

    //Invoked from the UI Buttons
    public void UpdateButtonsImage(string pieceType)
    {
        switch (pieceType) 
        {
            case "Head":
                pieceTitle.text = "Cabeza";
                SetPiecesList(minigameData.headPieces);
                break;

            case "Body":
                pieceTitle.text = "Cuerpo";
                SetPiecesList(minigameData.bodyPieces);
                break;

            case "LArm":
                pieceTitle.text = "Brazo Izquierdo";
                SetPiecesList(minigameData.leftPieces);
                break;

            case "RArm":
                pieceTitle.text = "Brazo Derecho";
                SetPiecesList(minigameData.rightPieces);
                break;

            case "Wheel":
                pieceTitle.text = "Rueda";
                SetPiecesList(minigameData.wheelPieces);
                break;
        }
    }

    //Update the UI buttons with the correct pieces according with the selected type
    void SetPiecesList(List<PieceData> pieces)
    {
        AudioManager.Instance.PlaySoundEffect("AS_BuilderArm", "CM_SelectedPiece", 0.5f);
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
        AudioManager.Instance.PlaySoundEffect("AS_BuilderArm", "CM_PickUpPiece", 0.5f);
        selectedPiece = piece;
        pieceMakerText.text = piece.maker;
        pieceModelVersionText.text = piece.modelVersion;
        CheckCanBuild();
    }

    void BuildPiece()
    {
        AudioManager.Instance.PlaySoundEffect("AS_BuilderArm", "CM_BuildPiece", 1f);
        android.UpdateAndroidVisuals(currentPieceType, selectedPiece);
        buildedPieces.Add(selectedPiece.type, selectedPiece);
        CheckCanBuild();
        CheckCanRemove();
        CheckCanBuildAndroid();
    }
    void RemoveLastPiece()
    {
        AudioManager.Instance.PlaySoundEffect("AS_BuilderArm", "CM_RemovePiece", 1f);
        android.UpdateAndroidVisuals(currentPieceType);
        buildedPieces.Remove(currentPieceType);
        CheckCanRemove();
        CheckCanBuild();
        CheckCanBuildAndroid();
        //Decirle al earnignController que aumente el porcentaje de castigo
        earningsScreenController.AddPenalty(0.1f);
    }

    void BuildAndroid()
    {
        earningsScreenController.AddMoney(orderScreenController.ComparePieces(buildedPieces));
        android.ResetAndroid();
        orderManager.OnOrderFinished();
        buildedPieces.Clear();
        CheckCanBuildAndroid();
        selectedPiece = null;
    }

    void CheckCanBuild()
    {
        buildPieceButton.interactable = (!buildedPieces.ContainsKey(currentPieceType))? true : false;
    }
    void CheckCanRemove()
    {
        removePieceButton.interactable = (buildedPieces.ContainsKey(currentPieceType))? true : false;
    }

    void CheckCanBuildAndroid()
    {
        buildAndroidButton.interactable = (buildedPieces.Count == 5) ? true : false;
    }
}
