using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using LitJson;
using AssemblyCSharp;

public class SeZiOtherPanelScripts : MonoBehaviour {

	public GameObject chargeButton;
	public GameObject buttonListObj;
    public GameObject destoryButton;
    public Text modeTypeText;

    private static SeZiOtherPanelScripts instance;
    public static SeZiOtherPanelScripts getMe()
    {
        return instance;
    }


    // Use this for initialization
    void Start () {

        instance = this;

        buttonListObj.SetActive (false);

        if (GlobalDataScript.roomVo.isGoldRoom)
        {
            destoryButton.SetActive(false);
        }
        modeTypeText.gameObject.SetActive(false);

		if (GlobalDataScript.hideChargeUI) {
			chargeButton.SetActive (false);
		}
    }

	//2倍底注
	public void onTwoClick() {
        GameObject obj = PrefabManage.loadPerfab("Prefab/sezi/Panel_UseTwoProp");
        obj.GetComponent<SZBuyPropScripts>();

		SoundCtrl.getInstance ().playSoundByActionButton (1);
    }

	//5倍抢开
	public void onFiveClick() {
        GameObject obj = PrefabManage.loadPerfab("Prefab/sezi/Panel_UseFiveProp");
        obj.GetComponent<SZBuyPropScripts>();
		SoundCtrl.getInstance ().playSoundByActionButton (1);
    }

    //自己离开房间，不影响其他人
    public void onExitClick() {
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		if (GlobalDataScript.getInstance().gameStart) {
			if (GlobalDataScript.isGoldQuickStar || GlobalDataScript.roomVo.isGoldRoom) {
				if (!GlobalDataScript.getInstance ().chageDesktop && !GlobalDataScript.getInstance().sendGoldAutoExitRequest) {
					buttonListObj.SetActive (true);
				} else {
					//弹框
					if (GlobalDataScript.getInstance ().sendGoldAutoExitRequest) {
						//发送了自动退出的请求
						loadPerfab ("Prefab/sezi/Panel_CanelQiangtui");
					} else if (GlobalDataScript.getInstance().chageDesktop) {
						//发送了自动换桌的请求
						loadPerfab("Prefab/sezi/Panel_HuanZhuo");
					}
				}
			}
			return;
		}
        OutRoomRequestVo vo = new OutRoomRequestVo();
        //vo.roomId = GlobalDataScript.roomVo.roomId;
        string sendMsg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));
        GlobalDataScript.isonApplayExitRoomstatus = true;

    }

	//离开房间，需要托管到当前局结束
	public void onLeaveRoomClick() {
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		loadPerfab("Prefab/sezi/Panel_Qiangtui");
        buttonListObj.SetActive(false);

    }

	//切换到另外一个房间
	public void onChangeDesk() {
        GlobalDataScript.getInstance ().sendGoldAutoExitRequest = false;
		GlobalDataScript.getInstance ().chageDesktop = true;
		SZGoldChangeRoomReuquestVO vo = new SZGoldChangeRoomReuquestVO ();
		vo.type = 0;
		string sendMsg = JsonMapper.ToJson(vo);
		CustomSocket.getInstance().sendMsg(new SZChangeRoomRequest(sendMsg));
		SoundCtrl.getInstance().playSoundByActionButton(1);
        buttonListObj.SetActive(false);

    }

    //设置当前模式
    public void setModeTypeText(int type)
    {
        modeTypeText.gameObject.SetActive(true);
        if (type == 1)
        {
            modeTypeText.text = "游戏结束自动离开房间";
        }
        else if (type == 2)
        {
            modeTypeText.text = "游戏结束自动切换房间";
        }
        else
        {
            modeTypeText.gameObject.SetActive(false);
        }
    }

    /**
     * 申请或同意解散房间请求
     * 
     */
    private string dissoliveRoomType = "0";
    public void doDissoliveRoomRequest()
    {
        //print ("doDissoliveRoomRequest::::" + avatarList.Count);
        if (GlobalDataScript.roomAvatarVoList == null || GlobalDataScript.roomAvatarVoList.Count <= 1)
        {

            OutRoomRequestVo vo = new OutRoomRequestVo();
            //vo.roomId = GlobalDataScript.roomVo.roomId;
            string sendMsg = JsonMapper.ToJson(vo);
            CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));
            GlobalDataScript.isonApplayExitRoomstatus = true;
        }
        else
        {
            DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo();
            //dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
            //dissoliveRoomRequestVo.type = dissoliveRoomType;
            string sendMsg = JsonMapper.ToJson(dissoliveRoomRequestVo);
            MyDebug.Log("doDissoliveRoomRequest:::" + sendMsg);
            CustomSocket.getInstance().sendMsg(new DissoliveRoomRequest(sendMsg));
            GlobalDataScript.isonApplayExitRoomstatus = true;
        }

		SoundCtrl.getInstance ().playSoundByActionButton (1);
    }

	//设置点击
    public void onSetClick() {
		SoundCtrl.getInstance().playSoundByActionButton(1);
		loadPerfab("Prefab/YueqinPanel/Panel_SheZhi");
	}
	//帮助点击
	public void onHelpClick() {
		SoundCtrl.getInstance().playSoundByActionButton(1);
		loadPerfab("Prefab/YueqinPanel/Panel_Help");
	}

    //点击充值
    public void onChargeClick()
    {
        loadPerfab("Prefab/sezi/PanelCoinGame");
        return;
        SoundCtrl.getInstance().playSoundByActionButton(1);
        if (!GlobalDataScript.roomVo.isGoldRoom)
        {
            loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
        }
        else
        {
            loadPerfab("Prefab/sezi/PanelCoinGame");
        }
        
    }

	//微信邀请
	public void onInviteByWX() {

	}

    public void onVoiceClick() {

	}

	// Update is called once per frame
	void Update () {
	
	}

	private void loadPerfab(string perfabName)
	{
		GameObject alertObj = Instantiate(Resources.Load(perfabName)) as GameObject;
		alertObj.transform.parent = gameObject.transform;
		alertObj.transform.localScale = Vector3.one;
		//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
		alertObj.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		alertObj.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
	}
}
