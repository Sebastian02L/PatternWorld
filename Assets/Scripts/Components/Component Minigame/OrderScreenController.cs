using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderScreenController : MonoBehaviour
{
    [Header("Current Order UI")]
    [SerializeField] TextMeshProUGUI[] currentOrderText;
    [SerializeField] TextMeshProUGUI currentRevenueText;
    Order currentOrder;

    [Header("Next Order UI")]
    [SerializeField] TextMeshProUGUI[] nextOrderText;
    [SerializeField] TextMeshProUGUI nextRevenueText;
    Order nextOrder;

    [Header("References to other Scripts")]
    [SerializeField]NotifierLightsController notifierLights;

    //Displays the current order on the second screen's desk
    void SetActualOrder(Order order)
    {
        currentOrder = order;
        int i = 0;
        foreach(var entry in order.pieces)
        {
            currentOrderText[i].text = entry.Value.maker + " " + entry.Value.modelVersion;
            i++;
        }
        currentRevenueText.text = order.GetRevenue.ToString() + " $";
    }

    //Displays the next order on the second screen's desk
    public void SetNextOrder(Order order)
    {
        nextOrder = order;
        int i = 0;
        foreach (var entry in order.pieces)
        {
            nextOrderText[i].text = entry.Value.maker + " " + entry.Value.modelVersion;
            i++;
        }
        nextRevenueText.text= order.GetRevenue.ToString() + " $";
    }

    //Swap the storaged next order to take the place of the current order and receive the next one
    public void SwapOrders(Order followingOrder, OrderManager orderManager)
    {
        if (followingOrder == null && nextOrder != null) //Case: When there are only 2 orders and the queue is empty
        {
            SetActualOrder(nextOrder);
            nextOrder = null;
            EraseSecondOrderUI();
        }
        else if (nextOrder == null && currentOrder != null)  //Case: When theres only 1 order and the queue is empty
        {
            orderManager.GameOver();
        }
        else //When there are still orders in the queue
        {
            SetActualOrder(nextOrder);
            SetNextOrder(followingOrder);
        }
    }

    //Compares the dictionarys to check if the pieces are correct
    internal float ComparePieces(Dictionary<string, PieceData> buildedPieces)
    {
        float earning = 0f;
        bool allCorrect = true;

        foreach(var piece in buildedPieces.Values)
        {
            if (currentOrder.pieces.ContainsValue(piece)) earning += piece.value;
            else allCorrect = false;
        }

        if (allCorrect)
        {
            notifierLights.OnCorrectOrder();
        }
        else
        {
            notifierLights.OnIncorrectOrder();
        }

        return earning;
    }

    void EraseSecondOrderUI()
    {
        foreach (TextMeshProUGUI text in nextOrderText)
        {
            text.text = string.Empty;
        }
    }
}
