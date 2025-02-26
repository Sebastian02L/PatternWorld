using TMPro;
using UnityEngine;

public class GameVersionController : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "v" + Application.version;
    }
}
