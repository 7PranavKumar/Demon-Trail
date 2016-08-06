using UnityEngine;
using System.Collections;

public class S_WeaponManager : MonoBehaviour 
{
    //Initializing variables for managing weapons
    public GameObject[] weaponList = new GameObject[2];
    private GameObject currentWeapon = null;
    private int currentWeaponID = 0; 
    public float spearCurrentAmmo;
    public float spearMaxAmmo;

	void Start () 
    {
        spearCurrentAmmo = 10;
        spearMaxAmmo = 10;

        Equip(weaponList[0]);
        currentWeaponID = 0;
	}
	
	void Update () 
    {
        //Equip spear is ammo available
	    if(Input.GetKeyDown("1") && currentWeaponID != 0 && GetComponent<S_FirstPersonCharacterController>().isPlayerDead == false)
        {
            if (spearCurrentAmmo >= 1)
            {
                Equip(weaponList[0]);
                currentWeaponID = 0;
            }
        }

        //Equip Sword
        if (Input.GetKeyDown("2") && currentWeaponID != 1 && GetComponent<S_FirstPersonCharacterController>().isPlayerDead == false)
        {
            Equip(weaponList[1]);
            currentWeaponID = 1;
        }

        //If player dies, destory the weapon held
        if(GetComponent<S_FirstPersonCharacterController>().isPlayerDead == true)
        {
            if(currentWeapon != null)
                Destroy(currentWeapon);
        }

        //Ensure spear ammo is between 0 and max
        spearCurrentAmmo = Mathf.Clamp(spearCurrentAmmo, 0, spearMaxAmmo);
	}

    //Equiping the weapon
     void Equip(GameObject weapon)
    {
        //Equip spear is ammo available
        if (weapon == weaponList[0] && spearCurrentAmmo >=0)
        {
            Destroy(currentWeapon);
            currentWeapon = Instantiate(weapon);
            currentWeapon.transform.parent = Camera.main.transform;
        }

        //Equip sword
        if (weapon == weaponList[1])
        {
            Destroy(currentWeapon);
            currentWeapon = Instantiate(weapon);
            currentWeapon.transform.parent = Camera.main.transform;
        }     
    }

    public bool isAmmoFull()
    {
        if (spearCurrentAmmo == spearMaxAmmo)
            return true;
        else
            return false;
    }

    public void SubtractSpear()
    {
        spearCurrentAmmo--; 
    }

    public void EquipSword()
    {
        Equip(weaponList[1]);
        currentWeaponID = 1;
    }
}
