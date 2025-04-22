using TMPro;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammoText;

        void Start()
        {
            GameObject.FindAnyObjectByType<WeaponManager>().GetCurrentWeapon().onAmmoChange += UpdateAmmoText;
        }

        void UpdateAmmoText(int ammoAmount)
        {
            ammoText.text = $"Munici�n: {ammoAmount.ToString()}";
        }
    }
}
