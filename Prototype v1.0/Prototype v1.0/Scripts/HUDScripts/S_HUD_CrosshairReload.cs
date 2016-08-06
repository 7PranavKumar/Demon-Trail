using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_HUD_CrosshairReload : MonoBehaviour 
{
    //Initializing
    private Image reloadImage;
    private float time;
    private bool reload;

	void Start ()
    {
        reloadImage = GetComponent<Image>();
        reloadImage.fillAmount = 0.0f;
        time = 0.34f;
	}

    //Changing fillamount based
	void Update () 
    {
	    if(reload == true)
        {
            reloadImage.fillAmount += Time.deltaTime * time;
            if(reloadImage.fillAmount == 1.0f)
            {
                reloadImage.fillAmount = 0.0f;
                reload = false;
            }
        }
	}

    //public function called from spear once reload has started
    public void Reload()
    {
        reload = true;
    }
}
