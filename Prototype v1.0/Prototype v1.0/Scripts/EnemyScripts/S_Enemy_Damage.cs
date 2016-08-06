using UnityEngine;
using System.Collections;

//This script is attached to each body part of the enemy to do respective damage and recoil
public class S_Enemy_Damage : MonoBehaviour
{
    //General initializations
    private Transform mainBody;
    private S_Enemy_HealthSystem healthSystem;
    private S_WorldEye_Detection eye;

    //Intializing parameters responsible for damage
    private GameObject spear;
    public float spearDamagePercent;
    private float spearDamage;
    private float damageMagnitude;

    //Initializing parameters responsbile for recoil of bodypart hit
    private Vector3 preAnimatePosition;
    private Vector3 preAnimateRotation;
    private Vector3 hitSpot;
    private float recoilMultiplier;

    //Initializing parameters responsible for particle effect
    public GameObject bloodSpawn;
    private S_BloodBank bloodBank;

    //Initializing parameters responsible for crosshair effect
    private GameObject crosshairDamage;
    
    void Start()
    {
        crosshairDamage = GameObject.FindGameObjectWithTag("UI").transform.FindChild("Crosshair_Damage").gameObject;
        eye = GameObject.FindGameObjectWithTag("WorldEye").GetComponent<S_WorldEye_Detection>();
        recoilMultiplier = 1f;
        mainBody = transform.root;
        healthSystem = mainBody.GetComponent<S_Enemy_HealthSystem>();
        spearDamage = spearDamagePercent * healthSystem.maxHealth;
        bloodBank = GameObject.FindGameObjectWithTag("BloodBank").GetComponent<S_BloodBank>();
    }
    
    //Object enters body, check if spear
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.StartsWith("Spear"))
        {
            //Finding point of entry
            hitSpot = col.contacts[0].point;

            //Calculating the damage to be taken based on the velocity multiplier variable in the spear
            spear = col.gameObject;
            if(spear.GetComponent<S_Weapon_Spear_Impact>()!=null)
                damageMagnitude = spear.GetComponent<S_Weapon_Spear_Impact>().velocityMultiplier;
            float damage = (spearDamage * damageMagnitude);

            //Damaging the enemy health
            healthSystem.takeDamage(damage);

            //Playing crosshair animation based on damage
            crosshairDamage.GetComponent<S_HUD_CrosshairHit>().damagedEnemy(damage / healthSystem.maxHealth);

            //Spawning blood based on damage
            bloodBank.SpawnBlood(transform, hitSpot, transform.rotation, damage/healthSystem.maxHealth);

            //Inform the world that the player has attacked an enemy and is spotted
            eye.WorldPlayerSpotted();
        }
    }

    //Animate the body based on damage position and velocity [NOT WORKING]
    void AnimateDamage()
    {
        preAnimatePosition = transform.localPosition;
        preAnimateRotation = transform.localEulerAngles;

        mainBody.GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<Rigidbody>().AddForceAtPosition(spear.transform.forward.normalized * recoilMultiplier, hitSpot);
        GetComponent<Rigidbody>().AddForce(spear.transform.forward.normalized * recoilMultiplier);
        StartCoroutine("Delay");
    }

    void BloodSplatter(float damage)
    {
        if(damage > 0.0f)
        {
            bloodSpawn.transform.GetChild(5).GetComponent<ParticleSystem>().Play();
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.02f);

        GetComponent<Rigidbody>().isKinematic = true;
        transform.localPosition = preAnimatePosition;
        transform.localEulerAngles =preAnimateRotation;
        mainBody.GetComponent<Animator>().enabled = true;
    }
}
