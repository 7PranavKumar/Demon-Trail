using UnityEngine;
using System.Collections;

public class S_Weapon_Spear_SoundAlert : MonoBehaviour 
{
    private S_Weapon_Spear spear;

    void Awake()
    {
        spear = GetComponent<S_Weapon_Spear>();
    }

    //Spear flies near enemy, detect the player
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10 && spear.isSpearFlying && other.GetComponent<S_MutantSight>()!=null)
        {
            other.GetComponent<S_MutantSight>().playerSpotted = true;
        }

        if (other.gameObject.layer == 10 && spear.isSpearFlying && other.GetComponent<S_DemonSight>() != null)
        {
            other.GetComponent<S_DemonSight>().playerSpotted = true;
        }

        if (other.gameObject.layer == 10 && spear.isSpearFlying && other.GetComponent<S_ParasiteSight>() != null)
        {
            other.GetComponent<S_ParasiteSight>().playerSpotted = true;
        }

        if (other.gameObject.layer == 10 && spear.isSpearFlying && other.GetComponent<S_NightshadeSight>() != null)
        {
            other.GetComponent<S_NightshadeSight>().playerSpotted = true;
        }
    }
}
