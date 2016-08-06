using UnityEngine;
using System.Collections;

//Simple script to monitor the blend of forward movement animation
public class S_DemonAnimation : MonoBehaviour 
{
    private NavMeshAgent nav;
    private Animator anim;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        SetupWalkAnim();
    }

    void SetupWalkAnim()
    {
        anim.SetFloat("ForwardSpeed", nav.velocity.magnitude / nav.speed);
    }
}
