using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;

public class PanelQiangtui : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void onAutoExitClick()
    {
        SeZiOtherPanelScripts.getMe().setModeTypeText(1);

        GlobalDataScript.getInstance ().sendGoldAutoExitRequest = true;
		GlobalDataScript.getInstance ().chageDesktop = false;

        OutRoomRequestVo vo = new OutRoomRequestVo();
        //vo.roomId = GlobalDataScript.roomVo.roomId;
        string sendMsg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));
        GlobalDataScript.isonApplayExitRoomstatus = true;

        onCloseClick();
    }

    public void onCloseClick()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        Destroy(this);
        DestroyObject(gameObject);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
