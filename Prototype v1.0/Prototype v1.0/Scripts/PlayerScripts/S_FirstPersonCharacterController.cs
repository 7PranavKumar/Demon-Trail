using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_FirstPersonCharacterController : MonoBehaviour
{
    //public variables
    public float moveSpeed = 1.5f;
    public float leftRightLookSpeed = 0.5f;
    public float jumpSpeed = 1.0f;
    public float gravity = 11.75f;
    public float gravityMax = 11.75f;
    public float smoothLeftRight = 2.0f;
    public float smoothJump = 2.0f;
    public float smoothAirControl = 2.0f;
    public float smoothPickup = 0.00002f;
    public float smoothStop = 0.003f;
    public float pickUpTime = 0.5f;
    public float airControl = 1.0f;
    public float headBobMag = 10.0f;
    public GameObject Hook;

    //Private variables
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController character;
    private GameObject player;
    private float forwardMovement;
    private float lastFrameForward;
    private float sideMovement;
    private float lastFrameSide;
    private Camera playercam;
    private float leftRightLook;
    private float jumpMove = 0.0f;

    //For Roll
    private float rollDistance = 20.0f;
    public bool isPlayerRolling = false;
    private Vector3 dirOfRoll;
    private Vector3 normalizedDirOfRoll;
    private float rolledDist;
    private float maxRolledDist = 12.5f;
    private float rollSpeed = 10f;
    private float currentCamRot = 0f;
    private float totalRot = 0f;
    private Vector3 axisRotateAround;
    private Quaternion originalPlayerCamPos;
    private float initialLeftRightLookSpeed;
    public bool allowRolling = true;
    private bool rollOffCoolDown = true;
    private float rollTime;
    private Vector3 moveBy;
    private bool firstHalf;

    //For Hook
    private RaycastHit ray;
    private Vector3 hook;
    private Vector3 pullForce;
    public float maxHookLength = 6.0f;
    private float hookLength;
    private float hookSpeed = 80f;
    private bool isHooking = false;
    private float hookSpeedMultiplier = 5f;
    private float gravityHookMultiplier = 1.0f;

    //Spawning Hooks
    private float maxHooks = 3.0f;                  
    private float currentHooks = 0.0f;
    private float originalHookSpawnForce = 15.0f;
    private float hookSpawnForce = 5.0f;
    public GameObject hookSpawn;
    private GameObject hookObject;
    private bool fireHook;
    private bool hookSort;
    private Vector3 hookFireDirection;
    private Vector3 hookSpawnPosition;
    private bool deployHookCooldown;
    private Ray hookableObjectRay;
    public Material M_Green;
    public Material M_Gray;

    //General initializations
    public List<S_Hookable> hookableObjects = new List<S_Hookable>();
    private GameObject[] hookableObjectsArray = new GameObject[3];
    private S_PlayerHealthSystem playerHealthSystem;
    private int hookArrayTrack = 0;

    // Player death
    public bool isPlayerDead = false;

    //Player slow
    private float recoverRate;
    private float slowRatio;
    private bool recoverFromNightShadeSlow;
    public float maxMoveSpeed;
    private float tempMoveSpeed;
    private bool enemyHit;
    private float spearRecoverRate;
    private bool spearRecover;

    //For Jump and Land Impact
    private Vector3 initialLandPosition;
    private float landImpactOffsetSpeed;
    private bool hasJumped;
    private bool landingImpact;
    private bool jumpOnce;

    //For Checking if Grounded
    public bool isPlayerGrounded;

    void Start()
    {
        initialLeftRightLookSpeed = leftRightLookSpeed;
        character = gameObject.GetComponent<CharacterController>();
        playerHealthSystem = gameObject.GetComponent<S_PlayerHealthSystem>();
        playercam = Camera.main;
        maxMoveSpeed = moveSpeed;
        tempMoveSpeed = moveSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        Instantiate(Hook);
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void FixedUpdate()
    {
        //Check if the player is grounded
        isPlayerGrounded = CheckPlayerGrounded();
        if (isPlayerGrounded == false)
            hasJumped = true;

        if (isPlayerGrounded)
        {
            if (hasJumped == true)
            {
                //Applying landing impact here
                playercam.GetComponent<S_MouseLook>().LandingImpact();
                hasJumped = false;
            }
            jumpMove = 0.0f;
        }
    }

    void Update()
    {
        //Player can trap 'Esc' to exit the level
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Player can tap key 'L' to restart the level
        if(Input.GetKeyDown(KeyCode.L))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        //Can player Hook? If so, start hook!
        if (Input.GetButtonUp("Fire2") && isPlayerRolling == false && hookableObjects.Count != 0 && isPlayerDead == false)
        {    
            hook = (hookableObjects[0].transform.position - playercam.transform.position).normalized;
            pullForce = hook * hookSpeed;
            gravity = gravityMax / gravityHookMultiplier;
            isHooking = true;
        }

        //Is player slowed by enemy? If so, recover movement speed upto the tempMoveSpeed [as he may still be pulling back the spear]
        if(recoverFromNightShadeSlow)
        {
            moveSpeed = Mathf.SmoothStep(moveSpeed, tempMoveSpeed, Time.deltaTime * recoverRate);
            if (Mathf.Approximately(moveSpeed, tempMoveSpeed) || moveSpeed >= tempMoveSpeed)
            {
                moveSpeed = tempMoveSpeed;
                recoverFromNightShadeSlow = false;
                enemyHit = false;
            }
        }

        //Has player thrown the spear? If so, recover movement speed to maximum
        if (spearRecover)
        {
            moveSpeed = Mathf.SmoothStep(moveSpeed, maxMoveSpeed, Time.deltaTime * spearRecoverRate);
            if (Mathf.Approximately(moveSpeed, maxMoveSpeed) || moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
                tempMoveSpeed = moveSpeed;
                spearRecover = false;
            }
        }

        //Calculate movements (keyboard and mouse - left right)
        if (isPlayerDead == false)
        {
            //Player starting from rest, accelerate according to smoothPickUp
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f)
            {
                forwardMovement = Mathf.SmoothStep(0, (Input.GetAxis("Vertical")), smoothPickup) * moveSpeed;
                lastFrameForward = Mathf.SmoothStep(0, (Input.GetAxis("Vertical")), smoothPickup);
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
            {
                sideMovement = Mathf.SmoothStep(0, (Input.GetAxis("Horizontal")), smoothPickup) * moveSpeed;
                lastFrameSide = Mathf.SmoothStep(0, (Input.GetAxis("Horizontal")), smoothPickup);
            }

            //Player moving from motion to rest
            if (Mathf.Abs(Input.GetAxis("Vertical")) == 0.0f)
            {
                forwardMovement = Mathf.Lerp(lastFrameForward, (Input.GetAxis("Vertical")), smoothStop) * moveSpeed;
                if (Mathf.Abs(forwardMovement) < 1.0f)
                {
                    forwardMovement = 0.0f;
                    lastFrameForward = 0.0f;
                }
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0.0f)
            {
                sideMovement = Mathf.Lerp(lastFrameSide, (Input.GetAxis("Horizontal")), smoothStop) * moveSpeed;
                if (Mathf.Abs(sideMovement) < 1.0f)
                {
                    sideMovement = 0.0f;
                    lastFrameSide = 0.0f;
                }
            }

            //Mouse Look - Left Right Calculate
            leftRightLook = Input.GetAxis("Mouse X") * leftRightLookSpeed;
            transform.Rotate(0, Mathf.SmoothStep(0, leftRightLook, 1.0f / smoothLeftRight), 0);
        }
        
        //Can player jump? Does he want to jump? If so, jump!
        if (isPlayerGrounded)
        {
            if (Input.GetButtonDown("Jump") && isPlayerDead == false)
            {
                jumpMove = Mathf.SmoothStep(jumpMove, jumpSpeed, 1.0f / smoothJump);
            }
        }

        //If player not grounded, apply aircontrol restrictions [always apply gravity]
        if(!isPlayerGrounded)
        {
            sideMovement = Mathf.SmoothStep(sideMovement, sideMovement / airControl, 1.0f / smoothAirControl);
            forwardMovement = Mathf.SmoothStep(forwardMovement, forwardMovement / airControl, 1.0f / smoothAirControl);     
        }
        jumpMove -= gravity * Time.deltaTime;

        //Can player roll? If so, roll!
        if (Input.GetButton("Roll") && isPlayerRolling == false && allowRolling == true && rollOffCoolDown && isPlayerDead == false)
        {
            if (isPlayerGrounded)
            {
                rollOffCoolDown = false;
                rollTime = 0.0f;
                PlayerRoll();
            }
        }

        //Rolling 
        if (isPlayerRolling == true)
        {
            //Tracking roll time
            rollTime = rollTime + Time.fixedDeltaTime;

            //Making player god
            playerHealthSystem.isImmune = true;

            //Disabling Fire on all weapons
            if (player.GetComponentInChildren<S_Spear_Manager>() != null)
                player.GetComponentInChildren<S_Spear_Manager>().allowFire = false;

            if (player.GetComponentInChildren<S_SwordManager>() != null)
                player.GetComponentInChildren<S_SwordManager>().allowFire = false;

            //Disabling Mouse
            playercam.GetComponent<S_MouseLook>().DisableMouse();
            leftRightLookSpeed = 0.0f;

            //Moving the player in the calculated direction of movement and rotating the camera with an increasing speed
            moveBy = normalizedDirOfRoll * Time.deltaTime * rollSpeed;
            currentCamRot = Mathf.SmoothStep(200f,500f, 0.38f) * Time.deltaTime;

            character.Move(moveBy);
            rollDistance += moveBy.magnitude;

            //Performing the player rotation movement
            totalRot = totalRot + currentCamRot;

            //Setting rotation back to zero if it goes above
            if (totalRot >= 360f)
            {
                currentCamRot = 0;
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
            }

            player.transform.Rotate(axisRotateAround, currentCamRot, Space.World);

            //Time to stop rolling?
            if (rollDistance >= maxRolledDist)
            {
                //Setting angles back to 0
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);

                //Enabling Mouse
                playercam.GetComponent<S_MouseLook>().EnableMouse();
                leftRightLookSpeed = initialLeftRightLookSpeed;

                //Enabling all weapons
                if (player.GetComponentInChildren<S_Spear_Manager>() != null)
                    player.GetComponentInChildren<S_Spear_Manager>().allowFire = true;

                if (player.GetComponentInChildren<S_SwordManager>() != null)
                    player.GetComponentInChildren<S_SwordManager>().allowFire = true;

                isPlayerRolling = false;

                //Making player human
                playerHealthSystem.isImmune = false;

                //Starting Cooldown
                StartCoroutine("Cooldown");
            }
        }

        //Can player hook? Deploying Hook!
        if(Input.GetKeyDown(KeyCode.F) && isPlayerRolling == false && deployHookCooldown == false && isPlayerDead == false)
        {
            //Gathering values for variables required to perform hook
            hookSpawnForce = originalHookSpawnForce;
            hookObject = Instantiate(hookSpawn);
            hookObject.transform.position = transform.position;
            hookSpawnPosition = transform.position;
            hookFireDirection = playercam.transform.forward;
            fireHook = true;
            deployHookCooldown = true;

            //Always ensuring that there are only maxHooks in the level, if player spawns a maxHook+1 hook, the first hook is removed from the level
            if(currentHooks + 1 > maxHooks)
            {
                hookableObjectsArray[0].GetComponent<S_Hookable>().Destroy();
                for(int i = 1; i < hookableObjectsArray.Length; i++)
                {
                    hookableObjectsArray[i - 1] = hookableObjectsArray[i];
                }
                hookableObjectsArray[hookableObjectsArray.Length - 1] = hookObject;
            }

            else
            {
                currentHooks++;
                hookableObjectsArray[hookArrayTrack] = hookObject;
                hookArrayTrack++;
                hookArrayTrack = (int)Mathf.Clamp(hookArrayTrack, 0, maxHooks - 1);
            }
        }

        //If the player is too far from the hook, he cannot perform the hook action
        if(fireHook == true && (hookObject.transform.position - hookSpawnPosition).magnitude > maxHookLength)
        {
            fireHook = false;
            deployHookCooldown = false;
        }

        //Fire the hook, if the hook gets close to a wall stop it from moving
        if(fireHook == true)
        {
            hookableObjectRay = new Ray(hookObject.transform.position, hookFireDirection);
            if (!Physics.Raycast(hookableObjectRay, 1f))
            {
                hookObject.transform.position = hookObject.transform.position + hookFireDirection * Time.deltaTime * hookSpawnForce * 2.5f;
                hookSpawnForce = hookSpawnForce - Time.deltaTime;
            }
            else
            {
                fireHook = false;
                deployHookCooldown = false;
            }
        }    

        //For Hook
        //Ensuring that the hooks are always sorted based on their distance from the player (ascending order)
        if (hookableObjects.Count > 1)
        {
            hookableObjects.Sort();
            for(int i = 0; i < hookableObjects.Count; i++)
            {
                if (i == 0)
                    hookableObjects[i].transform.GetChild(1).gameObject.SetActive(true);
                else
                    hookableObjects[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        //If the player is hooking, fake force by using character move and lerping vectors
        if (isHooking)
        {
            if (pullForce.magnitude > 1.0f)
            {
                character.Move(pullForce * Time.deltaTime);
                pullForce = Vector3.Slerp(pullForce, Vector3.zero, hookSpeedMultiplier * Time.deltaTime);
            }
            else
            {
                gravity = gravityMax;
                isHooking = false;
            }
        }

        //Final Movement Execution
        if (isPlayerRolling == false && isPlayerDead == false)
        {
            rollDistance = 0.0f;
            moveDirection = new Vector3(sideMovement, jumpMove, forwardMovement);
            moveDirection = transform.TransformDirection(moveDirection);

            //If player is walking diagonally, prevent the increase in speed by using the magic number
            if (Input.GetAxis("Horizontal") != 0f && Input.GetAxis("Vertical") != 0f)
                moveDirection = moveDirection * 0.7071f;

            character.Move(moveDirection * Time.deltaTime);
        }
    }

    //Finding the direction of the player roll based on player's current movement inputs
    void PlayerRoll()
    {
        //Calulating the direction the player wants to roll in
        if (forwardMovement == 0 && sideMovement == 0)
            normalizedDirOfRoll = Vector3.Normalize(player.transform.forward);
        else
            normalizedDirOfRoll = Vector3.Normalize(moveDirection);

        //Finding the perpendicular to the normalizedDirection of Roll using the 2D plane technique to find the axis on which we must rotate the playerobject around
        axisRotateAround = new Vector3(normalizedDirOfRoll.z, 0, -normalizedDirOfRoll.x);
        isPlayerRolling = true;
        totalRot = 0;
    }

    //Slow applied when pulling the spear back
    public void SpearSlow(float slowRate)
    {
        tempMoveSpeed = Mathf.SmoothStep(tempMoveSpeed, maxMoveSpeed * 0.6f, slowRate * Time.deltaTime);

        if(enemyHit == false)
            moveSpeed = tempMoveSpeed;
    }

    //Recovering from slow by spear attack
    public void RecoverSpearSlow(float recoverRate)
    {
        spearRecoverRate = recoverRate;
        spearRecover = true;
    }

    //Slow applied by Nightshade attack
    public void NightShadeSlow(float recoverSpeed, float reduceRatio)
    {
        enemyHit = true;
        moveSpeed = moveSpeed * reduceRatio;
        this.recoverRate = recoverSpeed;
        recoverFromNightShadeSlow = true;
    }

    private bool CheckPlayerGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(0f, -1.15f, 0f), Color.green);
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 1.15f))
            return true;
        else
            return false;
    }

    //Cooldown delay for roll
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2);
        rollOffCoolDown = true;
    }
}
