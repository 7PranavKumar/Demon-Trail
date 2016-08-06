using UnityEngine;
using System.Collections;

public class S_EnemyProjectile_ParasiteAttack : MonoBehaviour 
{
    //Initializing projectile variables
    private GameObject player;
    private float delayBetweenTicks;
    private float damageRatioPerTick;
    private bool ticked;

	void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        delayBetweenTicks = 0.3f;
        damageRatioPerTick = 0.15f;
	}

    void OnEnable()
    {
        ticked = false;
    }
	
    //Damage the player every Tick hes in the collider
	private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == "Player" && ticked == false)
        {
            player.GetComponent<S_PlayerHealthSystem>().playerDamage(damageRatioPerTick);
            ticked = true;
            StartCoroutine("Tick");
        }
    }

    //Delay between ticks
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(delayBetweenTicks);
        ticked = false;
    }
}
