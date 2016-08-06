using UnityEngine;
using System.Collections;

public class S_WorldEye_Detection : MonoBehaviour 
{
    //Initializing
    private GameObject[] objects;
    private bool Once = false;

	void Start () 
    {
        //Finding all enemies in the world
        objects = GameObject.FindGameObjectsWithTag("Enemy");
	}

    public void WorldPlayerSpotted()
    {
        if(Once == false)
        {
            Once = true;
            StartCoroutine("DelayBeforeSpot");       
        }
    }

    //Delay before world spotting enemies
    IEnumerator DelayBeforeSpot()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject enemy in objects)
        {
            if (enemy.GetComponent<S_DemonSight>() != null)
                enemy.GetComponent<S_DemonSight>().playerSpotted = true;

            if (enemy.GetComponent<S_MutantSight>() != null)
                enemy.GetComponent<S_MutantSight>().playerSpotted = true;

            if (enemy.GetComponent<S_ParasiteSight>() != null)
                enemy.GetComponent<S_ParasiteSight>().playerSpotted = true;

            if (enemy.GetComponent<S_NightshadeSight>() != null)
                enemy.GetComponent<S_NightshadeSight>().playerSpotted = true;
        }
    }
}
