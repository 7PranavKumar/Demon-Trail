using UnityEngine;
using System.Collections;

public class S_DemonAI : MonoBehaviour
{
    //General variables 
    private Animator anim;
    private S_DemonSight enemySight;
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

    //Teleport Escape variables
    private bool tpInSequence;
    private ParticleSystem teleportParticle;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemySight = GetComponent<S_DemonSight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        teleportParticle = transform.Find("Demon_Teleport").GetComponent<ParticleSystem>();
        teleportParticle.playbackSpeed = 3.0f;
    }

    void Update()
    {
        //If player is too close, teleport away
        if((enemySight.isTeleportDistance && enemySight.inRange) || tpInSequence)
        {
            Teleport();
        }

        //If player is within attack range, attack the player
        else if(enemySight.inRange)
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
            if(patrol == true)
            Patrolling();
    }

    //Perfrom the teleport
    void Teleport()
    {   //Stop enemy movement, set animation
        nav.SetDestination(transform.position);
        anim.SetBool("Teleport", true);

        teleportParticle.Play();

        tpInSequence = true;

        //Perform teleport sequence at the right stage of the animation
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(2);
        if (currentState.normalizedTime % 1.0f > 0.45f && currentState.IsName("Teleport.TeleportEnemy"))
        {
            NavMeshHit tpPosition;
            NavMesh.SamplePosition(player.transform.position + 1.25f * (Random.onUnitSphere * enemySight.maximumDistance), out tpPosition, 4.4f, 1);
            if (tpPosition.position.magnitude != Mathf.Infinity)
                transform.position = tpPosition.position;

            enemySight.inRange = false;
            tpInSequence = false;
        }
    }

    //Stop moving and attack with right animations
    void Attacking()
    {
         nav.SetDestination(transform.position);
         anim.SetBool("Attack01", true);
         anim.SetBool("inRange", true);
         anim.SetBool("Teleport", false);
    }

    //Chase the player with the right animations and speed
    void Chasing()
    {
        anim.SetBool("inRange", false);
        anim.SetBool("Attack01", false);
        anim.SetBool("Teleport", false);

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

            if (patrolWayPoints!=null)
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
