using UnityEngine;

public class ExitGameController : MonoBehaviour
{
    public void ExitGame()
    {
        PlayerDataManager.Instance.SaveData();
        Application.Quit();
    }
}
