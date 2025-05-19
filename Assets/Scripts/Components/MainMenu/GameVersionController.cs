using TMPro;
using UnityEngine;

public class GameVersionController : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "Alpha v" + Application.version;
    }
}
