using UnityEngine;
using System.Collections;

public class S_Enemy_PushCollision : MonoBehaviour 
{
    //Initializing variables
    private float forceMagnitude;
    
    void Start()
    {
        forceMagnitude = 6.0f;
    }

    //If other collider is from enemy, push him away
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == 10)
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce((col.gameObject.transform.position - transform.position).normalized * forceMagnitude);
        }
    }
}
