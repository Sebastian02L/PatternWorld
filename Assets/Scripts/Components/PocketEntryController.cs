using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PocketEntryController : MonoBehaviour
{
    [SerializeField] string minigameName;
    [SerializeField] Material lockMaterial;
    [SerializeField] Material unlockedMaterial;

    bool unlocked = false;
    public bool MinigameUnlocked => unlocked;
    public string MinigameName => minigameName;

    private void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().material = (unlocked) ? unlockedMaterial : lockMaterial;
    }

    public void SetUnlocked(bool value)
    {
        unlocked = value;
    }
}
