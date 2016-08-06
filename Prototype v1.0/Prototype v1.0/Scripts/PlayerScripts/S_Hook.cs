using UnityEngine;
using System.Collections;

public class S_Hook : MonoBehaviour 
{
    private GameObject player;
    private float displacementSpeed = 3.0f;
    private Vector3 originalPos = new Vector3(-0.26f, -0.333f, 0.294f);
    private Vector3 originalRot = new Vector3(15.6061f, 317.5029f, 351.2724f);

    void Start()
    {
        transform.parent = Camera.main.transform;
        transform.localPosition = originalPos;
        transform.localEulerAngles = originalRot;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
      if(player.GetComponent<S_FirstPersonCharacterController>().hookableObjects.Count > 0)
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(originalPos.x + 0.05f, originalPos.y + 0.05f, originalPos.z + 0.05f), displacementSpeed * Time.deltaTime); 
      else
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, displacementSpeed * Time.deltaTime);      
    }
}
