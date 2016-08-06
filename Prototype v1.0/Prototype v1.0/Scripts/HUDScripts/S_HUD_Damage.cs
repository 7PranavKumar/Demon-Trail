using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_Damage : MonoBehaviour 
{
    //Initializing entities
    private Color flashColor;
    private Image image; //This is a red HUD overlay with 0 alpha

    //Initializing property parameters 
    private float colorChangeSpeed;
    private float alpha;
    private bool changeColor;
    private bool changeColorBack;
    private bool changeColorDeath;
    
	void Start ()
    {
        colorChangeSpeed = 0.8f;
        image = GetComponent<Image>();
        flashColor = image.color;
        alpha = 0.0f;
	}

    //HUDDamage is called by the playerhealth whenever the player takes damage
    public void HUDDamage()
    {
        if(changeColorDeath == false)
            changeColor = true;
    }

    //DeathFlash is called by the playerhealth when the player dies
    public void DeathFlash()
    {
        changeColorDeath = true;
    }

    void Update()
    {
        //If the player is not dead but has taken damage, set the overlay to tint 0.8f/1.0f alpha red immediately
        if(changeColor == true && changeColorDeath == false)
        {
            alpha = 0.8f;
            flashColor = new Color(image.color.r, image.color.g, image.color.b, alpha);
            image.color = flashColor;
            if(image.color.a == 0.8f)
            {  
                changeColor = false;
                changeColorBack = true;
            }
        }

        //Once tinted red, gradually reduce the tint over time
        if(changeColorBack == true && changeColorDeath == false)
        {
            alpha = alpha - Time.deltaTime * colorChangeSpeed;
            alpha = Mathf.Clamp(alpha, 0.0f, 0.8f);
            flashColor = new Color(image.color.r, image.color.g, image.color.b, alpha);
            image.color = flashColor;
            if(image.color.a == 0.0f)
            {
                changeColorBack = false;
            }
        }

        //If the player is dead, set alpha of red tint to 0.7f permanently
        if(changeColorDeath == true)
        {
            alpha = 0.7f;
            flashColor = new Color(image.color.r, image.color.g, image.color.b, alpha);
            image.color = flashColor;
        }
    }
}
