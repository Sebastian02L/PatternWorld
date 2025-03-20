using TMPro;
using UnityEngine;

public class EarningsScreenController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI earningsText;
    float currentEarnings = 0f;
    float penaltyPercent = 0f;
    public float GetCurrentEarnings => currentEarnings;

    //Adds the money when the android is builded
    public void AddMoney(float value)
    {
        currentEarnings += (value - (value * penaltyPercent));
        UpdateEarnignText();
        ResetPenalty();
    }

    //Adds % penalty when a piece is removed
    public void AddPenalty(float value)
    {
        if(penaltyPercent <= 1f) penaltyPercent += value;
    }

    void ResetPenalty()
    {
        penaltyPercent = 0f;
    }

    public void UpdateEarnignText()
    {
        earningsText.text = currentEarnings.ToString() + " $";
    }
}
