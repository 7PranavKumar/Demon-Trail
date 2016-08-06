using UnityEngine;
using System.Collections;

public class S_PlayerHealthSystem : MonoBehaviour 
{
    //Initializing general variables
    private CharacterController character;
    private S_FirstPersonCharacterController firstPersonController;
    private Transform UI;

    //Initializing knockback variables
    private Vector3 force;
    private float deltaTime;
    private float speedMultipler;
 
    //Initializing playerhealth variables
    public bool isImmune;
    public float playerHealth;
    private bool killPlayerOnce;
    private float playerMaxHealth;

    void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI").gameObject.transform.Find("DamageScreen");
        character = GetComponent<CharacterController>();
        firstPersonController = GetComponent<S_FirstPersonCharacterController>();
        playerMaxHealth = 100.0f;
        playerHealth = playerMaxHealth;
    }

    void Update()
    {
        //Always ensure that the player health is between 0 and 100
        playerHealth = Mathf.Clamp(playerHealth, 0f, 100.0f);

        //Applying knockback if force is applied [not real force, transform movements]
        if (force.magnitude > 0.2)
        {
            character.Move(force * Time.deltaTime);
            force = Vector3.Lerp(force, Vector3.zero, speedMultipler * Time.deltaTime);
        }
    }

    //public knockback function that triggers if player is not immune
    public void KnockBack(Vector3 force, float speedMultipler)
    {
        if (!isImmune)
        {
            this.force = force;
            this.speedMultipler = speedMultipler;
        }
    }

    //public player hurt function that triggers if player is not immune
    public void playerDamage(float damageRatio)
    {
        if (!isImmune)
        {
            //If player cannot survive the damage - perform kill sequences
            if (playerHealth - (playerMaxHealth * damageRatio) <= 0.0f)
            {
                playerHealth = 0f;
                UI.GetComponent<S_HUD_Damage>().DeathFlash();
                KillPlayer();
            }
            
            //If player can survive the damage - perform damage sequences
            if(playerHealth - (playerMaxHealth * damageRatio) > 0.0f)
            {
                playerHealth = playerHealth - (playerMaxHealth * damageRatio);
                UI.GetComponent<S_HUD_Damage>().HUDDamage();
            }
        }
    }

    //Heal the player and return true/false if healed or not 
    public bool PlayerHeal(float healRatio)
    {
        if (playerHealth < playerMaxHealth)
        {
            playerHealth = playerHealth + healRatio * playerMaxHealth;
            playerHealth = Mathf.Clamp(playerHealth, 0, 100);
            return true;
        }
        else
            return false;
    }

    //Killing the player - disabling inputs, removing the camera from the body, adding rididbody to camera, enabling its disabled collider and S_DeadCamera script
    void KillPlayer()
    {
        if (killPlayerOnce == false)
        {
            //Disabling inputs 
            firstPersonController.isPlayerDead = true;

            //Kill the player
            transform.root.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            Camera.main.transform.parent = null;
            Camera.main.gameObject.AddComponent<Rigidbody>();
            Camera.main.gameObject.GetComponent<Rigidbody>().mass = 200.0f;
            Camera.main.gameObject.GetComponent<Collider>().enabled = true;
            Camera.main.gameObject.GetComponent<S_DeadCamera>().enabled = true;

            //Ensure we kill the player only once
            killPlayerOnce = true;
        }
    }
}
