using UnityEngine;
using System.Collections;

public class S_MutantSight : MonoBehaviour 
{
    //Initializing view restriction angles
    public float fieldOfViewAngle;
    public float maximumDistance;
    public bool playerInSight;
    public bool playerSpotted;
    public bool inRange;
    private bool playerDead;

    //Initializing general variables
    private SphereCollider col;
    private GameObject player;
    private Transform head;
    private S_WorldEye_Detection eye;
    
    void Awake()
    {
        maximumDistance = 7f;
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        head = FindHead(transform, "Mutant:Head");
        eye = GameObject.FindGameObjectWithTag("WorldEye").GetComponent<S_WorldEye_Detection>();
    }

    void FixedUpdate()
    {
        //If player is dead, make enemy blind
        if(player.GetComponent<S_FirstPersonCharacterController>().isPlayerDead == true)
        {
            playerDead = true;
            inRange = false;
            playerSpotted = false;
            playerInSight = false;
        }
    }

    //Check if the player can be seen by the enemy
    void OnTriggerStay(Collider other)
    {
        //If collided object is player and player not dead
        if(other.gameObject == player && playerDead == false)
        {
            playerInSight = false;

            //Find the angle between the head forward vector and head to player vector
            Vector3 direction = Camera.main.transform.position - head.transform.position;
            float angle = Vector3.Angle(head.transform.forward, direction);

            //If the player is in the field of view perform raycast
            if (angle < fieldOfViewAngle)
            {
                RaycastHit hit;
                Ray ray = new Ray(head.transform.position, direction.normalized);

                //Using layermask to prevent objects in the Enemy layer from preventing the raycast
                int layermask = 1 << 10;
                layermask = ~layermask;

                //Perform raycast to check if the player can actually be seen by the enemy's eye or is behind a barrier
                if (Physics.Raycast(ray, out hit, col.radius * gameObject.transform.lossyScale.x, layermask))
                {
                    if (hit.collider.gameObject == player)
                    {
                        //If successful, player is spotted
                        playerInSight = true;
                        playerSpotted = true;

                        //Inform the world that the player is spotted (all other enemies in the world)
                        eye.WorldPlayerSpotted();

                        //Check threshold distances
                        if ((player.transform.position - transform.position).magnitude <= maximumDistance)
                            inRange = true;
                        else
                            inRange = false;
                    }
                    else
                        inRange = false;
                }
                else
                    inRange = false;
            }
            else
                inRange = false;
        }
    }

    //Finding the head transform of the enemy prefab
    private Transform FindHead(Transform transform, string name)
    {
        if (transform.name == "Mutant:Head") return transform;
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform result = FindHead(transform.GetChild(i), name);
            if (result != null) return result;
        }
        return null;
    }
}
