using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectPoolMinigame
{
    public class PlayerCanvas : MonoBehaviour
    {
        [Header("Player UI Elements")]
        [SerializeField] TextMeshProUGUI ammoText;
        [SerializeField] TextMeshProUGUI superiorText;
        [SerializeField] TextMeshProUGUI reloadingText;
        [SerializeField] Image reloadingSprite;

        Coroutine reloadCoroutine;

        void Start()
        {
            GameObject.FindAnyObjectByType<GameManager>().OnEnemyDefeated += UpdateSuperiorText;
        }

        //Invoked when the players shoots or reload a weapon
        void UpdateAmmoText(int ammoAmount)
        {
            ammoText.text = $"Munición: {ammoAmount.ToString()}";
        }

        //Invoked when the player eliminates someone
        void UpdateSuperiorText(int defeatedEnemies, int totalEnemiesToWin)
        {
            superiorText.text = $"OBJETIVOS ELIMINADOS: {defeatedEnemies} DE {totalEnemiesToWin}";
        }

        //Invoked when the player reaload his weapon
        void UpdateReloadUI(float reloadTime)
        {
            reloadCoroutine = StartCoroutine(ReloadAnimation(reloadTime));
        }

        //Plays the reload UI animation
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

        //Subscribe the PlayerCanvas to the current player's weapon events
        public void SubscribeToCurrentWeapon(IWeapon currentWeapon)
        {
            currentWeapon.onAmmoChange += UpdateAmmoText;
            currentWeapon.onReloadWeapon += UpdateReloadUI;
        }
    }
}
