using TMPro;
using UnityEngine;

public class QuotaScreenController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quotaText;

    public void SetQuota(float value)
    {
        quotaText.text = value.ToString() + " $";
    }
}
