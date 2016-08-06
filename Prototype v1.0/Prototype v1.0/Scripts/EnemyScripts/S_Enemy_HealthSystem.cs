using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Enemy_HealthSystem : MonoBehaviour 
{
    //Initializing variables responsible for health
    //TODO: Rage behaviour not incorporated into gameplay yet
    public float health;
    public float maxHealth;
    public float rage;
    private float maxRage;
    private float minRage;
    private bool isDead;

    //Initializing general variables
    private NavMeshAgent nav;
    private Animator anim;
    private MonoBehaviour[] scripts;
    public GameObject pickupSpear;

    //Intializing lists of position and rotation where the new spears must spawn
    //Responsible for spawning 'Pickup Spears' once the enemy is dead
    public List<Vector3> respawnPositions = new List<Vector3>();
    public List<Vector3> respawnRotations = new List<Vector3>();

    //Initializing list of attackSpears
    private List<S_Weapon_Spear> allSpears = new List<S_Weapon_Spear>();

    void Start()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rage = 0.0f;
        minRage = 0.0f;
        maxRage = 10.0f;
        scripts = GetComponents<MonoBehaviour>();
    }

    //Add rage to the enemy behaviour (TODO : Incorporate)
    public void AddRage(float add)
    {
        rage += add;
        rage = Mathf.Clamp(rage,minRage,maxRage);
    }

    //Public function responsible for hurting enemy [Damage is done based on ratio rather than absolute values]
    //For eg., for an enemy of 100 health, if input damage = 0.2f, the actual damage done is 20 health.
    public void takeDamage(float damage)
    {
        health = health - damage;

        //If enemy is dead, disable all scripts and enable ragdoll
        if(health <= 0 && isDead == false)
        {
            isDead = true;
            StartCoroutine("KillEnemy");   
        }
    }

    void FindSpearsAttached()
    {
        FindSpearsRecursive(transform.root);

        foreach(S_Weapon_Spear spear in allSpears)
        {
            GameObject spawnedSpear = Instantiate(pickupSpear);
            spawnedSpear.GetComponent<Collider>().enabled = true;
            spawnedSpear.GetComponent<Rigidbody>().isKinematic = false;
            spawnedSpear.transform.position = transform.position + spear.transform.localPosition;
            spawnedSpear.transform.localEulerAngles = transform.localEulerAngles + spear.transform.localEulerAngles;

            Destroy(spear.gameObject);
        }
    }

    //Recursively find all attackspears
    void FindSpearsRecursive(Transform trans)
    {
        if (trans.GetComponent<S_Weapon_Spear>() != null)
        {
            allSpears.Add(trans.GetComponent<S_Weapon_Spear>());
        }
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                FindSpearsRecursive(t);
        else
            return;
    }

    //Recursively changing layer of all ragdoll parts to not collider with player
    void ChangeLayersTo(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                ChangeLayersTo(t, layer);
    }

    //Wait and Destroy the ragdolled enemy
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4.4f);
        Destroy(gameObject);
    }

    IEnumerator DeadEnemies()
    {
        yield return new WaitForFixedUpdate();
        ChangeLayersTo(transform.root, 12);
    }

    IEnumerator KillEnemy()
    {
        yield return new WaitForFixedUpdate();
        nav.SetDestination(transform.position);
        nav.speed = 0.0f;
        anim.enabled = false;

        //Finding all rigidbodies in enemy and disabling kinematic
        Rigidbody[] allRigidBodies = GetComponentsInChildren<Rigidbody>();
        allRigidBodies[0].isKinematic = false;

        foreach (Rigidbody ob in allRigidBodies)
            ob.isKinematic = false;

        //Finding positions of attack spears
        FindSpearsAttached();

        //Disabling scripts
        foreach (MonoBehaviour script in scripts)
            script.enabled = false;

        //Changing layer to dead enemies
        StartCoroutine("DeadEnemies");

        //Destroying gameobject after a certain duration
        StartCoroutine("Destroy");
    }
}
