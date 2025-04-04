using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EarningsScreenController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI earningsText;
    [SerializeField] TextMeshProUGUI penalizationText;
    float currentEarnings = 0f;
    int penaltyPercent = 0;
    public float GetCurrentEarnings => currentEarnings;
    public float quota = 0f;
    AudioSource audioSourceEarningsScreen;

    private void Start()
    {
        audioSourceEarningsScreen = transform.Find("AS_EarningScreen").GetComponent<AudioSource>();
    }

    //Adds the money when the android is builded
    public void AddMoney(float value)
    {
        currentEarnings += value - (value * penaltyPercent/100f);

        //Fix possible accurancy errors
        if (Mathf.Approximately(currentEarnings, 0f))
        {
            currentEarnings = 0f;
        }

        UpdateEarnignText();
        ResetPenalty();
        CheckWinCondition();
    }

    //Adds % penalty when a piece is removed
    public void AddPenalty(int value)
    {
        if(penaltyPercent >= 100) return;
        penaltyPercent += value;
        AudioManager.Instance.PlaySoundEffect(audioSourceEarningsScreen, "CM_PenaltyAddition", 0.5f);
        UpdatePenaltyText();
        
    }

    void ResetPenalty()
    {
        penaltyPercent = 0;
        UpdatePenaltyText();
    }

    public void UpdateEarnignText()
    {
        earningsText.text = currentEarnings.ToString() + " $";
    }

    public void UpdatePenaltyText()
    {
        float value = (penaltyPercent == 0f)? 0 : (penaltyPercent * -1);
        penalizationText.text = value.ToString() + "%";
    }
    void CheckWinCondition()
    {
        if (currentEarnings >= quota)
        {
            GameObject.FindAnyObjectByType<OrderManager>().GameOver();
        }
    }
}
