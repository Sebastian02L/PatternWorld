using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectPoolMinigame
{
    public class WeaponManager : MonoBehaviour
    {
        //Origin of the "player's hand" to place the weapons
        [SerializeField] Transform weaponsOrigin;
        [SerializeField] WeaponInfoCanvasManager weaponInfoCanvasManager;

        List<WeaponData> weaponsData = new List<WeaponData>();
        List<GameObject> weapons = new List<GameObject>();

        GameObject currentWeaponGO;
        WeaponData currentWeaponData;
        IWeapon currentWeapon;

        PlayerInput input;

        //Save the player weapons data and save the prefabs on weapons list
        public void SetWeaponsData(List<WeaponData> weaponData)
        {
            weaponsData = weaponData;
            weapons.Add(weaponsData[0].weaponPrefab);
            weapons.Add(weaponsData[1].weaponPrefab);
        }

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            input.actions["Reload"].performed += RealoadWeapon;
            input.actions["ShowInfo"].performed += ShowWeaponInfo;
        }

        //Instantiates the first player weapon
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

        //Instantiates the weapon indexed by the received number
        void InstantiateWeapon(int weaponTurn)
        {
            currentWeaponGO = Instantiate(weapons[weaponTurn], weaponsOrigin);
            currentWeaponData = weaponsData[weaponTurn];
            currentWeapon = currentWeaponGO.GetComponent<IWeapon>();

            //Sends the data to the specific weapon manager
            currentWeapon.SetWeaponData(weaponsData[weaponTurn]);
        }

        //Deactivates currents player weapon and activates the next one
        public void ChangeWeapon(PlayerInput playerInput)
        {
            playerInput.actions["Shoot"].Disable();
            playerInput.actions["Reload"].Disable();

            currentWeapon = null;
            currentWeaponGO.SetActive(false);
            InstantiateWeapon(1);

            playerInput.actions["Shoot"].Enable();
            playerInput.actions["Reload"].Enable();
        }

        //Shoots the current players weapon when the left mouse button is pressed
        void ShootWeapon()
        {
           if(!PauseController.IsGamePaused && !weaponInfoCanvasManager.isPaneActive) currentWeapon.Shoot();
        }

        //Cancels the shoot of the current players weapon
        public void CancelShoot()
        {
            if (!PauseController.IsGamePaused) currentWeapon.ShootCanceled();
        }

        //Reloads the curren players weapon
        void RealoadWeapon(InputAction.CallbackContext ctx) 
        {
            if (!PauseController.IsGamePaused && !weaponInfoCanvasManager.isPaneActive) currentWeapon.Reload();
        }

        //Sends the player weapon description to the WeaponInfoCanvasManager
        void ShowWeaponInfo(InputAction.CallbackContext ctx)
        {
            weaponInfoCanvasManager.EvaluaPanelVisualization(currentWeaponData.description);
        }

        /*public IWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }*/
    }
}
