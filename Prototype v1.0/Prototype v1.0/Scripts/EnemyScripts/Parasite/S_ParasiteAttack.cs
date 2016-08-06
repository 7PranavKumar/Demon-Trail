using UnityEngine;
using System.Collections;

public class S_ParasiteAttack : MonoBehaviour
{
    //Initializing general variables
    private Animator anim;
    private Transform player;

    //Initializing variables used when attacking the player
    private S_ParasiteSight enemySight;
    private Transform mouth;
    private bool once;

    void Start()
    {
        enemySight = GetComponent<S_ParasiteSight>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Finding the mouth (attack spray) of the enemy and disabling it on spawn
        mouth = FindMouth(transform, "Parasite_Attack");
        mouth.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        //Retreive the current animation playing
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);

        //If animation is screaming, face the player and enable the mouth at the right time
        if (currentState.IsName("Scream"))
        {
            //Smooth turn towards the player
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8.5f);

            if (currentState.normalizedTime % 1f > 0.30f && currentState.normalizedTime % 1f < .75f)
                mouth.gameObject.SetActive(true);
            else
                mouth.gameObject.SetActive(false);
        }

        //If player is in attack range, face the player and play the attack animation
        if (enemySight.inRange == true)
        {
            transform.LookAt(player.transform);
            anim.SetBool("isAttack", true);
        }

        //If playing the attack animation, activate the mouth (attack spray) according to animation
        if (currentState.IsName("Attack"))
        {
            if (currentState.normalizedTime % 1 > 0.90)
                anim.SetBool("isAttack", false);

            if (currentState.normalizedTime % 1f > 0.32f && currentState.normalizedTime % 1f < .75f)
                mouth.gameObject.SetActive(true);
            else
                mouth.gameObject.SetActive(false);
        }   
    }

    // Finding the Vemon head
    private Transform FindMouth(Transform transform, string name)
    {
        if (transform.name == "Parasite_Attack") return transform;
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform result = FindMouth(transform.GetChild(i), name);
            if (result != null) return result;
        }
        return null;
    }
}
