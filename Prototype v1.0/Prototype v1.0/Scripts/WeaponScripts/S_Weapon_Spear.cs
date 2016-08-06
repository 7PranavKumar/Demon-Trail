using UnityEngine;
using System.Collections;

public class S_Weapon_Spear : MonoBehaviour
{
    //Initializing general variables
    private Camera playerCam;
    private GameObject player;

    //Initializing properly variables
    public bool isSpearFlying = false;
    public bool isSpearThrown = false;

    //Initializing particle variables
    private TrailRenderer trail;

    //Initializing crosshair variables
    private GameObject Crosshair;

	void Start ()  
    {
       playerCam = Camera.main;
       player = GameObject.FindGameObjectWithTag("Player");
       trail = GetComponent<TrailRenderer>();
       trail.enabled = false;
	}

    void Update()
    {
        //Ensure that the spear is pointing in the direction its moving
        if(isSpearFlying && GetComponent<Rigidbody>().velocity.magnitude != 0)
        transform.forward = GetComponent<Rigidbody>().velocity;

        //If the spear falls out of the world, kill it if its y < - 20
        if(transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }

    //Called by manager when firing spear, adds force to the spear based on power/maxPower
    public bool FireWeapon(float power, float maxPower, float speed)
    {
        //Stop spear movement with respect to camera
        playerCam.GetComponent<S_MouseLook>().newSpear(null);
        transform.parent = null;
        trail.enabled = true;

        //Add force to the spear with a little swing depending on player's direction of movement
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce((playerCam.transform.forward + Input.GetAxis("Horizontal")/35.0f * playerCam.transform.right.normalized) * speed * power / maxPower);

        isSpearFlying = true;
        player.GetComponent<S_FirstPersonCharacterController>().allowRolling = true;
        isSpearThrown = true;

        return isSpearFlying;
    }
}
