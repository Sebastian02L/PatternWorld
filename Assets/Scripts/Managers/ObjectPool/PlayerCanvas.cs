using TMPro;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammoText;
        [SerializeField] TextMeshProUGUI superiorText;

        void Start()
        {
            GameObject.FindAnyObjectByType<GameManager>().OnEnemyDefeated += UpdateSuperiorText;
        }

        void UpdateAmmoText(int ammoAmount)
        {
            ammoText.text = $"Munici�n: {ammoAmount.ToString()}";
        }

        void UpdateSuperiorText(int defeatedEnemies, int totalEnemiesToWin)
        {
            superiorText.text = $"OBJETIVOS ELIMINADOS: {defeatedEnemies} DE {totalEnemiesToWin}";
        }

        public void SubscribeToCurrentWeapon(IWeapon currentWeapon)
        {
            currentWeapon.onAmmoChange += UpdateAmmoText;
        }
    }
}
