using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;

public class PanelReEnterRoom : MonoBehaviour {

    private int _roomId;
    private int _roomType;

	// Use this for initialization
	void Start () {
	
	}

    public void setReconnectData(int roomid, int roomType)
    {
        _roomId = roomid;
        _roomType = roomType;
    }

    public void onEnterRoom()
    {
        GlobalDataScript.isGoldQuickStar = true;
        SZQuickJoinRoomRequestVO vo = new SZQuickJoinRoomRequestVO();
        vo.roomtype = _roomType;
        string msg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new SZQuickJoinRoomRequest(msg));
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
