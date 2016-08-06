using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_Heal : MonoBehaviour 
{
    //Initializing
    private Image healImage;
    private float flashSpeed;
    private float maxAlpha;
    private float alpha;

	void Start () 
    {
        maxAlpha = 0.25f;
        flashSpeed = 11.0f;
        healImage = GetComponent<Image>();
	}

	void Update () 
    {
        healImage.color = new Color(healImage.color.r, healImage.color.g, healImage.color.b, alpha);
        alpha = Mathf.SmoothStep(alpha, 0.0f, Time.deltaTime * flashSpeed);
	}

    public void Healed()
    {
        alpha = maxAlpha;
    }
}
