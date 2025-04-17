using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectPoolMinigame
{
    public class WeaponManager : MonoBehaviour
    {
        //Origin of the "player's hand"
        [SerializeField] Transform weaponsOrigin;
        public List<GameObject> weapons;
        IWeapon currentWeapon;

        PlayerInput input;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            input.actions["Reload"].performed += RealoadWeapon;

            currentWeapon = Instantiate(weapons[0], weaponsOrigin).GetComponent<IWeapon>();
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
            }
        }

        void ShootWeapon()
        {
            currentWeapon.Shoot();
        }

        void RealoadWeapon(InputAction.CallbackContext ctx) 
        {
            currentWeapon.Reload();
        }

        public IWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }
    }
}
