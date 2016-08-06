using UnityEngine;
using System.Collections;

//This script is enabled once the player is dead
public class S_DeadCamera : MonoBehaviour
{
    //Initializing variables to control game restart
    private Ray ray;
    private float distFromGround = 0.1f;
    private float timeBeforeRestart = 3.5f;
    private bool Once;

	void Start () 
    {
        ray = new Ray(transform.position, -transform.up);

        //Adding a small force to the camera rigidbody in the up axis when the player dies
        GetComponent<Rigidbody>().AddForce(Vector3.up * 0.05f);
	}
	
	void Update ()
    {
        //Checking the camera has hit the ground
        //TODO: Currently not working if the camera falls on its side [Needs fix]
        if (Physics.Raycast(ray, distFromGround) && Once == false)
        {
            Once = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine("RestartGame");
        }
	}

    //Restarting game after a certain period
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(timeBeforeRestart);
        Application.LoadLevel(Application.loadedLevel);
    }
}
