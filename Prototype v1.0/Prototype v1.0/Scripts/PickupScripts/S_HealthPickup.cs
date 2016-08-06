using UnityEngine;
using System.Collections;

public class S_HealthPickup : MonoBehaviour 
{
    //Initializing
    private GameObject player;
    public S_PlayerHealthSystem healthSystem;
    private float healAmountRatio;

    //Initializing parameters responsible for HUD flash
    private GameObject HUDFlash;

	void Start () 
    {
        HUDFlash = GameObject.FindGameObjectWithTag("UI").transform.FindChild("HealScreen").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        healthSystem = player.GetComponent<S_PlayerHealthSystem>();
        healAmountRatio = 0.5f;
	}
	
    //If the player is not full on HP, heal the player and kill self
	void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player && !(healthSystem.playerHealth == 100.0f) && !(healthSystem.playerHealth <= 0f))
        {
            healthSystem.PlayerHeal(healAmountRatio);
            HUDFlash.GetComponent<S_HUD_Heal>().Healed();
            Destroy(gameObject);
        }
    }
}
