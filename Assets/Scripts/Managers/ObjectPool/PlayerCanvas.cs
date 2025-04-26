using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectPoolMinigame
{
    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammoText;
        [SerializeField] TextMeshProUGUI superiorText;
        [SerializeField] TextMeshProUGUI reloadingText;
        [SerializeField] Image reloadingSprite;

        Coroutine reloadCoroutine;

        void Start()
        {
            GameObject.FindAnyObjectByType<GameManager>().OnEnemyDefeated += UpdateSuperiorText;
        }

        void UpdateAmmoText(int ammoAmount)
        {
            ammoText.text = $"Munición: {ammoAmount.ToString()}";
        }

        void UpdateSuperiorText(int defeatedEnemies, int totalEnemiesToWin)
        {
            superiorText.text = $"OBJETIVOS ELIMINADOS: {defeatedEnemies} DE {totalEnemiesToWin}";
        }

        void UpdateReloadUI(float reloadTime)
        {
            reloadCoroutine = StartCoroutine(ReloadAnimation(reloadTime));
        }

        IEnumerator ReloadAnimation(float reloadTime)
        {
            float timer = 0;
            reloadingText.gameObject.SetActive(true);
            reloadingSprite.gameObject.SetActive(true);
            while (timer < reloadTime)
            {
                reloadingSprite.fillAmount = Mathf.Max(0, Mathf.Min(1, timer/reloadTime));
                timer += Time.deltaTime;
                yield return null;
            }
            reloadingText.gameObject.SetActive(false);
            reloadingSprite.gameObject.SetActive(false);
            reloadCoroutine = null;
        }

        public void SubscribeToCurrentWeapon(IWeapon currentWeapon)
        {
            currentWeapon.onAmmoChange += UpdateAmmoText;
            currentWeapon.onReloadWeapon += UpdateReloadUI;
        }
    }
}
