using UnityEngine;
using System.Collections;

public class S_ParasiteAI : MonoBehaviour 
{
    //General variables
    private Animator anim;
    private S_ParasiteSight enemySight;
    private NavMeshAgent nav;
    private Transform player;

    //Patrol variables
    public Transform[] patrolWayPoints;
    public float patrolSpeed;
    public float patrolWaitTime;
    private float patrolTimer;
    private float AngularSpeed;
    private int wayPointIndex;
    private int nextPoint;
    private bool once;

    //One-off animation variables
    private bool mockOnce;
    private bool scream;

    void Start()
    {
        wayPointIndex = 0;
        nextPoint = 0;
        once = false;
        mockOnce = false;
        scream = false;

        anim = GetComponent<Animator>();
        enemySight = GetComponent<S_ParasiteSight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //If player is spotted, chase the player
        if (enemySight.playerSpotted)
        {
            Chasing();
        }

        //If player is dead, play the mock animation
        else if (player.GetComponent<S_FirstPersonCharacterController>().isPlayerDead)
        {
            //Ensure that mock animation is played only ONCE
            if (mockOnce == false)
            {
                nav.SetDestination(Camera.main.transform.position);

                //Walk towards the player before performing the mock animation
                if (!nav.pathPending)
                {
                    if (nav.remainingDistance <= nav.stoppingDistance)
                    {
                        if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
                        {
                            anim.SetBool("isMock", true);

                            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
                            if (currentState.IsName("Mock"))
                            {
                                if (currentState.normalizedTime % 1.0f > 0.90f)
                                {
                                    mockOnce = true;
                                    anim.SetBool("isMock", false);
                                }
                            }
                        }
                    }
                }
            }

            //If mock animation already played, resume patrolling
            else
            {
                if (patrolWayPoints.Length > 0)
                    Patrolling();
            }
        }

        //If none of the above, start patrolling
        else
        {
            if (patrolWayPoints.Length > 0)
                Patrolling();
        }
    }

    //Chase the player with the right animations and speed
    void Chasing()
    {
        //Perform scream animation once before chase
        if(scream == false)
        {
            anim.SetBool("isScream", true);
        }

        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime % 1 > 0.90 && currentState.IsName("Scream"))
        {
            scream = true;
            anim.SetBool("isScream", false);
        }

        //Starting chasing the player if scream is done and enemy not in middle of Attack animation
        if (scream == true && !currentState.IsName("Attack"))
        {
            nav.speed = 6f;
            nav.stoppingDistance = 5.0f;
            nav.acceleration = 8.0f;
            nav.SetDestination(player.transform.position);
        }  
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
