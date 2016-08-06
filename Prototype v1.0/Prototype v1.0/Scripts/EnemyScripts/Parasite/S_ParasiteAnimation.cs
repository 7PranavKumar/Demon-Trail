using UnityEngine;
using System.Collections;

//Simple script to monitor the blend of forward movement animation
public class S_ParasiteAnimation : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator anim;

    void Awake()
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
        anim.SetFloat("Speed", nav.velocity.magnitude / nav.speed);
    }
}
