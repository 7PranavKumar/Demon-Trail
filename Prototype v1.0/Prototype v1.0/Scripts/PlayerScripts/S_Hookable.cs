using UnityEngine;
using System.Collections;
using System;

public class S_Hookable : MonoBehaviour, IComparable<S_Hookable> //Added IComparable to ensure that we can sort
{
    //Initializing general variables
    private GameObject player;
    private RaycastHit hit;
    private Ray ray;

    //Initializing parameters that determine properties of the hookable object
    private float angle;
    private bool added;
    public float distance; 

    void Start()
    {
        added = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Raycasting from hookable object to the player camera
        //Raycast only till the player position
        ray = new Ray(transform.position, Camera.main.transform.position - transform.position);
        Physics.Raycast(ray, out hit, (Camera.main.transform.position - transform.position).magnitude);

        //If ray hit something
        if (hit.collider != null)
        {
            //If the object is within 30 degrees of either side of the camera's forward angle, and within maxHookLength, and if collider is the player
            //We add it to the hookableObjects list
            if (Vector3.Angle(Camera.main.transform.forward, transform.position - Camera.main.transform.position) <= Mathf.Abs(30) && (Camera.main.transform.position - transform.position).magnitude <= player.GetComponent<S_FirstPersonCharacterController>().maxHookLength+1.0f && hit.collider.name == "Player")
            {
                if (added == false)
                {
                    added = true;
                    player.GetComponent<S_FirstPersonCharacterController>().hookableObjects.Add(gameObject.GetComponent<S_Hookable>());
                    player.GetComponent<S_FirstPersonCharacterController>().hookableObjects.Sort();
                    transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            //The moment it slips away from the player's vision, we remove it from the list
            else
            {
                if (added == true)
                {
                    added = false;
                    player.GetComponent<S_FirstPersonCharacterController>().hookableObjects.Remove(gameObject.GetComponent<S_Hookable>());
                    player.GetComponent<S_FirstPersonCharacterController>().hookableObjects.Sort();
                    transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        //'distance' variable measures the perpendicular distance from the hookableobject to the camera's forward vector. Its rounded off to get a good discrete distance for straightforward sorting
        Vector2 cameraHook = new Vector2(Camera.main.transform.position.x - transform.position.x, Camera.main.transform.position.z - transform.position.z);
        Vector2 cameraForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        distance = Mathf.Abs(cameraHook.magnitude * Mathf.Sin(Mathf.Deg2Rad * (Vector2.Angle(cameraHook, cameraForward))));
        distance = Mathf.Round((distance * 1000f)/10f);
    }

    //Function helps sort the list based on the value of distance
    public int CompareTo(S_Hookable other)
    {
        if(other == null)
        {
            return 1;
        }

        return (int)(distance - other.distance);
    }

    //Destroying the hookable
    public void Destroy()
    {
        player.GetComponent<S_FirstPersonCharacterController>().hookableObjects.Remove(gameObject.GetComponent<S_Hookable>());
        Destroy(gameObject);
    }
}
