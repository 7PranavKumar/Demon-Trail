using UnityEngine;
using System.Collections;

public class S_NightshadeAnimation : MonoBehaviour
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
        anim.SetFloat("Speed", nav.velocity.magnitude / nav.speed);
    }
}
