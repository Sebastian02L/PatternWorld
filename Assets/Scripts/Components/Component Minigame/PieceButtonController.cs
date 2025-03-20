using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceButtonController : MonoBehaviour
{
    PiecesScreenController piecesScreenController;
    PieceData currentPiece;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnPieceSelected);
        piecesScreenController = gameObject.GetComponentInParent<PiecesScreenController>();    
    }

    private void OnDestroy()
    {
        gameObject.GetComponent<Button>().onClick.RemoveListener(OnPieceSelected);
    }

    //Receive the piece that the button will represent
    public void SetUpButton(PieceData piece)
    {
        currentPiece = piece;
        UpdateImage();
    }

    void UpdateImage()
    {
        GetComponent<Image>().sprite = currentPiece.render;
    }

    //Invoked when the player clicks the button
    public void OnPieceSelected()
    {
        piecesScreenController.ShowPieceInformation(currentPiece);
    }
}
