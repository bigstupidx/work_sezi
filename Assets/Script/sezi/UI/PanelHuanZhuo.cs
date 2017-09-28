using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;

public class PanelHuanZhuo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void onCancelClick()
    {

        SeZiOtherPanelScripts.getMe().setModeTypeText(2);

        GlobalDataScript.getInstance ().sendGoldAutoExitRequest = false;
		GlobalDataScript.getInstance ().chageDesktop = false;
		//SZGoldChangeRoomReuquestVO vo = new SZGoldChangeRoomReuquestVO ();
		//vo.type = 0;
		//string sendMsg = JsonMapper.ToJson(vo);
		CustomSocket.getInstance().sendMsg(new SZChangeRoomRequest(""));
		SoundCtrl.getInstance().playSoundByActionButton(1);
        onExitClick();
    }

    public void onExitClick()
    {
        Destroy(this);
		Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
