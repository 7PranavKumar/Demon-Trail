using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_SpearAmmo : MonoBehaviour 
{
    //Initializing
    private S_WeaponManager weaponManager;
    private GameObject player;
    private Text ammoText;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponManager = player.GetComponent<S_WeaponManager>();
        ammoText = GetComponent<Text>();
	}
	
    //Constantly update the ammotext to show the current spear ammo held by the player
	void Update ()
    {
        ammoText.text =  weaponManager.spearCurrentAmmo.ToString() + "/" + weaponManager.spearMaxAmmo.ToString();
	}
}
