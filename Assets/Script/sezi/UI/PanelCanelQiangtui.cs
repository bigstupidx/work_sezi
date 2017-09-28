using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;

public class PanelCanelQiangtui : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void onCancelExit(){

        SeZiOtherPanelScripts.getMe().setModeTypeText(3);

		GlobalDataScript.getInstance ().sendGoldAutoExitRequest = false;
		GlobalDataScript.getInstance ().chageDesktop = false;
		
        onCloseClick();
    }

	public void onCloseClick(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		Destroy (this);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
