using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_PlayerHealth : MonoBehaviour 
{
    //Initializing
    private S_PlayerHealthSystem healthManager;
    private GameObject player;
    private Text healthText; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthManager = player.GetComponent<S_PlayerHealthSystem>();
        healthText = GetComponent<Text>();
    }

    //Constantly update the value of health text based on actual player health
    //if health < 25, set color to red - else to green
    void Update()
    {
        healthText.text = ((int)(healthManager.playerHealth)).ToString();

        if (healthManager.playerHealth < 25.0f)
            healthText.color = Color.red;
        else
            healthText.color = Color.green;
    }
}
