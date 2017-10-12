using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using LitJson;

public class HomePanelScript : MonoBehaviour
{
    public Image headIconImg;//头像路径
                             //public Image tipHeadIcon;
    public Text noticeText;
    //public Text tipNameText;
    //	public Text tipIdText;
    //public Text tipIpText;
    public Text nickNameText;//昵称
    public Text cardCountText;//房卡剩余数量
    public Text goldCountText;//金币剩余数量
    public Text IpText;

    public Image jiahao;
   

    public Image message_view;

    public Text message;

    public Text contactInfoContent;

    //public GameObject userInfoPanel;
    public GameObject roomCardPanel;
    WWW www;                     //请求
    string filePath;             //保存的文件路径
    Texture2D texture2D;         //下载的图片
    private string headIcon;
    private GameObject panelCreateDialog;//界面上打开的dialog
    private GameObject panelExitDialog;
    /// <summary>
    /// 这个字段是作为消息显示的列表 ，如果要想通过管理后台随时修改通知信息，
    /// 请接收服务器的数据，并重新赋值给这个字段就行了。
    /// </summary>
    private bool startFlag = false;
    public float waiteTime = 1;
    private float TimeNum = 0;
    private int showNum = 0;
    private int i;
    private int a = 0;

    public GameObject panel_left, panel_top, panel_bot, panel_room;
    public GameObject panel_jinbi;
	public GameObject shopObj;

    bool ceshi = true;
    public void ClickAni() 
    {
        Vector3 v3_L = panel_left.transform.position;
        Vector3 v3_T = panel_top.transform.position;
        Vector3 v3_B = panel_bot.transform.position;
        Vector3 v3_R = panel_room.transform.position;
        if (ceshi)
        {
            //panel_left.transform.DOMoveX(v3_L.x - 600, 0.2f);
            //panel_top.transform.DOMoveY(v3_T.y + 300, 0.2f);
            //panel_bot.transform.DOMoveY(v3_B.y - 300, 0.2f);
            panel_room.transform.DOMoveY(v3_R.y - 340, 0.2f);
            panel_jinbi.SetActive(true);
            ceshi = false;
        }
        else
        {
            //panel_left.transform.DOMoveX(v3_L.x + 600, 0.2f);
            //panel_top.transform.DOMoveY(v3_T.y - 300, 0.2f);
            //panel_bot.transform.DOMoveY(v3_B.y + 300, 0.2f);
            panel_room.transform.DOMoveY(v3_R.y + 340, 0.2f);
            panel_jinbi.SetActive(false);
            ceshi = true;
        }
    }
    //200倍的房间
    public void onClickTwoRoom()
    {
        if (GlobalDataScript.loginResponseData.account.gold < 1000)
        {
            loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
            TipsManagerScript.getInstance().setTips("您的金币少于1000，不能进入该房间");
            return;
        }
        createQuickRequest(1);
    }
    //1000倍的房间
    public void onClickOneThoundRoom()
    {
        if (GlobalDataScript.loginResponseData.account.gold < 20000)
        {
            loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
            TipsManagerScript.getInstance().setTips("您的金币少于20000，不能进入该房间");
            return;
        }
        createQuickRequest(2);
    }

