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
        WeaponData currentWeaponData;
        IWeapon currentWeapon;

        PlayerInput input;
        WeaponInfoCanvasManager weaponInfoCanvasManager;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            weaponInfoCanvasManager = FindAnyObjectByType<WeaponInfoCanvasManager>();
            input.actions["Reload"].performed += RealoadWeapon;
            input.actions["ShowInfo"].performed += ShowWeaponInfo;
        }

        private void Start()
        {
            InstantiateWeapon(0);
        }

        private void OnDestroy()
        {
            input.actions["Reload"].performed -= RealoadWeapon;
            input.actions["ShowInfo"].performed -= ShowWeaponInfo;
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
            currentWeaponData = weaponsData[weaponTurn];
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
           if(!PauseController.IsGamePaused && !weaponInfoCanvasManager.isPaneActive) currentWeapon.Shoot();
        }

        void CancelShoot()
        {
            if (!PauseController.IsGamePaused) currentWeapon.ShootCanceled();
        }

        void RealoadWeapon(InputAction.CallbackContext ctx) 
        {
            if (!PauseController.IsGamePaused && !weaponInfoCanvasManager.isPaneActive) currentWeapon.Reload();
        }

        void ShowWeaponInfo(InputAction.CallbackContext ctx)
        {
            weaponInfoCanvasManager.EvaluaPanelVisualization(currentWeaponData.description);
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
