using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GuiScripts : MonoBehaviour {
    public Toggle baiguiToggle;
    public Toggle isHaveFenToggle;
	
	
	// Update is called once per frame
	void Update () {
        if (baiguiToggle.isOn)
        {
            isHaveFenToggle.isOn = true;
        }
        
	}
}
