using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponManager : MonoBehaviour
{
    public WeaponType currentWeapon = WeaponType.Weapon1;

    [SerializeField] private InputActionReference swapWeapon1;
    [SerializeField] private InputActionReference swapWeapon2;

    private void Update()
    {
       if(swapWeapon1.action.WasPressedThisFrame())
       {
            int nextWeapon = (int)currentWeapon + 1;
            
            if (nextWeapon > 2)
            {
                nextWeapon = 0;
            }
            currentWeapon = (WeaponType)nextWeapon;
            Debug.Log("Current Weapon: " + currentWeapon);
       }
       if(swapWeapon2.action.WasPressedThisFrame())
       {
            int previousWeapon = (int)currentWeapon - 1;
            
            if (previousWeapon < 0)
            {
                previousWeapon = 2;
            }
            currentWeapon = (WeaponType)previousWeapon;
            Debug.Log("Current Weapon: " + currentWeapon);
       }
    }

    private void OnEnable()
    {
        swapWeapon1.action.Enable();
        swapWeapon2.action.Enable();
    }

    private void OnDisable()
    {
        swapWeapon1.action.Disable();
        swapWeapon2.action.Disable();
    }
}
