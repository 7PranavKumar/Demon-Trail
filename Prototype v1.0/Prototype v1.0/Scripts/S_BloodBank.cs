using UnityEngine;
using System.Collections;

public class S_BloodBank : MonoBehaviour 
{
    //Initializing array of blood particles
    public GameObject[] bloodParticles;
    private GameObject blood;
	
    //Spawning blood based on damage, rotation
	public void SpawnBlood(Transform spawnTransform, Vector3 position, Quaternion rotation, float damage)
    {
	    if(damage > 0.0f & damage < 0.2f)
        {
            blood = (GameObject) GameObject.Instantiate(bloodParticles[0], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[1], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[2], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[3], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;
        }

        else if(damage > 0.2f && damage < 0.40f)
        {
            blood = (GameObject)GameObject.Instantiate(bloodParticles[4], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[5], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[6], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[7], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[8], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[9], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;
        }

        else
        {
            blood = (GameObject)GameObject.Instantiate(bloodParticles[11], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[12], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[13], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;

            blood = (GameObject)GameObject.Instantiate(bloodParticles[14], position, rotation);
            blood.transform.parent = spawnTransform;
            blood = null;
        }
	}
}
