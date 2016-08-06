using UnityEngine;
using System.Collections;

public class S_MutantAttack : MonoBehaviour
{
    //Initializing general variables
    private Animator anim;
    private Transform player;
    private NavMeshAgent nav;

    //Initializing variables used when attacking the player
    private S_PlayerHealthSystem playerHealth;
    private S_MutantSight enemySight;
    private bool once;
    private float swingDamage;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<S_MutantSight>();
        swingDamage = 0.34f;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<S_PlayerHealthSystem>();
    }

    void FixedUpdate()
    {
        //If the enemy is in range, perform the attack animation
        if (enemySight.inRange == true)
        {
            nav.speed = 0.0f;
            anim.SetBool("isAttacking", true);

            //Enable the damage and knockback at the right point in the animation
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentState.normalizedTime % 1 > 0.52 && currentState.IsName("mutant_swiping") && currentState.normalizedTime % 1 < 0.6)
            {
                //Ensure the attacking animation only does damage once per attack
                if(once == false)
                {
                    once = true;
                    playerHealth.KnockBack((player.transform.position - transform.position) *8f + 8* Vector3.up, 3f);
                    playerHealth.playerDamage(swingDamage);
                }  
            }
        }

        //If player is not in range, stop attacking animation
        else
        {
            anim.SetBool("isAttacking", false);
            once = false;
        }
    }
}
