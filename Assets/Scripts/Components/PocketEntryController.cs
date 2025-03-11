using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PocketEntryController : MonoBehaviour
{
    [SerializeField] PatternData patternData;

    bool unlocked = false;
    public bool MinigameUnlocked => unlocked;
    public string MinigameName => patternData.minigameName;

    private void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().material = (unlocked) ? patternData.unlockedMaterial : patternData.lockMaterial;
    }

    public void SetUnlocked(bool value)
    {
        unlocked = value;
    }
}
