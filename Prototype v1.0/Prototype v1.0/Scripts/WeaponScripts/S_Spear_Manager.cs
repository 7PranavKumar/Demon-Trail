using UnityEngine;
using System.Collections;

public class S_Spear_Manager : MonoBehaviour 
{
    //Initializng general varaibles
    private GameObject player;
    public GameObject spear;
    public GameObject spearWeapon;
    private S_WeaponManager weaponManager;
    private Camera playerCam;

    //Initializing spear throw variables
    public float speed = 5.0f;
    public float power = 0.0f;
    public float maxPower = 30.0f;
    public float minPower = 10.0f;
    public float dragBackSpeed = 0.5f;
    private float powerSpeed = 1.50f;
    public bool allowFire = true;

    //Initializing spear pullback variables
    private Vector3 spearPosition;
    private Vector3 spearRotation;
    private Vector3 placementPos = Vector3.zero;
    private Vector3 placementRot = Vector3.zero;
    private Vector3 finalPos = new Vector3(0.0f, 0.2f, 0.0f);
    private Vector3 placement_original;
    private float maxHeadTilt = 4.0f;
    private float headTiltSpeed = 2.25f;
    private float currentHeadTilt = 0.0f;

    //Initializing movement speed slow variables
    private S_FirstPersonCharacterController character;
    private float movementSpeedSlowRate;

    //Initializing HUD variables
    private GameObject HUD;

    void Start()
    {
        playerCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        character = player.GetComponent<S_FirstPersonCharacterController>();

        movementSpeedSlowRate = 4f;

        weaponManager = player.GetComponent<S_WeaponManager>();
        transform.parent = playerCam.transform;

        transform.localPosition = placementPos;
        transform.localEulerAngles = placementRot;

        spear = transform.FindChild("Spear_1").gameObject;
        spearPosition = spear.transform.localPosition;
        spearRotation = spear.transform.localEulerAngles;
        playerCam.GetComponent<S_MouseLook>().newSpear(spear);

        HUD = GameObject.FindGameObjectWithTag("UI");
    }

    void Update()
    {
        //If player lets go of the fire1 button, fire spear based on power value gathered
        if (Input.GetButtonUp("Fire1") && allowFire == true && spear!=null)
        {
            power = Mathf.Clamp(power, minPower, maxPower);
            player.GetComponent<S_FirstPersonCharacterController>().allowRolling = false;
            spear.GetComponent<S_Weapon_Spear>().FireWeapon(power, maxPower, speed);
            spear = null;
            power = 0;
            allowFire = false;
            character.RecoverSpearSlow(movementSpeedSlowRate * 3.0f);
            StartCoroutine("ReEquip");
        }
        
        //If player is holding fire1 button, gather power and pull spear backwards using lerps
        if (Input.GetButton("Fire1") && allowFire == true && spear!=null)
        { 
            player.GetComponent<S_FirstPersonCharacterController>().allowRolling = false;
            power = power + powerSpeed * Time.deltaTime * 12;

            if (power > maxPower)
                power = maxPower;

            //Pulling spear backwards and reducing movement speed
            if (power < maxPower)
            {
                spear.transform.localPosition = new Vector3(Mathf.Lerp(spear.transform.localPosition.x, finalPos.x, Time.deltaTime * dragBackSpeed * 1.5f), spear.transform.localPosition.y, spear.transform.localPosition.z - Time.deltaTime * dragBackSpeed);
                character.SpearSlow(movementSpeedSlowRate);
            }

            //Tilt the player's head as the spear is being pulled back
            currentHeadTilt += Time.deltaTime * headTiltSpeed;
            if(currentHeadTilt > maxHeadTilt)
            currentHeadTilt = maxHeadTilt;

            playerCam.transform.Rotate(0, 0, currentHeadTilt);
        }
    }

    //Re-Equip cooldown period
    IEnumerator ReEquip()
    {
        //Resetting values
        power = 0;
        currentHeadTilt = 0;
        weaponManager.SubtractSpear();
        HUD.gameObject.transform.FindChild("Crosshair_Reload").GetComponent<S_HUD_CrosshairReload>().Reload();
        yield return new WaitForSeconds(3.0f);

        //If still have ammo, requip a new spear
        if (weaponManager.spearCurrentAmmo > 0)
        {
            spear = Instantiate(spearWeapon);
            spear.transform.parent = transform;
            spear.transform.localEulerAngles = spearRotation;
            spear.transform.localPosition = spearPosition;
            allowFire = true;
            playerCam.GetComponent<S_MouseLook>().newSpear(spear);
        }
        
        //If out of ammo, equip the sword
        else
        {
            weaponManager.EquipSword();
        }
    }

    void OnDestroy()
    {
        character.RecoverSpearSlow(movementSpeedSlowRate * 3.0f);
    }
}
