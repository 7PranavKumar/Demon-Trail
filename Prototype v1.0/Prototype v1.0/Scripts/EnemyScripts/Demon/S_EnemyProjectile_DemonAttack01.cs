using UnityEngine;
using System.Collections;

public class S_EnemyProjectile_DemonAttack01 : MonoBehaviour
{
    //Variables for flameball's particles effect
    private GameObject flameBallSkill;
    private GameObject flameBall;
    private GameObject flameBallBurst;
    private GameObject player;

    //Various parameters
    private Vector3 endPos;
    private bool fire;
    private bool damageDone;
    private bool playerHit;
    private float speed;
    private float maxDamage;
    private float damage;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 2.75f;
        maxDamage = 0.75f;

        flameBallSkill = transform.FindChild("flameBallSkill").gameObject;
        flameBall = transform.FindChild("flameBall").gameObject;
        flameBallBurst = transform.FindChild("flameBallBurst").gameObject;

        endPos = player.transform.position;

        //Activate set up particles immediately on spawn
        flameBall.SetActive(true);
        flameBallSkill.SetActive(true);
	}
	
	void Update()
    {
        //Spherically Lerp the projectile from its current position to the position of the player when spawned (endPos)
        transform.LookAt(endPos);
        transform.position = Vector3.Slerp(transform.position, endPos, Time.deltaTime * speed);

        //If close to destination, explode
        if(((transform.position - endPos).magnitude < Vector3.one.magnitude) || playerHit)
        {
            flameBall.SetActive(false);
            flameBallSkill.SetActive(false);
            flameBallBurst.SetActive(true);
            fire = true;
        }

        //If exploded, perform the damage to the player at the right time in the explosion animation
        if (fire == true)
        {
            if (flameBallBurst.gameObject.GetComponentInChildren<ParticleSystem>().time > 0.075f && damageDone == false)
            {
                RaycastDamage();
            }

            //Destroy the object once explosion animation is done
            if (flameBallBurst.gameObject.GetComponentInChildren<ParticleSystem>().isStopped)
            {
                Destroy(gameObject);
            }
        }
    }

    //If fireball hits the player directly, explode
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player)
            playerHit = true;
    }

    //Damage the player ONLY if he is visible and based on his distance from the explosion center
    private void RaycastDamage()
    {
        damageDone = true;

        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;

        //Raycasting on explosion
        if(Physics.Raycast(ray, out hit, 4f))
        {
            if(hit.collider.gameObject.name == "Player")
            {
                //Calcuating parameters and performing damage and knockback accordingly
                float distance = (player.transform.position - transform.position).magnitude;
                float damageRatio = (1.0f - distance / 4.0f) * maxDamage;

                damageRatio = Mathf.Abs(damageRatio);
                player.GetComponent<S_PlayerHealthSystem>().playerDamage(damageRatio);
                player.GetComponent<S_PlayerHealthSystem>().KnockBack((player.transform.position - transform.position) * 20f + 12f * Vector3.up, 3f);            
            }
        } 
    }
}
