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
    }

    //Swap the storaged next order to take the place of the current order and receive the next one
    public void SwapOrders(Order followingOrder)
    {
        SetActualOrder(nextOrder);
        SetNextOrder(followingOrder);
    }
}
