using System.Collections.Generic;
using UnityEngine;

public class AlphaPanelEnabler : MonoBehaviour
{
    void Start()
    {
        List<List<bool>> gameRounds = PlayerDataManager.Instance.GetMinigameRounds();
        if (gameRounds[0][0]) gameObject.SetActive(false); //If the first round of the first game is not succeded, then we will keep the panel on
    }
}
