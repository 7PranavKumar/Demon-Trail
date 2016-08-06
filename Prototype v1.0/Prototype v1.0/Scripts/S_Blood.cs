using UnityEngine;
using System.Collections;

public class S_Blood : MonoBehaviour
{
	void Start ()
    {
        GetComponent<ParticleSystem>().Play();
	}
	
	void Update () 
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
            Destroy(gameObject);
	}
}
