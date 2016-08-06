using UnityEngine;
using System.Collections;

public class S_Weapon_Spear_Impact : MonoBehaviour 
{
    //Initializing variables to handle spear impact
    private bool targetHit = false;
    
    private float veloMagnitudeBeforeImact;
    public float velocityMultiplier;
    private float maxVelocity = 54.0f; //Magic number - Calculated in the Editor 

    private S_Weapon_Spear spear;
    private Vector3 oldRot;
    private Vector3 oldPos;
    private bool track = true;
    private bool doOnce = false;

    public bool isPickup;

    void Awake()
    {
        spear = GetComponent<S_Weapon_Spear>();
        oldRot = transform.eulerAngles;
    }

    void Update()
    {
        //The moment spear is thrown, we begin tracking
        if(track && spear.isSpearThrown)
        {
            track = true;
        }

        //If target has not been hit, keep track of the velocity multiplier [it is required by the enemy damage script]
        if (targetHit == false)
        {
            veloMagnitudeBeforeImact = GetComponent<Rigidbody>().velocity.magnitude;
            velocityMultiplier = veloMagnitudeBeforeImact / maxVelocity;     
        }

        //If tracking, save current rotation to oldRot and save current position to oldPos
        if (track == true)
        {
            if (oldRot != transform.eulerAngles)
            {
                oldPos = transform.position;
                oldRot = transform.eulerAngles;
            }
        }

        //The moment tracking stops, set the object's position/rotation to oldRot/oldPos which was calculated in the previous frame
        else
        {
            if(doOnce == false)
            {
                transform.position = oldPos;
                transform.eulerAngles = oldRot;
                doOnce = true;
            }      
        }
    }

    //Hit something
    void OnCollisionEnter(UnityEngine.Collision col)
    {
        //Stop moving and set variables accordingly
        if (spear.isSpearFlying)
        {
            //If hit enemy [ 10 -> enemy layer]
            if (col.transform.gameObject.layer == 10)
            {
                GetComponent<Collider>().isTrigger = true;
                GetComponent<Rigidbody>().isKinematic = true;
                spear.isSpearFlying = false;
                track = false;
                targetHit = true;
                transform.parent = col.gameObject.transform;
                isPickup = false;
                StartCoroutine("StopTrail");    
            }

            //If did not hit enemy
            else
            {
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().Sleep();
                GetComponent<Collider>().enabled = false;       
                spear.isSpearFlying = false;
                track = false;
                targetHit = true;
                isPickup = true;
            }
        }     
    }

    IEnumerator StopTrail()
    {
        yield return new WaitForSeconds(GetComponent<TrailRenderer>().time);
        GetComponent<TrailRenderer>().enabled = false;
    }
}
