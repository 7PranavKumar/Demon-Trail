using UnityEngine;
using System.Collections;

public class S_EnemyProjectile_NightshadeCollision : MonoBehaviour 
{
    public float damage;

    //If it hit player, do damage and slow
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            col.gameObject.GetComponent<S_PlayerHealthSystem>().playerDamage(damage);
            col.gameObject.GetComponent<S_PlayerHealthSystem>().KnockBack(Vector3.up * 1.5f, 3.0f);
            col.gameObject.GetComponent<S_FirstPersonCharacterController>().NightShadeSlow(9f, 0.0f);
        }          
    }
}
