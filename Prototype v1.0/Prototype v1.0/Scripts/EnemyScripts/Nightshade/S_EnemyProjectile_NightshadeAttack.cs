using UnityEngine;
using System.Collections;

public class S_EnemyProjectile_NightshadeAttack : MonoBehaviour 
{
    //Variables for iceball's particles effect
    private GameObject iceBallSkill;
    private GameObject iceBall;
    private GameObject player;

    //Various parameters
    private Vector3 endPos;
    private bool fire;
    private bool damageDone;
    private float speed;
    private float radius;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 3.1f;

        iceBallSkill = transform.FindChild("iceBallSkill").gameObject;
        iceBall = transform.FindChild("iceBall").gameObject;

        //Getting a random position around the player 
        radius = 4.5f;
        endPos = FindRandomPosition();

        //Activate set up particles immediately on spawn
        iceBall.SetActive(true);
        iceBallSkill.SetActive(true);
    }

    void Update()
    {
        //Lerp the projectile from its current position to the position found using FindRandomPosition
        transform.LookAt(endPos);
        transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * speed);

        //If close to destination, destroy
        if ((transform.position - endPos).magnitude < Vector3.one.magnitude/0.5f)
        {
            Destroy(gameObject);
        }
    }

    //Kill the object once it hits anything
    void OnTriggerEnter()
    {
        Destroy(gameObject);
    }

    //This function returns a random vector3 position behind the player depending on radius
    private Vector3 FindRandomPosition()
    {
        //This generates a random vector3 behind the player
        Vector3 returnVector = ((player.transform.position - transform.root.position).normalized * radius) + player.transform.position + Random.insideUnitSphere * radius;

        //This ensures that the position returned is not below the player
        //If it is below the player send it up
        if (returnVector.y < player.transform.position.y - 0.5f)
        {
            float byHowMuch = Mathf.Abs((player.transform.position.y - 0.5f) - returnVector.y);
            returnVector.y = returnVector.y + byHowMuch * 2f;
        }

        return returnVector;
    }
}
