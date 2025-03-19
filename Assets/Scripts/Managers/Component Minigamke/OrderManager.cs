using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    [SerializeField] ComponentRoundData minigameData;
    [SerializeField] PiecesScreenController piecesScreen;
    [SerializeField] int numberOfOrders = 20;
    Queue<Order> orders = new Queue<Order>();
    System.Random random = new System.Random();

    OrderScreenController screenController;

    int currentRound = 1;
    void Start()
    {
        //Determinates the current round of the minigame
        List<bool> minigameRounds = PlayerDataManager.Instance.GetMinigameRounds()[0];
        foreach (bool succededRound in minigameRounds) if (succededRound) currentRound += 1;
        if(currentRound > 3) currentRound = 3;

        //Loads the round configuration
        minigameData = Resources.Load<ComponentRoundData>(SceneManager.GetActiveScene().name + "/" + currentRound);
        piecesScreen.Setup(minigameData);

        screenController = GetComponent<OrderScreenController>();
        orders = new Queue<Order>(numberOfOrders);
        RandomizeOrders();
    }

    //Creates the orders using the ComponentRoundData
    private void RandomizeOrders()
    {
        for (int i = 0; i < numberOfOrders; i++) 
        {
            Order order = new Order();

            if (minigameData.headPieces.Count > 0) order.AddPiece("Head", SelectRandomPiece(minigameData.headPieces));

            if (minigameData.bodyPieces.Count > 0) order.AddPiece("Body", SelectRandomPiece(minigameData.bodyPieces));

            if (minigameData.rightPieces.Count > 0) order.AddPiece("RArm", SelectRandomPiece(minigameData.rightPieces));

            if (minigameData.leftPieces.Count > 0) order.AddPiece("LArm", SelectRandomPiece(minigameData.leftPieces));

            if (minigameData.wheelPieces.Count > 0) order.AddPiece("Wheel", SelectRandomPiece(minigameData.wheelPieces));

            orders.Enqueue(order);
        }

        Debug.Log("Orders ready!");
        screenController.SetNextOrder(Pop());
        screenController.SwapOrders(Pop());
    }

    PieceData SelectRandomPiece(List<PieceData> piecesList)
    {
        return piecesList[random.Next(0, piecesList.Count)];
    }

    public Order Pop() 
    { 
        return orders.Dequeue();
    }
}
