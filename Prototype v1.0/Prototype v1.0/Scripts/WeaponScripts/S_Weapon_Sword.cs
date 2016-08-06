using UnityEngine;
using System.Collections;

public class S_Weapon_Sword : MonoBehaviour
{
    //Variables for Sword Damage
    private S_SwordManager sword;
    private float damage = 90.0f;

    //Variables for Sword Blood Particles
    private bool doOnce;
    private bool spawnBlood;
    private GameObject swordBloodParticle;

    void Start()
    {
        sword = transform.parent.GetComponent<S_SwordManager>();
        swordBloodParticle = transform.FindChild("SwordFlow").gameObject;
        swordBloodParticle.SetActive(false);
    }

    void FixedUpdate()
    {
        //If doing damage, start particle loop
        if(spawnBlood == true && doOnce == false)
        {
            doOnce = true;
            swordBloodParticle.SetActive(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        //Damage enemy when collide with enemy
        if (col.gameObject.layer == 10)
        {
            col.transform.root.GetComponent<S_Enemy_HealthSystem>().takeDamage(Time.deltaTime * damage * (sword.power / sword.maxPower));
            spawnBlood = true;
        }

        //Alert other enemies in the area
        if (col.transform.root.GetComponent<S_MutantSight>() != null)
            col.transform.root.GetComponent<S_MutantSight>().playerSpotted = true;

        if (col.transform.root.GetComponent<S_DemonSight>() != null)
            col.transform.root.GetComponent<S_DemonSight>().playerSpotted = true;

        if (col.transform.root.GetComponent<S_ParasiteSight>() != null)
            col.transform.root.GetComponent<S_ParasiteSight>().playerSpotted = true;

        if (col.transform.root.GetComponent<S_NightshadeSight>() != null)
            col.transform.root.GetComponent<S_NightshadeSight>().playerSpotted = true;
    }

    //If stop doing damage, stop blood spawn
    void OnTriggerExit()
    {
        swordBloodParticle.SetActive(false);
        spawnBlood = false;
        doOnce = false;
    }
}