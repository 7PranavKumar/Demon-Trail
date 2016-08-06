using UnityEngine;
using System.Collections;

public class S_NightshadeAI : MonoBehaviour
{
    //General variables 
    private Animator anim;
    private S_NightshadeSight enemySight;
    private NavMeshAgent nav;
    private Transform player;

    //Patrol variables
    public float patrolSpeed = 1f;
    public float patrolWaitTime = 0.001f;
    public Transform[] patrolWayPoints;
    private float patrolTimer;
    private float AngularSpeed;
    private int wayPointIndex = 0;
    private int nextPoint = 0;
    private bool once = false;
    public bool patrol = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemySight = GetComponent<S_NightshadeSight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //If player is too close, push him away
        if(enemySight.isBlastDistance && enemySight.playerInSight)
        {
            PushBlast();
        }

        //If player is within attack range, attack the player
        else if (enemySight.inRange)
        {
            Attacking();
        }

        //If the player is not in attack range but is spotted, chase the player
        else if (enemySight.playerSpotted)
        {
            Chasing();
        }
        //If none of the above, attempt to patrol
        else
            if (patrol == true)
                Patrolling();
    }

    //Stop moving and push the player away with right animations
    void PushBlast()
    {
        nav.SetDestination(transform.position);
        anim.SetBool("isAttack", false);
        anim.SetBool("inRange", false);
        anim.SetBool("isAttack1", true);
        anim.SetBool("isTooClose", true);
    }

    //Stop moving and attack with right animations
    void Attacking()
    {
        nav.SetDestination(transform.position);
        anim.SetBool("isAttack", true);
        anim.SetBool("inRange", true);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isTooClose", false);
    }

    //Chase the player with the right animations and speed
    void Chasing()
    {
        anim.SetBool("inRange", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isTooClose", false);

        nav.speed = 3f;
        nav.acceleration = 3f;
        nav.SetDestination(player.transform.position);
    }

    //Patrol controls
    void Patrolling()
    {
        //Set up patrol properties
        anim.SetBool("isPatrolling", true);
        nav.speed = patrolSpeed;
        nav.stoppingDistance = 8;
        nav.acceleration = 3;

        if (nextPoint > patrolWayPoints.Length - 1)
            nextPoint = 0;

        if (patrolWayPoints != null)
            nav.SetDestination(patrolWayPoints[nextPoint].position);

        //If the enemy is at the end of a patrol point
        if (!nav.pathPending)
        {
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
                {
                    //Stop, Search for the next patrol point, and play the right turn animations depending on the angle
                    //TODO: Add turn animations based on angularspeed
                    nav.SetDestination(transform.position);
                    if (once == false)
                    {
                        wayPointIndex = nextPoint;
                        if (wayPointIndex > patrolWayPoints.Length - 2)
                            wayPointIndex = 0;
                        AngularSpeed = FindAngle(transform.forward, patrolWayPoints[wayPointIndex + 1].position - transform.position);
                        anim.SetFloat("AngularSpeed", AngularSpeed);
                        once = true;
                        StartCoroutine("Wait");
                    }
                }
            }
        }
    }

    //Returns the angle between two vectors along with the direction using cross product
    float FindAngle(Vector3 forward, Vector3 toward)
    {
        float angle = Vector3.Angle(forward, toward);
        angle = Mathf.Sign(Vector3.Cross(forward, toward).z) * angle;
        return angle;
    }

    //Time Delay - Wait and reset patrol variables
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.6f);
        AngularSpeed = 0;
        anim.SetFloat("AngularSpeed", AngularSpeed);
        once = false;
        nextPoint++;
    }
}