    //快速开始
    private void createQuickRequest(int beishu)
    {
		//int roomtype  1--200   2--1000
        GlobalDataScript.isGoldQuickStar = true;
        SZQuickJoinRoomRequestVO vo = new SZQuickJoinRoomRequestVO();
        vo.roomtype = beishu;
        string msg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new SZQuickJoinRoomRequest(msg));
    }

    // Use this for initialization
    void Start()
    {
        initUI();
        GlobalDataScript.isonLoginPage = false;
        checkEnterInRoom();
        addListener();

        
    }


    void setNoticeTextMessage()
    {

        if (GlobalDataScript.noticeMegs != null && GlobalDataScript.noticeMegs.Count != 0)
        {
            noticeText.transform.localPosition = new Vector3(500, noticeText.transform.localPosition.y);
            noticeText.text = GlobalDataScript.noticeMegs[showNum];
            float time = noticeText.text.Length * 0.5f + 422f / 56f;

            Tweener tweener = noticeText.transform.DOLocalMove(
                new Vector3(-noticeText.text.Length * 40, noticeText.transform.localPosition.y), (float)(time/1.6))
                .OnComplete(moveCompleted);
            tweener.SetEase(Ease.Linear);
            //tweener.SetLoops(-1);
        }

    }

    void moveCompleted()
    {
        showNum=+1;
        if (showNum == GlobalDataScript.noticeMegs.Count)
        {
            showNum = 0;
        }
        setNoticeTextMessage();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        { //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
            MyDebug.Log("Input.GetKey(KeyCode.Escape)");
            if (panelCreateDialog != null)
            {
                Destroy(panelCreateDialog);
				panelCreateDialog = null;
                return;
            }
            else if (panelExitDialog == null)
            {
				//panelExitDialog = Instantiate(Resources.Load("Prefab/YueqinPanel/Panel_Exit")) as GameObject;
    //            panelExitDialog.transform.parent = gameObject.transform;
    //            panelExitDialog.transform.localScale = Vector3.one;
    //            panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
    //            panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
    //            panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            }
        }

        //		TimeNum += Time.deltaTime;
        //		if(TimeNum >= waiteTime){
        //			TimeNum = 0;
        //			setNoticeTextMessage ();
        //		}

    }

    //增加服务器返沪数据监听
    public void addListener()
    {
        SocketEventHandle.getInstance().cardChangeNotice += cardChangeNotice;
        SocketEventHandle.getInstance().contactInfoResponse += contactInfoResponse;
        SocketEventHandle.getInstance().goldResponse += goldChangeNotice;

        //	SocketEventHandle.getInstance ().gameBroadcastNotice += gameBroadcastNotice;
        CommonEvent.getInstance().DisplayBroadcast += gameBroadcastNotice;


        SocketEventHandle.getInstance().enterRoomResponse += onCreateRoomCallback;
        //SocketEventHandle.getInstance().QuickJoinRoomCallBack += onJoinRoomCallBack;
		SocketEventHandle.getInstance ().syncPlayerDataResponse += syncPlayerDataResponse;
        SocketEventHandle.getInstance().SE_DOLTime_Response += SE_DOLTime_Response;
        SocketEventHandle.getInstance().SZ_Reconnect_Response += SZ_Reconnect_Response;

        SocketEventHandle.getInstance().szGetPointResponse += pickCard;


		SocketEventHandle.getInstance().whoBetResponse += whoBetResponse;
		SocketEventHandle.getInstance().betResponse += betResponse;
		//SocketEventHandle.getInstance().StartGameNotice += startGame;
		SocketEventHandle.getInstance().DizhuAndDizhu_response += DizhuAndDizhu_response;

        SocketEventHandle.getInstance().SZChargeResponse += SZChargeResponse;
    }

    private void SZChargeResponse(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        string orderInfo = json["olderInfo"].ToString();
        Debug.LogError("chargeResponse:::" + orderInfo);


    }

    private void betResponse(ClientResponse response)
	{
		SocketEventHandle.getInstance().betResponse -= betResponse;
		GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().betResponse(response);
	}

	private void DizhuAndDizhu_response(ClientResponse response)
	{
		SocketEventHandle.getInstance().DizhuAndDizhu_response -= DizhuAndDizhu_response;
		GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().DizhuAndDizhu_response(response);
	}

	private void whoBetResponse(ClientResponse response)
	{
		SocketEventHandle.getInstance().whoBetResponse -= whoBetResponse;
		GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().whoBetResponse(response);
	}

//	private void startGame(ClientResponse response)
//	{
//		SocketEventHandle.getInstance().StartGameNotice -= startGame;
//		GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().startGame(response);
//	}

    private void pickCard(ClientResponse response)
    {
        SocketEventHandle.getInstance().szGetPointResponse -= pickCard;
        GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().pickCard(response);
    }



    public void removeListener()
    {
        SocketEventHandle.getInstance().SZChargeResponse -= SZChargeResponse;
        SocketEventHandle.getInstance().cardChangeNotice -= cardChangeNotice;
        CommonEvent.getInstance().DisplayBroadcast -= gameBroadcastNotice;
    //    SocketEventHandle.getInstance().contactInfoResponse -= contactInfoResponse;
        //	SocketEventHandle.getInstance ().gameBroadcastNotice -= gameBroadcastNotice;
       SocketEventHandle.getInstance().goldResponse -= goldChangeNotice;

       SocketEventHandle.getInstance().enterRoomResponse -= onCreateRoomCallback;
        //SocketEventHandle.getInstance().QuickJoinRoomCallBack -= onJoinRoomCallBack;

		SocketEventHandle.getInstance ().syncPlayerDataResponse -= syncPlayerDataResponse;
        SocketEventHandle.getInstance().SE_DOLTime_Response -= SE_DOLTime_Response;

        SocketEventHandle.getInstance().SZ_Reconnect_Response -= SZ_Reconnect_Response;

        SocketEventHandle.getInstance().szGetPointResponse -= pickCard;
    }



    //房卡变化处理
    private void cardChangeNotice(ClientResponse response)
    {
        //判断是否为金币场 金币场退出 否则返回扣除的金币
        //if (GlobalDataScript.roomJoinResponseData.isGoldRoom)
        //{
           
        //}
        //else
        if (int.Parse(response.message) >= -10)
        {


            int oldCout = int.Parse(cardCountText.text);
            cardCountText.text = response.message;
            GlobalDataScript.loginResponseData.account.roomcard = int.Parse(response.message);
			if (int.Parse(response.message) - oldCout != 0) {
				contactInfoContent.text = "钻石" + (int.Parse(response.message) - oldCout) + "颗";
				roomCardPanel.SetActive(true);
			}
            
        }
        //}
       
    }
    //金币变化处理
    private void goldChangeNotice(ClientResponse response)
    {
       // goldCountText.text = response.message;
        GlobalDataScript.loginResponseData.account.gold = int.Parse(response.message);
        goldCountText.text = response.message;
    }

    private void gameBroadcastNotice()
    {
        showNum = 0;
        if (!startFlag)
        {
            startFlag = true;
            setNoticeTextMessage();
        }
    }


    private void contactInfoResponse(ClientResponse response)
    {
        contactInfoContent.text = response.message;
        roomCardPanel.SetActive(true);
    }
    /***
	 *初始化显示界面 
	 */
    private void initUI()
    {
        if (GlobalDataScript.loginResponseData != null)
        {
            headIcon = GlobalDataScript.loginResponseData.account.headicon;
            string nickName = GlobalDataScript.loginResponseData.account.nickname;
            int roomCardcount = GlobalDataScript.loginResponseData.account.roomcard;
            int goldcount = GlobalDataScript.loginResponseData.account.gold;
            cardCountText.text = roomCardcount + "";
            goldCountText.text = goldcount + "";
            nickNameText.text = nickName;
            IpText.text = "ID:" + GlobalDataScript.loginResponseData.account.uuid;
            GlobalDataScript.loginResponseData.account.roomcard = roomCardcount;
           // GlobalDataScript.loginResponseData.account.gold = goldcount;
            //if (GlobalDataScript.hupaiResponseVo.currentScore != null && GlobalDataScript.hupaiResponseVo.currentScore!="")
            //{
            //    goldCountText.text = GlobalDataScript.hupaiResponseVo.currentScore + goldcount;
            //}
           
        }

		if (GlobalDataScript.hideChargeUI) {
			//隐藏
			panel_jinbi.SetActive(false);
			panel_top.SetActive (false);
			shopObj.SetActive (false);
		}



 


/**#if UNITY_ANDROID
        //显示游客登录按钮  主要用于ios版本
        if (APIS.hide_android.Equals("0" + APIS.version))
        {
            jiahao.gameObject.SetActive(false);
            message_view.gameObject.SetActive(false);
        }

#endif
        **/
        StartCoroutine(LoadImg());
        //	CustomSocket.getInstance ().sendMsg (new GetContactInfoRequest ());

    }

    public void showUserInfoPanel()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        //userInfoPanel.SetActive (true);
        GameObject obj = PrefabManage.loadPerfab("Prefab/userInfo");
        obj.GetComponent<ShowUserInfoScript>().setUIData(GlobalDataScript.loginResponseData);



    }

    /**
	public void closeUserInfoPanel (){
		userInfoPanel.SetActive (false);
	}
*/
    public void showRoomCardPanel()
    {

		SoundCtrl.getInstance().playSoundByActionButton(1);
        CustomSocket.getInstance().sendMsg(new GetContactInfoRequest());

    }

    public void closeRoomCardPanel()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        roomCardPanel.SetActive(false);
    }

    /****
	 * 判断进入房间
	 */
    private void checkEnterInRoom()
    {
        if (GlobalDataScript.roomVo != null && GlobalDataScript.roomVo.roomId != 0)
        {
            //loadPerfab ("Prefab/Panel_GamePlay");
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GamePlay(Clone)");
        }

    }

    public void Button_openWeb()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        
        Application.OpenURL("https://www.baidu.com/");
      
    }


    public GameObject Panel_message;

    /*
  消息显示隐藏
      */
    public void Button_Mess_Open()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        Panel_message = PrefabManage.loadPerfab("Prefab/Panel_message");
    }

    /***
	 * 打开创建房间的对话框
	 * 
	 */
    public void openCreateRoomDialog()
    {

		SoundCtrl.getInstance().playSoundByActionButton(1);
        if (GlobalDataScript.loginResponseData == null || GlobalDataScript.loginResponseData.roomId == 0)
        {
            //原版
             loadPerfab("Prefab/Panel_Create_DialogNew");
           //温州版本
          //  loadPerfab("Prefab/YueqinPanel/Panel_Create_DialogWenZhou");
        }
        else {

            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法创建房间");
        }




        //Application.LoadLevel ("Play_Scene");
    }
    public void Button_Rank()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);      
        loadPerfab("Prefab/YueqinPanel/Panel_Rank");
        //RankType type = new RankType();
        //type.type = 0;
        //string sendmsgstr = JsonMapper.ToJson(type);
        //CustomSocket.getInstance().sendMsg(new RankRequest(sendmsgstr));
       
        //type.type = 1;
        //string sendmsgstr1 = JsonMapper.ToJson(type);
        //CustomSocket.getInstance().sendMsg(new RankRequest(sendmsgstr1));
        

    }


	private void syncPlayerDataResponse(ClientResponse response)
    {
		GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript> ().syncPlayerDataResponse (response);
	}

    private void SE_DOLTime_Response(ClientResponse response)
    {
        GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().SE_DOLTime_Response(response);
    }

    private void SZ_Reconnect_Response(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int roomid = Int32.Parse(json["roomid"].ToString());
        int type = Int32.Parse(json["type"].ToString());
        if (roomid > 0)
        {
            if (type == 1)
            {
                GlobalDataScript.getInstance().reConnect = true;
                onClickTwoRoom();
            }
            else
            {
                //房卡
                GlobalDataScript.getInstance().reConnect = true;
                RoomJoinVo roomJoinVo = new RoomJoinVo();
                roomJoinVo.roomId = roomid;
                string sendMsg = JsonMapper.ToJson(roomJoinVo);
                CustomSocket.getInstance().sendMsg(new JoinRoomRequest(sendMsg));
            }
        }        
    }

    private RoomCreateVo sendVo;//创建房间的信息
    public void onCreateRoomCallback(ClientResponse response)
    {
        //if (watingPanel != null)
        //{
        //    watingPanel.gameObject.SetActive(false);
        //}

        SoundCtrl.getInstance().StopaudioS();

		if (panelCreateDialog != null) {
			EnterRoomScript sc = panelCreateDialog.GetComponent<EnterRoomScript> ();
			if (sc != null) {
				sc.closeDialog ();
			}
		}

        //RoomCreateResponseVo responseVO = JsonMapper.ToObject<RoomCreateResponseVo> (response.message);
        if (sendVo == null)
        {
            sendVo = new RoomCreateVo();
        }
        JsonData json = JsonMapper.ToObject(response.message);
        int roomid = Int32.Parse(json["roomid"].ToString());
        sendVo.roomId = roomid;
        GlobalDataScript.roomVo = sendVo;
        GlobalDataScript.roomVo.isGoldRoom = false;
        GlobalDataScript.loginResponseData.roomId = roomid;
        GlobalDataScript.loginResponseData.isReady = true;
        GlobalDataScript.loginResponseData.main = true;
        GlobalDataScript.loginResponseData.isOnLine = true;
        GlobalDataScript.reEnterRoomData = null;
        GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameShiZi");

        GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().enterRoomResponse(response);

        resetAnimation();

    }
    //把界面的位置恢复
    private void resetAnimation()
    {

    }

    public void onJoinRoomCallBack(ClientResponse response)
    {
        MyDebug.Log(response);
        if (response.status == 1)
        {
            GlobalDataScript.roomJoinResponseData = JsonMapper.ToObject<RoomJoinResponseVo>(response.message);
            GlobalDataScript.roomVo.addWordCard = GlobalDataScript.roomJoinResponseData.addWordCard;
            GlobalDataScript.roomVo.hong = GlobalDataScript.roomJoinResponseData.hong;
            GlobalDataScript.roomVo.ma = GlobalDataScript.roomJoinResponseData.ma;
            GlobalDataScript.roomVo.name = GlobalDataScript.roomJoinResponseData.name;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            GlobalDataScript.roomVo.roomType = GlobalDataScript.roomJoinResponseData.roomType;
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.roomJoinResponseData.sevenDouble;
            GlobalDataScript.roomVo.xiaYu = GlobalDataScript.roomJoinResponseData.xiaYu;
            GlobalDataScript.roomVo.ziMo = GlobalDataScript.roomJoinResponseData.ziMo;
            GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            GlobalDataScript.roomVo.isGoldRoom = GlobalDataScript.roomJoinResponseData.isGoldRoom;
            
            GlobalDataScript.reEnterRoomData = null;

            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_GameShiZi");
            //GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);
            //GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);
        }
        else
        {
           
            TipsManagerScript.getInstance().setTips2("");
            TipsManagerScript.getInstance().setTips(response.message);           
            GlobalDataScript.homePanel.GetComponent<HomePanelScript>().openEnterRoomDialog();
        }
    }
    /***
	 * 打开进入房间的对话框
	 * 
	 */
    public void openEnterRoomDialog()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        if (GlobalDataScript.roomVo == null || GlobalDataScript.roomVo.roomId == 0)
        {
            loadPerfab("Prefab/Panel_Enter_Room");
            
        }
        else {
            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法加入新的房间");
        }
    }

    /**
	 * 打开游戏规则对话框
	 */
    public void openGameRuleDialog()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/YueqinPanel/Panel_Help");
        panelCreateDialog.GetComponent<PanelHelp>().setType(1);
    }

    /**
	 * 打开游戏排行对话框
	 */
    public void openGameRankDialog()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/YueqinPanel/Panel_Rank_Dialog");
       
    }
    /// <summary>
    /// 打开分享对话框
    /// </summary>
    public void openGameFenXiang() 
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/YueqinPanel/Panel_Fenxiang");
    }
    /// <summary>
    /// 打开设置对话框
    /// </summary>
    public void openGameSheZhi() 
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/YueqinPanel/Panel_SheZhi");
    }
    public void openJinbi()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/YueqinPanel/Panel_Coin");
    }
    public void Shouchong(int num) 
    {
        if (num==0)
        {
            GlobalDataScript.shouchongrugou = true;
        }
        else
        {
            GlobalDataScript.shouchongrugou = false;
        }
        loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
    }
    
    public void ZhanjiBtnClick()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        loadPerfab("Prefab/YueqinPanel/Panel_ZhanJi");
      
        CustomSocket.getInstance().sendMsg(new ZhanjiRequest(""));
    }
    public void ShopBtnClick() 
    {
        loadPerfab("Prefab/YueqinPanel/Panel_Shop");
    }
    private void loadPerfab(string perfabName)
    {
        panelCreateDialog = Instantiate(Resources.Load(perfabName)) as GameObject;
        panelCreateDialog.transform.parent = gameObject.transform;
        panelCreateDialog.transform.localScale = Vector3.one;
        //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
    }


    private IEnumerator LoadImg()
    {
        //开始下载图片
        if (headIcon != null && headIcon != "")
        {
            if (FileIO.wwwSpriteImage.ContainsKey(headIcon))
            {
                headIconImg.sprite = FileIO.wwwSpriteImage[headIcon];
                yield break;
            }

            WWW www = new WWW(headIcon);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                texture2D = www.texture;
                byte[] bytes = texture2D.EncodeToPNG();
                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                headIconImg.sprite = tempSp;
                FileIO.wwwSpriteImage.Add(headIcon, tempSp);
            }
            catch (Exception e)
            {

                MyDebug.Log("LoadImg" + e.Message);
            }
        }
    }

   

    public void exitApp()
    {
		SoundCtrl.getInstance().playSoundByActionButton(1);
        if (panelExitDialog == null)
        {
            panelExitDialog = Instantiate(Resources.Load("Prefab/YueqinPanel/Panel_Exit")) as GameObject;
            panelExitDialog.transform.parent = gameObject.transform;
            panelExitDialog.transform.localScale = Vector3.one;
            //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
            panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        }
    }

}
