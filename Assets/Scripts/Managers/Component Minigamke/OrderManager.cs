using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    [SerializeField] int numberOfOrders = 20;
    ComponentRoundData minigameData;
    Queue<Order> orders = new Queue<Order>();
    System.Random random = new System.Random();

    [Header("References to other Scripts")]
    [SerializeField] PiecesScreenController piecesScreen;
    [SerializeField] EarningsScreenController earningsScreenController;
    [SerializeField] EndGameController endGameController;
    [SerializeField] CursorVisibility cursorVisibility;
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

        QuotaScreenController quotaScreenController = GameObject.FindAnyObjectByType<QuotaScreenController>();
        quotaScreenController.SetQuota(minigameData.quota);
    }

    //Creates the orders using the ComponentRoundData
    private void RandomizeOrders()
    {
        for (int i = 0; i < numberOfOrders; i++) 
        {
            Order order = new Order();
            order.AddPiece("Head", SelectRandomPiece(minigameData.headPieces));
            order.AddPiece("Body", SelectRandomPiece(minigameData.bodyPieces));
            order.AddPiece("RArm", SelectRandomPiece(minigameData.rightPieces));
            order.AddPiece("LArm", SelectRandomPiece(minigameData.leftPieces));
            order.AddPiece("Wheel", SelectRandomPiece(minigameData.wheelPieces));
            orders.Enqueue(order);
        }

        //Show the first two orders on the left monitor
        screenController.SetNextOrder(Pop());
        screenController.SwapOrders(Pop(), this);
    }

    PieceData SelectRandomPiece(List<PieceData> piecesList)
    {
        return piecesList[random.Next(0, piecesList.Count)];
    }

    public Order Pop() 
    { 
        if (orders.Count == 0) return null;
        return orders.Dequeue();
    }

    //Invoked when an android is finished
    public void OnOrderFinished()
    {
        screenController.SwapOrders(Pop(), this);
    }

    //Invoked when the time is over or when theres no more orders
    public void GameOver()
    {
        //EarningsScreenController earningsScreenController = GameObject.FindAnyObjectByType<EarningsScreenController>();
        //Check win or lose round
        if (earningsScreenController.GetCurrentEarnings >= minigameData.quota)
        {
            List<bool> newMinigameData = new List<bool>();
            for(int i = 0; i < 3; i++)
            {
                if(i < currentRound) newMinigameData.Add(true);
                else newMinigameData.Add(false);
            }
            //Save data
            PlayerDataManager.Instance.SetMinigameRound(0, newMinigameData);
            endGameController.EnablePanel(true);
        }
        else
        {
            endGameController.EnablePanel(false);
        }

        cursorVisibility.ShowCursor();
    }
}
