using UnityEngine;
using System.Collections;

public class S_MouseLook : MonoBehaviour 
{
    //Initializing mouse look variables
    public float upDownLookSpeed = 0.5f;
    public float smoothUpDown = 2.0f;
    private float upDownLook = 0.0f;
    private float totalUpDownRot = 0.0f;
    private bool mouseEnabled = true;

    //Initializing headbob variables
    private Vector3 originalPos;
    private float timer = 0.0f; 
    private float bobWave = 0.0f;
    public float bobSpeed = 0.18f;
    public float bobAmount = 0.2f;

    //Initializing land impact variables
    private bool landImpact;
    private float impactWave = 0.0f;
    private float impactTimer = 0.0f;
    private float impactSpeed = 0.37f;
    private float impactAmount = 0.045f;

    //Initializing general variables
    private S_FirstPersonCharacterController firstPersonCharacter;

    //Spear movement
    private GameObject spear;
    private float totalRot;
    private float totalRotUpDown;

    void Start()
    {
        originalPos = transform.localPosition;
        firstPersonCharacter = transform.root.GetComponent<S_FirstPersonCharacterController>();
    }

	void Update ()
    {
        //If there is a spear, spear delay move with camera
        if(spear != null)
        {
            totalRot += Input.GetAxis("Mouse X") * Time.deltaTime * 5f;
            totalRot = Mathf.SmoothStep(totalRot, 0.0f, 0.23f);
            totalRot = Mathf.Clamp(totalRot, -10f, 10f);
            
            totalRotUpDown += Input.GetAxis("Mouse Y") * Time.deltaTime * 5f;
            totalRotUpDown = Mathf.SmoothStep(totalRotUpDown, 0.0f, 0.09f);
            totalRotUpDown = Mathf.Clamp(totalRotUpDown, -0.5f, 1f);

            spear.transform.localEulerAngles = new Vector3(-totalRotUpDown, -totalRot, 0f);  
        }

        if (mouseEnabled == true && firstPersonCharacter.isPlayerDead == false) 
        {
            //Mouse Look - Up/Down - ensuring that the player cannot look beyond -80/80 degrees rotation on X axis
            upDownLook = -Input.GetAxis("Mouse Y") * upDownLookSpeed;
            totalUpDownRot = Mathf.SmoothStep(totalUpDownRot, totalUpDownRot + upDownLook, 1.0f / smoothUpDown);
            totalUpDownRot = Mathf.Clamp(totalUpDownRot, -80, 80);
            transform.localRotation = Quaternion.Euler(totalUpDownRot, 0, 0);

            if(firstPersonCharacter.isPlayerDead == false && transform.root.GetComponent<S_FirstPersonCharacterController>().isPlayerGrounded)
                HeadBob();

            //Responsible for slight screen bump when landing after a jump
            if(landImpact == true)
            {
                impactTimer = impactTimer + impactSpeed;
                if(impactTimer >= 0.0f  && impactTimer < Mathf.PI * 2)
                {
                    impactWave = -Mathf.Sin(impactTimer);
                    Vector3 newPos = transform.localPosition;
                    newPos.y += impactWave * impactAmount;
                    transform.localPosition = newPos;
                }
                else
                {
                    transform.localPosition = originalPos;
                    impactTimer = 0.0f;
                    landImpact = false;
                }
            }
        }
    }
    
    //Perform headbob when moving and setting it back to original position when not moving
    void HeadBob()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            timer = timer + bobSpeed;
            if (timer > Mathf.PI * 2)
                timer = timer - (Mathf.PI * 2);

            bobWave = Mathf.Sin(timer) * bobAmount/15;
            Vector3 newPos = transform.localPosition;
            newPos.y += bobWave;
            transform.localPosition = newPos;
        }
        else
        {
            transform.localPosition.Set(originalPos.x, Mathf.SmoothStep(transform.localPosition.y, originalPos.y, 0.2f), originalPos.z);
        }
    }

    //Public function called from the player controller when landing
    public void LandingImpact()
    {
        landImpact = true;
    }

    //Enabling and disabling mouse lookupdown functions
    public void EnableMouse()
    {
        mouseEnabled = true;
    }
    
    public void DisableMouse()
    {
        mouseEnabled = false;
    }

    public void newSpear(GameObject spearObject)
    {
        spear = spearObject;
    }
}
