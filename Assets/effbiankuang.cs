using UnityEngine;
using System.Collections;
using DG;
using System.Collections.Generic;
using UnityEngine.UI;
public class effbiankuang : MonoBehaviour {
    public List<Sprite> splist = new List<Sprite>();
    public Image thisimage;
    private float time = 1;

    public SeZiNPScripts npscript;
    private bool playAnimation = false;

	// Use this for initialization
	void Start () {
        
	}

    public void reset(bool value)
    {
        gameObject.SetActive(value);
        playAnimation = value;
        time = 1;
		if (value == false) {
			thisimage.fillAmount = 1;
		}
    }
	
	// Update is called once per frame
	void Update () {

        if (playAnimation == false)
        {
            return;
        }

        thisimage.fillAmount = time;
        time -= Time.deltaTime*0.04f;
        if (thisimage.fillAmount <= 0.25f)
        {
            thisimage.sprite = splist[0];
            if (thisimage.fillAmount <= 0 && time <= 0 && playAnimation)
            {
//                if (npscript && GlobalDataScript.getInstance().gameStart == true)
//                {
//                    npscript.sendReq();
//                }
                playAnimation = false;
            }

        }
       else  if (thisimage.fillAmount >= 0.25f&& thisimage.fillAmount <= 0.5f)
        {
            thisimage.sprite = splist[1];
           
        }
        else if (thisimage.fillAmount >= 0.5f&& thisimage.fillAmount <= 0.75f)
        {
            thisimage.sprite = splist[2];
        }
        else if(thisimage.fillAmount >= 0.75f && thisimage.fillAmount <= 1f)
        {
            thisimage.sprite = splist[3];
        }
      
    }
}
