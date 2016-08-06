using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_Crosshair : MonoBehaviour
{
    //Initializing
    private Image crosshair;
    private Vector3 originalScale;
    private float speed = 0.5f;
    public bool equippedSpear;
    private S_Spear_Manager spearManager;
    private Camera playerCam;


	void Start () 
    {
        playerCam = Camera.main;
        crosshair = GetComponent<Image>();
        originalScale = crosshair.transform.localScale;
	}

	void Update () 
    {
        //Checking if player has a spear equipped, if yes, do crosshair animation, if no, send crosshair back to original size
        spearManager = playerCam.GetComponentInChildren<S_Spear_Manager>();
        if (spearManager != null)
        {
            if (spearManager.spear != null)
                equippedSpear = true;
            else
                equippedSpear = false;
        }
        else
            equippedSpear = false;

        //This portion allows one script to be attached to both parts of the crosshair (inner and outer)
        if (Input.GetButton("Fire1") && crosshair.name.EndsWith("Out") && equippedSpear)
        {
            Vector3 updatedCrosshairScale = crosshair.transform.localScale - Vector3.one * Time.deltaTime * speed;
            updatedCrosshairScale = new Vector3(Mathf.Clamp(updatedCrosshairScale.x, 0.1f, 1.0f), Mathf.Clamp(updatedCrosshairScale.y, 0.1f, 1.0f), Mathf.Clamp(updatedCrosshairScale.z, 0.1f, 1.0f)); 
            crosshair.transform.localScale = updatedCrosshairScale;
        }

        else if (Input.GetButton("Fire1") && crosshair.name.EndsWith("In") && equippedSpear)
        {
            Vector3 updatedCrosshairScale = crosshair.transform.localScale + Vector3.one * Time.deltaTime * speed;
            updatedCrosshairScale = new Vector3(Mathf.Clamp(updatedCrosshairScale.x, 1.0f, 1.50f), Mathf.Clamp(updatedCrosshairScale.y, 1.0f, 1.50f), Mathf.Clamp(updatedCrosshairScale.z, 1.0f, 1.50f)); 
            crosshair.transform.localScale = updatedCrosshairScale;
        }

        else
        {
            crosshair.transform.localScale = Vector3.Lerp(crosshair.transform.localScale, originalScale, Time.deltaTime * speed * 4f);
        }
	}
}
