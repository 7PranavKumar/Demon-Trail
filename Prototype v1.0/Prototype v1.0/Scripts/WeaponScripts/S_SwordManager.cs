using UnityEngine;
using System.Collections;

public class S_SwordManager : MonoBehaviour 
{
    //Initializing general variables
    private GameObject sword;
    private Camera playercam;
    private GameObject player;

    //Initializing variables responsible for sword damage
    public bool allowFire = true;
    public float power = 5.0f;
    private float powerSpeed = 1.50f;
    public float maxPower = 30.0f;
    private float rotateFrontSpeed = 7.7f;
    private float originalRotateFrontSpeed;
    private float totalRotateFront = 0.0f;

    //Initializing variables for sword transform
    public Vector3 originalPosition;
    public Vector3 originalRotation;
    private Vector3 currentPosition;
    private Vector3 currentRotation;

    //Initializing variables for camera tilt
    private float currentHeadTilt = 0.0f;
    private float maxHeadTilt = 1.5f;
    private float headTiltSpeed = 1f;

	void Start ()
    {
        playercam = Camera.main;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;

        transform.parent = playercam.transform;
        sword = transform.FindChild("Sword").gameObject;
        originalPosition = sword.transform.localPosition;
        originalRotation = sword.transform.localEulerAngles;
        originalRotateFrontSpeed = rotateFrontSpeed;
	}

	void Update () 
    {
        //If player holds fire1
        if (Input.GetButton("Fire1") && allowFire == true)
        {
            rotateFrontSpeed = originalRotateFrontSpeed;

            //Gathering power
            power = power + powerSpeed * 9 * Time.deltaTime;
            if (power > maxPower)
                power = maxPower;

            //Tilting camera 
            currentHeadTilt += Time.deltaTime * headTiltSpeed;
            currentHeadTilt = Mathf.Clamp(currentHeadTilt, 0f, maxHeadTilt);
            playercam.transform.localEulerAngles = new Vector3(playercam.transform.localEulerAngles.x, playercam.transform.localEulerAngles.y, playercam.transform.localEulerAngles.z - currentHeadTilt);

            //Rotating sword
            totalRotateFront += rotateFrontSpeed * Time.deltaTime * 2.5f;
            totalRotateFront = Mathf.Clamp(totalRotateFront, 0f, 65f);

            sword.transform.localEulerAngles = new Vector3(originalRotation.x + totalRotateFront, sword.transform.localEulerAngles.y, sword.transform.localEulerAngles.z);
            sword.transform.localEulerAngles = new Vector3(sword.transform.localEulerAngles.x, sword.transform.localEulerAngles.y, sword.transform.localEulerAngles.z + power / 1.55f);  
        }

        //If player lets go off fire1
        else
        {
            //Lerping power back to 0, camera tilt = 0
            power = Mathf.Lerp(power, 0, 0.02f);
            currentHeadTilt = 0.0f;

            //Moving the sword back to its original rotation and position
            if (totalRotateFront <= 0.0f)
                rotateFrontSpeed = 0;

            if(totalRotateFront > 0)
            {
                totalRotateFront -= rotateFrontSpeed * Time.deltaTime * 3.0f;
                sword.transform.localEulerAngles = new Vector3(sword.transform.localEulerAngles.x - rotateFrontSpeed * Time.deltaTime * 3.0f, sword.transform.localEulerAngles.y, sword.transform.localEulerAngles.z);
            }
            sword.transform.localEulerAngles = new Vector3(sword.transform.localEulerAngles.x, sword.transform.localEulerAngles.y, sword.transform.localEulerAngles.z + power / 1.55f);
        }   
	}
}
