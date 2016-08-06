using UnityEngine;
using System.Collections;

public class S_Tutorial : MonoBehaviour 
{
    //Declaring enemy prefabs
    public GameObject mutant;
    public GameObject nightshade;
    public GameObject demon;
    public GameObject parasite;

    //Declaring 
    public GameObject tutorialWorld;
    private GameObject enemy;
    private GameObject player;
    private float enemyCounter;

    //Declaring inGame locations
    public Transform enemySpawnLocation1;
    public Transform enemySpawnLocation2;
    public Transform playerTeleportPosition;
    public Transform healPickUpTransform;
    public GameObject healPickUpPrefab;
    private GameObject healObject;
    private Vector3 playerPosition;

    //Game Eye
    private GameObject eye;

    //Spawn detail information
    public GameObject spawnInformation;
    private bool begin;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        eye = GameObject.FindGameObjectWithTag("WorldEye");
        spawnInformation.SetActive(false);
	}

    void Update()
    {
        if(begin == true)
        {
            if(enemy != null)
                spawnInformation.SetActive(false);
            else
                spawnInformation.SetActive(true);
        }

        //Checking if player has been teleported
        if (Input.GetKeyDown("y") || Input.GetKeyDown("u") || Input.GetKeyDown("i") || Input.GetKeyDown("o"))
        {
            if (tutorialWorld != null)
            {
                player.transform.position = playerTeleportPosition.position;
                player.transform.rotation = playerTeleportPosition.rotation;

                begin = true;
                Destroy(tutorialWorld.gameObject);
            }
        }

        //Spawning DEMON
        if (Input.GetKeyDown("y") && enemy == null)
        {
            if ((player.transform.position - enemySpawnLocation1.position).magnitude < (player.transform.position - enemySpawnLocation2.position).magnitude)
            {
                enemy = (GameObject)Instantiate(demon, enemySpawnLocation2.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();
                
            }
            else
            {
                enemy = (GameObject)Instantiate(demon, enemySpawnLocation1.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();
            }
        }

        //Spawning MUTANT
        if (Input.GetKeyDown("o") && enemy == null)
        {
            if ((player.transform.position - enemySpawnLocation1.position).magnitude < (player.transform.position - enemySpawnLocation2.position).magnitude)
            {
                enemy = (GameObject)Instantiate(mutant, enemySpawnLocation2.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();

            }
            else
            {
                enemy = (GameObject)Instantiate(mutant, enemySpawnLocation1.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();
            }
        }

        //Spawning PARASITE
        if (Input.GetKeyDown("i") && enemy == null)
        {
            if ((player.transform.position - enemySpawnLocation1.position).magnitude < (player.transform.position - enemySpawnLocation2.position).magnitude)
            {
                enemy = (GameObject)Instantiate(parasite, enemySpawnLocation2.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();

            }
            else
            {
                enemy = (GameObject)Instantiate(parasite, enemySpawnLocation1.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();
            }
        }

        //Spawning NIGHTSHADE
        if (Input.GetKeyDown("u") && enemy == null)
        {
            if ((player.transform.position - enemySpawnLocation1.position).magnitude < (player.transform.position - enemySpawnLocation2.position).magnitude)
            {
                enemy = (GameObject)Instantiate(nightshade, enemySpawnLocation2.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();

            }
            else
            {
                enemy = (GameObject)Instantiate(nightshade, enemySpawnLocation1.position, enemySpawnLocation2.rotation);
                if (healObject == null)
                    healObject = (GameObject)Instantiate(healPickUpPrefab, healPickUpTransform.position, healPickUpTransform.rotation);
                eye.GetComponent<S_WorldEye_Detection>().WorldPlayerSpotted();
            }
        }
    }
}
