using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QuanbaoScripts : MonoBehaviour {

    public Toggle zimotoggle;
    public Toggle qianggangtoggle, qianggangtoggle1;

	
	
	// Update is called once per frame
	void Update () {
        if (zimotoggle.isOn)
        {
            qianggangtoggle.gameObject.SetActive(true);
            qianggangtoggle1.gameObject.SetActive(false);
           // qianggangtoggle.isOn = false;
        }
        else
        {          
            qianggangtoggle.gameObject.SetActive(false);
            qianggangtoggle1.gameObject.SetActive(true);
            qianggangtoggle.isOn = false;
        }
	}
}
