using UnityEngine;
using System.Collections;

public class S_SpearPickup : MonoBehaviour 
{
    //Initializing general variables
    private GameObject player;
    private S_WeaponManager weaponManager;
    private S_Weapon_Spear_Impact spearImpact;
    private bool doOnce;

    //These variables get set to determine if the pick up is fallen from the enemy or is placed on the ground
    private bool purePickup;
    public bool disableCollision;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponManager = player.GetComponent<S_WeaponManager>();

        //If there exists no spearImpact, the spear is a pure pickup and not fallen from the enemy
        spearImpact = transform.parent.gameObject.GetComponent<S_Weapon_Spear_Impact>();
        if(spearImpact == null)
        {
            purePickup = true;
        }
        if(disableCollision == true)
        {
            transform.parent.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    //On trigger enter, pick up spear if spear ammo is not full
    void OnTriggerEnter(Collider col)
    {     
        if (purePickup == false)
        {
            if (col.gameObject == player && spearImpact.isPickup == true && doOnce == false)
            {
                if (!weaponManager.isAmmoFull())
                {
                    weaponManager.spearCurrentAmmo++;
                    doOnce = true;
                    Destroy(transform.parent.gameObject);
                }
            }
        }
        else
        {
            if (col.gameObject == player && purePickup == true && doOnce == false)
            {
                if (!weaponManager.isAmmoFull())
                {
                    weaponManager.spearCurrentAmmo++;
                    doOnce = true;
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
