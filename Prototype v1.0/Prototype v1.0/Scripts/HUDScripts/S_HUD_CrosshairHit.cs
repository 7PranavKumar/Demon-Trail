using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_CrosshairHit : MonoBehaviour 
{
    //Initializing
    private Image damageIndicator;
    private float alpha;
    private float alphaReductionSpeed;

	void Start () 
    {
        alpha = 0.0f;
        alphaReductionSpeed = 7.0f;
        damageIndicator = GetComponent<Image>();
        damageIndicator.color = new Color(damageIndicator.color.r, damageIndicator.color.g, damageIndicator.color.b, 0.0f);
	}
	
	void Update() 
    {
        damageIndicator.color = new Color(damageIndicator.color.r, damageIndicator.color.g, damageIndicator.color.b, alpha);
        alpha = Mathf.SmoothStep(alpha, 0.0f, Time.deltaTime * alphaReductionSpeed);    
	}

    //Changing crosshair scale and color speed depending on damage taken from the enemy
    public void damagedEnemy(float damage)
    {
        if (damage > 0.0f && damage <= 0.1f)
        {
            alphaReductionSpeed = 7.0f;
            damageIndicator.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        else if (damage > 0.1f && damage <= 0.2f)
        {
            alphaReductionSpeed = 5.0f;
            damageIndicator.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }

        else
        {
            alphaReductionSpeed = 5.0f;
            damageIndicator.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }

        alpha = 1.0f;
    }
}
