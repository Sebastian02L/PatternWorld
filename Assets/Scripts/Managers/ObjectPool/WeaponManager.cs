using System.Collections.Generic;
using Unity.VisualScripting;
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

        public void ChangeWeapon()
        {
            currentWeapon = null;
            currentWeaponGO.SetActive(false);
            InstantiateWeapon(1);
        }

        void ShootWeapon()
        {
            currentWeapon.Shoot();
        }

        void CancelShoot()
        {
            currentWeapon.ShootCanceled();
        }

        void RealoadWeapon(InputAction.CallbackContext ctx) 
        {
            currentWeapon.Reload();
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
