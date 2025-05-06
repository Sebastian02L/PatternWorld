using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectPoolMinigame
{
    public class WeaponManager : MonoBehaviour
    {
        //Origin of the "player's hand"
        [SerializeField] Transform weaponsOrigin;
        List<WeaponData> weaponsData = new List<WeaponData>();
        List<GameObject> weapons = new List<GameObject>();
        GameObject currentWeaponGO;
        IWeapon currentWeapon;

        PlayerInput input;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            input.actions["Reload"].performed += RealoadWeapon;
        }

        private void Start()
        {
            InstantiateWeapon(0);
        }

        private void OnDestroy()
        {
            input.actions["Reload"].performed -= RealoadWeapon;
        }

        private void Update()
        {
            if (input.actions["Shoot"].IsPressed())
            {
                ShootWeapon();
            } else if (input.actions["Shoot"].WasReleasedThisFrame())
            {
                CancelShoot();
            }
        }

        void InstantiateWeapon(int weaponTurn)
        {
            currentWeaponGO = Instantiate(weapons[weaponTurn], weaponsOrigin);
            currentWeapon = currentWeaponGO.GetComponent<IWeapon>();
            currentWeapon.SetWeaponData(weaponsData[weaponTurn]);
        }

        public void ChangeWeapon(PlayerInput playerInput)
        {
            Debug.Log("Cambiando armas");
            playerInput.actions["Shoot"].Disable();
            playerInput.actions["Reload"].Disable();
            currentWeapon = null;
            currentWeaponGO.SetActive(false);
            InstantiateWeapon(1);
            playerInput.actions["Shoot"].Enable();
            playerInput.actions["Reload"].Enable();
            Debug.Log("Arma cambiada");
        }

        void ShootWeapon()
        {
           if(!PauseController.IsGamePaused) currentWeapon.Shoot();
        }

        void CancelShoot()
        {
            if (!PauseController.IsGamePaused) currentWeapon.ShootCanceled();
        }

        void RealoadWeapon(InputAction.CallbackContext ctx) 
        {
            if (!PauseController.IsGamePaused) currentWeapon.Reload();
        }

        public IWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public void SetWeaponsData(List<WeaponData> weaponData)
        {
            weaponsData = weaponData;
            weapons.Add(weaponsData[0].weaponPrefab);
            weapons.Add(weaponsData[1].weaponPrefab);
        }
    }
}
