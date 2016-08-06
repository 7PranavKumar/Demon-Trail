using UnityEngine;
using System.Collections;

public class S_DemonAttack : MonoBehaviour 
{
    //Initializing general variables
    private Animator anim;
    
    //Initializing variables used when attacking the player
    private bool once;
    private Transform firePos;
    public GameObject projectile;

    void Start()
    {
        anim = GetComponent<Animator>();
        firePos = FindFirePos(transform, "RightHandMiddle1");
    }

    void Update()
    {
        //If the enemy is playing the attack animation, instantiate the set projectile at the right transform and at the right moment in animation
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(1);
        if (currentState.IsName("Attack.Attack01") && currentState.normalizedTime % 1.0f <= 0.45f && currentState.normalizedTime % 1.0f >= 0.38f && once == false)
        {
            Instantiate(projectile, firePos.position, firePos.rotation);
            once = true;
        }
        
        //Prevent multiple instantiations
        if (currentState.IsName("Attack.Attack01") && currentState.normalizedTime % 1.0f >= 0.92f)
        {
            once = false;
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
