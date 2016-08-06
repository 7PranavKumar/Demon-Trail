using UnityEngine;
using System.Collections;

public class S_NightshadeAttack : MonoBehaviour 
{
    //Initializing general variables
    private Animator anim;
    private Transform player;

    //Initializing variables used when attacking the player
    private float blastDamage;
    private bool once;
    private bool once1;
    private Transform firePos;
    public GameObject projectile;
    private S_PlayerHealthSystem playerHealth;
    private S_NightshadeSight enemySight;

    //Initializing particle effect
    private GameObject blastEffect;

    void Start()
    {
        blastDamage = 0.5f;
        anim = GetComponent<Animator>();
        firePos = FindFirePos(transform, "RightHandMiddle1");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<S_PlayerHealthSystem>();
        enemySight = GetComponent<S_NightshadeSight>();
        blastEffect = transform.FindChild("Nightshade_Blast").gameObject;
        blastEffect.SetActive(false);
    }

    void Update()
    {
        //If the enemy is playing the attack animation, instantiate the set projectile at the right transform and at the right moment in animation
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Attack") && currentState.normalizedTime % 1.0f <= 0.45f && currentState.normalizedTime % 1.0f >= 0.33f && once == false)
        {
            int randomNumberOfSpawns = Random.Range(5, 8);
            for (int i = 0; i < randomNumberOfSpawns; i++)
            {
                Instantiate(projectile, firePos.position, firePos.rotation);
            }
            once = true;
        }

        //If the enemy is playing the attack1 animation, instantiate the set projectile at the right transform and at the right moment in animation
        if (currentState.IsName("Attack1") && currentState.normalizedTime % 1.0f <= 0.55f && currentState.normalizedTime % 1.0f >= 0.43f && once1 == false)
        {
            once1 = true;
            if (enemySight.isBlastDistance)
            {
                playerHealth.KnockBack((player.transform.position - transform.position) * 6f + 6 * Vector3.up, 3f);
                playerHealth.playerDamage(blastDamage);
            }

            //Play particle effect
            blastEffect.SetActive(true);
        }

        //Prevent multiple instantiations
        if (currentState.IsName("Attack") && currentState.normalizedTime % 1.0f >= 0.92f)
        {
            once = false;
        }

        //Prevent multiple instantiations
        if (currentState.IsName("Attack1") && currentState.normalizedTime % 1.0f >= 0.92f)
        {
            once1 = false;
            blastEffect.SetActive(false);
        }
    }

    // Finding the fire transform in the model using recursive search - in this case, looking for the middle finger of the right hand
    private Transform FindFirePos(Transform transform, string name)
    {
        if (transform.name == "RightHandMiddle1") return transform;
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform result = FindFirePos(transform.GetChild(i), name);
            if (result != null) return result;
        }
        return null;
    }
}
