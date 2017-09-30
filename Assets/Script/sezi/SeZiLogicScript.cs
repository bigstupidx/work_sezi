using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;

public class SeZiLogicScript : MonoBehaviour {

	public SeZiNPScripts npscripts;
    public SeZiChatScript chatscripts;

	public GameObject ready_button;
    public GameObject gameStart_button;

	private Boolean isGameStart = true;
	public double lastTime;

    public GameObject roomRoundAndMarkObj;
	public Text roomRoundNumber;
	public Text roomRemark; //房号

    public Image reShockImg;//重新摇色子的图片

    public PlayerItemScript myItem;
    public Transform playerContainer;
	//public ButtonActionScript btnActionScript;
	public List<PlayerItemScript> playerItems;  //个人信息
	public Text LeavedRoundNumText;//剩余局数
	//public int StartRoundNum;
	public List<AvatarVO> avatarList;
	//public Button inviteFriendButton;  //微信邀请好友
	//public Button setButton;            //设置
	//public Button helpButton;           //帮助
	//public Button ExitRoomButton;  //退出房间

	public GameObject dichiAndDizhuObj;//底池和底注object
	public GameObject jiaodianObj;	//底注的叫点信息
	public Image jiaodianObj_point;
	public Text jiaodianObj_num;
	public Text jiaodianObj_gailvinfo;
	private int serverDizhuNum;
	private int serverDiChiNum;

	public GameObject nextGameStartObj;//下次游戏开始的世界
	public GameObject jingcaiObj;//竞猜的倒计时面板
	private GameObject jingcaiPanel;//竞猜的panel
	private int jingcaiTime;
	private double jingcaiGetTime;

    public GameObject goldContainer;//放豆子的容器
    public GameObject goldContainer1;//房豆子的容器1

    public GameObject overResultObj;
    public Image overResultPoint;
    public Text overResultText;

    public Text dichiTxt;	//底池
	public Text dizhuTxt;	//底注

	public Image resultImg;  //输赢
    private int showResultImgTime;
	public Image hiddenFlyImg;
	// public Image tab;

	public Image centerImage;
	public GameObject noticeGameObject;  //广播
	public Text noticeText;  //广播内容赋值
	//public GameObject genZhuang;
	//======================================

	public GameObject quickTimeUI;		//倒计时
	//public GameObject quickSearchUI;	//显示快速查询

    public GameObject glodJiabeiObj;    //2倍底住/5倍抢开
    public GameObject inviteButton;//邀请好友的按钮


    private double flyTimes;

    private float timer = 0;
	/// <summary>
	/// 
	/// </summary>
	public Image dialog_fanhui;
	/// <summary>
	/// 庄家的索引
	/// </summary>
	private int bankerId;


	private int showTimeNumber = 0;
	private int showNoticeNumber = 0;
	private bool timeFlag = false;



	/**是否为抢胡 游戏结束时需置为false**/
	//private bool isQiangHu = false;
    /**更否申请退出房间申请**/
    //private bool canClickButtonFlag = false;

    //private string passType = "";
    private int who_first_bet_playerIndex;
	private int who_bet_playerId;
    private int who_rebet_playerId;
	private int m_maxBei = 5;	//最大的倍数
	private bool isFirstBet = true;//是不是第一次摇动
	private bool isFirstBetResponse = true;//是不是第一次出牌
	private int m_currentBetTimes = 0;//当前出了几次了
	private bool isStart = false;

    private List<LittleGameOverPlayerInfo> littleEndPlayerArr;//小局胜利
    private List<LittleGameOverPlayerInfo> allGameEndPlayerArr;//大局胜利
    private List<int> losePlayerIdsArr;//失败的玩家的数组
    private int little_over_openid;
    private int little_over_bopenid;
    private int little_over_opennum;
    private int little_over_openpoint;
    private bool little_over_game;
    private int little_over_show_index;
    private int little_over_my_win = -1;//-1不处理，1赢，0输
    private double little_over_excu_time;

	private int jc_playerid ;//竞猜者的ID
	private int bjc_playerid;//被竞猜者的ID
	private int jcplayer_index;//我猜者的位置

    private List<Vector2> playerLocationList;

	private float old_y;
    private float new_y;
    private float d_y;

	private GameObject resultPanel;

	private int goldType_Status;//进入金币模式的状态
	private bool _updateCallPointButton;
    private bool isMyTrun;
    private bool isGuanZhan = false;//是不是在观战

    private int game_dichi;
    private int game_dizhu;
    private static bool isClose = false;

    // Use this for initialization
    void Start () {

        isClose = false;

        GlobalDataScript.getInstance().isInGame = true;

        playerLocationList = new List<Vector2>();
        playerLocationList.Add(new Vector2 ( -459 , -12 ));
        playerLocationList.Add(new Vector2(-223, 80 ));
        playerLocationList.Add(new Vector2( 7, 115 ));
        playerLocationList.Add(new Vector2( 253, 80 ));
        playerLocationList.Add(new Vector2( 461, -12 ));

        initUI ();

        if (allGameEndPlayerArr != null)
        {
            allGameEndPlayerArr.Clear();
            allGameEndPlayerArr = null;
        }
        

        addListener();

		GlobalDataScript.isonLoginPage = false;
		if (GlobalDataScript.getInstance().reConnect)
		{
			//GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
			reEnterRoom();
		}
		else
		{
            //markselfReadyGame ();
		}
		GlobalDataScript.reEnterRoomData = null;

        glodJiabeiObj.SetActive(false);
        
        
        if (GlobalDataScript.isGoldQuickStar || GlobalDataScript.roomVo.isGoldRoom)
        {
            flyTimes = GlobalDataScript.getInstance().getTime();
            gameStart_button.SetActive(false);
            roomRoundAndMarkObj.SetActive(false);
            inviteButton.SetActive(false);
        }
        else
        {
            roomRoundAndMarkObj.SetActive(true);
        }
        onReadyClick();

		isAddToStage = true;

		
		if (GlobalDataScript.getInstance ().betNum > 0) {
			dichiAndDizhuObj.SetActive (true);
			gameStart_button.SetActive(false);
			inviteButton.SetActive(false);
			showQKImg (GlobalDataScript.getInstance ().betId);

			jiaodianObj.SetActive (true);
			jiaodianObj_num.text = GlobalDataScript.getInstance ().betNum.ToString ();
			jiaodianObj_point.sprite = Resources.Load ("sizi/size_scene/point/point_" + GlobalDataScript.getInstance ().betPoint, typeof(Sprite)) as Sprite;


			for (int i = 0; i < playerItems.Count; i++) {
				playerItems [i].startGame (false);
				if (playerItems [i].readyImg != null) {
					playerItems [i].readyImg.gameObject.SetActive (false);
				}
			}

		} else {
			dichiAndDizhuObj.SetActive(false);
		}

		GlobalDataScript.getInstance ().betId = GlobalDataScript.getInstance ().betNum = GlobalDataScript.getInstance ().betPoint = 0;

        //TipsManagerScript.getInstance().setTips2("");
    }

    private void initUI() {
        //selectSZBtnList = selectSZButtons.GetComponentsInChildren<Button> ();

        isStart = true;
        playerItems.Add(myItem);

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load("Prefab/sezi/SZPlayerItem"));
            
            obj.transform.SetParent(playerContainer);
            obj.transform.localPosition = playerLocationList[i];
            obj.transform.localScale = Vector3.one;
            PlayerItemScript item = obj.GetComponent<PlayerItemScript>();
            playerItems.Add(item);
        }

        for (int i = 0; i < playerItems.Count; i++) {
            if (i > 0) {
                playerItems[i].gameObject.SetActive(false);
            }
        }
		if (avatarList != null && avatarList.Count > 0) {
			for (int i = 0; i < avatarList.Count; i++) {
				setSeat (avatarList[i]);
			}
		}
        jingcaiObj.SetActive(false);
        jiaodianObj.SetActive(false);
        nextGameStartObj.SetActive(false);
        overResultObj.SetActive(false);
        if (jingcaiGetTime > 0 )
        {
            if (goldType_Status == 0)
            {
                //nextGameStartObj.SetActive(true);
            }else if (goldType_Status == 3)
            {
               // jingcaiObj.SetActive(true);
            }
            else if (goldType_Status == 6)
            {
                jingcaiObj.SetActive(true);
            }
        }

        if (game_dichi > 0)
        {
            dichiAndDizhuObj.SetActive(true);
            dichiTxt.text = game_dichi + "";
            dizhuTxt.text = game_dizhu + "";
        }

    }

    private void showtestani()
    {
        return;
        int playerIndex = 1;
        int flyIndex = 0;
        GameObject needCloneObj = GameObject.Instantiate(hiddenFlyImg.gameObject);
        //底池
        SeZiGlobalData.getMe().flyGoldOrPK(needCloneObj, playerItems[playerIndex].headerIcon.gameObject, gameObject, 2, 6, false, false);

        SeZiGlobalData.getMe().flyGoldOrPK(playerItems[flyIndex].goldImg.gameObject, playerItems[playerIndex].headerIcon.gameObject, playerItems[flyIndex].gameObject, 2, 5, false, false);

    }

    public void addListener()
	{
		SocketEventHandle.getInstance().whoBetResponse += whoBetResponse;
		SocketEventHandle.getInstance().whoReShockResponse += whoReShockResponse;
		SocketEventHandle.getInstance().betResponse += betResponse;

		SocketEventHandle.getInstance ().dissoliveRoomResponse += dissoliveRoomResponse;
		SocketEventHandle.getInstance ().dissoliveRoomResultResponse += dissoliveRoomResultResponse;
		//SocketEventHandle.getInstance ().syncPlayerDataResponse += syncPlayerDataResponse;
		SocketEventHandle.getInstance ().leaveRoomResponse += leaveRoomResponse;

		SocketEventHandle.getInstance().StartGameNotice += startGame;
		SocketEventHandle.getInstance().readyResponse += gameReadyNotice;
		SocketEventHandle.getInstance().szGetPointResponse += pickCard;
		SocketEventHandle.getInstance().szReShockResponse += szReShockResponse;
		SocketEventHandle.getInstance().szGameGuessResponse += szGameGuessResponse;
		SocketEventHandle.getInstance().szGameShowGuessPanelResponse += szGameShowGuessPanelResponse;
		SocketEventHandle.getInstance().szGameResultResponse += szGameResultResponse;
        SocketEventHandle.getInstance().szGameLittleOverResponse += szGameLittleOverResponse;
        SocketEventHandle.getInstance().szGameAllOverResponse += szGameAllOverResponse;

        SocketEventHandle.getInstance().DizhuAndDizhu_response += DizhuAndDizhu_response;

        SocketEventHandle.getInstance().szRoomChatResponse += szRoomChatResponse;

		SocketEventHandle.getInstance().SE_ReadyOnline_Response += szReadyOnline_Response;
		

		SocketEventHandle.getInstance().SZ_ChangeRoom_Response += SZ_ChangeRoom_Response;
		SocketEventHandle.getInstance().SZ_UseProp_Response += SZ_UseProp_Response;

        //SocketEventHandle.getInstance().outRoomCallback += outRoomCallbak;
        //SocketEventHandle.getInstance().gameReadyNotice += gameReadyNotice;
        SocketEventHandle.getInstance().offlineNotice += offlineNotice;
		SocketEventHandle.getInstance().onlineNotice += onlineNotice;
		//CommonEvent.getInstance().readyGame += markselfReadyGame;
		//CommonEvent.getInstance().closeGamePanel += exitOrDissoliveRoom;
		SocketEventHandle.getInstance().micInputNotice += micInputNotice;

        //SocketEventHandle.getInstance().offlineNotice += offlineNotice;
        //SocketEventHandle.getInstance().onlineNotice += onlineNotice;

    }

	//动画结束然后变量叫点按钮
	public void updateCallPointButton(){
		_updateCallPointButton = true;
        if (isMyTrun)
        {
            npscripts.changeToGrayButtonState(false);
        }
		
	}

	//轮到谁出牌
	public void whoBetResponse(ClientResponse response) {

        isGameStart = true;
        //GlobalDataScript.getInstance().gameStart = true;

        if (playerItems == null || playerItems.Count <= 0)
        {
            return;
        }

        print("whoBetResponse::"+ response.message);
        JsonData json = JsonMapper.ToObject(response.message);
		who_bet_playerId = Int32.Parse(json["playerid"].ToString());;

		//处理UI
		int bet_index = getIndex(who_bet_playerId);
        if (isFirstBet)
        {
            who_first_bet_playerIndex = bet_index;
        }
		for (int i = 0; i < playerItems.Count; i++) {
			playerItems [i].startGame (false);
			if (playerItems [i].readyImg != null) {
				playerItems [i].readyImg.gameObject.SetActive (false);
			}
		}
        isMyTrun = false;
        npscripts.changeToGrayButtonState ();
		npscripts.kuang_img.gameObject.SetActive (false);
		inviteButton.SetActive (false);


		if (who_bet_playerId == GlobalDataScript.loginResponseData.account.uuid) {
            //轮到自己,处理UI
            npscripts.turnMyCall();
            isMyTrun = true;
            if (GlobalDataScript.getInstance().reConnect)
            {
                npscripts.changeToGrayButtonState(false);
            }
            else
            {
                if (_updateCallPointButton || !GlobalDataScript.roomVo.isGoldRoom)
                {
                    npscripts.changeToGrayButtonState(false);
                }
            }
            
			playerItems [0].startGame (true);
            //print("whoBetResponse::" + who_bet_playerId + ":::" + GlobalDataScript.loginResponseData.account.uuid);
            //GlobalDataScript.canOpenSZ = false;
        } else {
            //npscripts.kuang_img.gameObject.SetActive (false);
            //npscripts.canShowNumAndPoint = false
            playerItems[bet_index].startGame(true);
            showQKImg(who_bet_playerId);
        }
        isFirstBet = false;
    }

	private void showQKImg(int uuid){
		if (isPutCard) {
			int bet_index = getIndex (uuid);
			playerItems[bet_index].startGame(true);
			playerItems[bet_index].showBetCDAnimation();
			if (!isGuanZhan)//不在观战中处理UI
			{
				//显示倍数抢开
				m_currentBetTimes++;
				int my_index = getMyIndexFromList();
				int show_bei = 0;
				if (GlobalDataScript.roomAvatarVoList.Count > 2)
				{
					m_maxBei = avatarList.Count - 1;
					if (m_currentBetTimes >= avatarList.Count)
					{

					}
					else
					{
						if (!isFirstBet && m_maxBei > 1)
						{
							show_bei = Math.Abs(bet_index - my_index);
						}
					}
				}
				GlobalDataScript.currentBeiShu = show_bei;
				if (show_bei > 1)
				{
					//显示倍数
					npscripts.showQKBeiImg(show_bei, true);
				}
				else
				{
					npscripts.showQKBeiImg(show_bei, false);
				}
			}
		}
	}

	//谁重新摇色子
	public void whoReShockResponse(ClientResponse response) {
        JsonData json = JsonMapper.ToObject(response.message);
        int playerId = Int32.Parse(json["playerid"].ToString());
        print("whoReShockResponse====" + playerId);
        //处理UI
        int bet_index = getIndex(playerId);
        playerItems[bet_index].GetComponent<PlayerItemScript>().playShockSeZiAnimation();
        who_rebet_playerId = playerId;
    }

	private float avg_num1;
	private bool isPutCard = false;
	private bool isAddToStage = false;
	//出牌的结果
	public void betResponse(ClientResponse response) {
		print ("betResponse:::" + response.message);
		isFirstBetResponse = false;
        reShockImg.gameObject.SetActive(false);
        isGameStart = true;
        //GlobalDataScript.getInstance().gameStart = true;
		JsonData json = JsonMapper.ToObject(response.message);
        int put_num = Int32.Parse(json["key"].ToString());
		int put_point = Int32.Parse(json["value"].ToString());
		int uuid = Int32.Parse(json["uuid"].ToString());
		if (!isAddToStage) {
			GlobalDataScript.getInstance ().betNum = put_num;
			GlobalDataScript.getInstance ().betPoint = put_point;
			GlobalDataScript.getInstance ().betId = uuid;
			return;
		}


       
		if (isPutCard == false) {
			isPutCard = true;
			showQKImg (uuid);
		}

        //SeZiGlobalData.getMe().otherPutNum = put_num;
		//SeZiGlobalData.getMe ().otherPutPoint = put_point;
		if (put_point == 1) {
            GlobalDataScript.isCallOne = true;
		}
		npscripts.updateNumPosAndPoint (put_num,put_point);

        if (uuid != GlobalDataScript.loginResponseData.account.uuid)
        {
            GlobalDataScript.canOpenSZ = true;
//            int index = getIndex(uuid);
//            if (index >= GlobalDataScript.roomAvatarVoList.Count - 1)
//            {
//                npscripts.turnMyCall();
//                playerItems[0].startGame(true);
//            }
        }
        else
        {
            GlobalDataScript.canOpenSZ = false;
            npscripts.stopEffect();
        }
        //播放声音
		SoundCtrl.getInstance().playSound(put_num,put_point);

		int index = getIndex(uuid);

		//金币/积分扣除
		if (playerItems [index].getAvatarVo () != null) {
			if (GlobalDataScript.roomVo.isGoldRoom) {
				playerItems [index].getAvatarVo ().account.gold -= serverDizhuNum;
				playerItems [index].updateScore (playerItems [index].getAvatarVo ().account.gold);
			} else {
				playerItems [index].getAvatarVo ().scores -= serverDizhuNum;
				playerItems [index].updateScore (playerItems [index].getAvatarVo ().scores);
			}
		}


		//播放金币动画
        if (index < 3)
        {
			if (GlobalDataScript.roomAvatarVoList.Count == 2) {
				if (uuid != GlobalDataScript.loginResponseData.account.uuid) {
					playerItems [index].playThrowGoldAnimation (goldContainer1);
				} else {
					playerItems [index].playThrowGoldAnimation (goldContainer);
				}
			} else {
				playerItems[index].playThrowGoldAnimation(goldContainer);
			}            
        }
        else
        {
            playerItems[index].playThrowGoldAnimation(goldContainer1);
        }

        //算平均个数     1:计算自己有几个   2:总数-自己个数/人数列表
        int myNums = 0;
		if (!isGuanZhan && GlobalDataScript.myPointsArr != null)//观战模式
        {
            for (int i = 0; i < GlobalDataScript.myPointsArr.Length; i++)
            {
                if (!GlobalDataScript.isCallOne && (GlobalDataScript.myPointsArr[i] == 1))
                {
                    myNums++;
                }
                if ((GlobalDataScript.myPointsArr[i] == put_point))
                {
                    myNums++;
                }
            }
            //人物信息显示除掉自己的，中间显示全部的
            int aa = put_num - myNums;
            int bb = GlobalDataScript.roomAvatarVoList.Count - 1;
            string avg_num1 = String.Format("{0:N1}", (float)aa / bb);
            float cc = (float)put_num / GlobalDataScript.roomAvatarVoList.Count;
            string avg_num2 = String.Format ("{0:N1}", cc);

			jiaodianObj_gailvinfo.text = "每人人均"+avg_num2+"个";
			npscripts.gailv_txt.text = "其余人均" + avg_num1 + "个";
        }
		jiaodianObj.SetActive (true);
		jiaodianObj_num.text = put_num.ToString ();
		jiaodianObj_point.sprite = Resources.Load ("sizi/size_scene/point/point_" + put_point, typeof(Sprite)) as Sprite;
    }

	/*************************断线重连*********************************/
	private void reEnterRoom()
	{
        setRoomRemark();
        avatarList = GlobalDataScript.roomAvatarVoList;

        gameEndHandler();
        
        npscripts.initNumList();
        isGameStart = true;
        
        if (!GlobalDataScript.roomVo.isGoldRoom)
        {
            GlobalDataScript.surplusTimes--;
            roomRoundNumber.text = (GlobalDataScript.totalTimes - GlobalDataScript.surplusTimes) + "/" + GlobalDataScript.totalTimes;//刷新剩余圈数
        }

        if ((GlobalDataScript.roomVo.createrId == GlobalDataScript.loginResponseData.account.uuid) && !GlobalDataScript.roomVo.isGoldRoom)
        {
            gameStart_button.SetActive(true);
        }

        isFirstBetResponse = false;
        GlobalDataScript.isOverByPlayer = false;

        //UpateTimeReStart();

        setAllPlayerReadImgVisbleToFalse();

        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].GetComponent<PlayerItemScript>().itemContainer.SetActive(true);
            playerItems[i].ke_img.gameObject.SetActive(true);
        }
        playerItems[0].showPoint(GlobalDataScript.myPointsArr,-1,true);
    }

	// 断线风位算法
	private int getBankLoactionByBanid(int bankid)
	{
		Boolean isMain;
		int flaguuid = 0;
		for (int i = 0; i < avatarList.Count; i++)
		{
			isMain=avatarList[i].main;
			if (isMain)
			{
				flaguuid = avatarList[i].account.uuid;
				break;
			}

		}
		for (int i = 0; i < playerItems.Count; i++)
		{
			if (flaguuid == playerItems[i].getUuid())
			{
				return i;
			}
		}
		return -1;
	}
	//恢复其他全局数据
	private void recoverOtherGlobalData()
	{
		int selfIndex = getMyIndexFromList();
		GlobalDataScript.loginResponseData.account.roomcard = GlobalDataScript.reEnterRoomData.playerList[selfIndex].account.roomcard;//恢复房卡数据，此时主界面还没有load所以无需操作界面显示

	}

	//准备游戏
	public void onReadyClick() {
		for (int i = 0; i < goldContainer.transform.childCount; i++)
        {
            Destroy(goldContainer.transform.GetChild(i).gameObject);
        }
		for (int i = 0; i < goldContainer1.transform.childCount; i++)
		{
			Destroy(goldContainer1.transform.GetChild(i).gameObject);
		}
		resultImg.gameObject.SetActive(false);
        jiaodianObj.SetActive(false);
		ready_button.SetActive (false);
		CustomSocket.getInstance().sendMsg(new GameReadyRequest());
	}
    
    //开始游戏
    public void onGameStartClick()
    {
        showtestani();
        if (GlobalDataScript.roomAvatarVoList == null || GlobalDataScript.roomAvatarVoList.Count <= 1) {
			TipsManagerScript.getInstance ().setTips("开始游戏至少需要俩个人");
			return;
		}
        gameStart_button.SetActive(false);
        ready_button.SetActive(false);
        CustomSocket.getInstance().sendMsg(new StartGameRequest());
    }

	/// <summary>
	/// 准备游戏
	/// </summary>
	/// <param name="response">Response.</param>
	public void gameReadyNotice(ClientResponse response)
	{
        //===============================================
        JsonData json = JsonMapper.ToObject(response.message);

		for (int i = 0; i < playerItems.Count; i++) {
			if (playerItems[i].getUuid() == Int32.Parse(json["playerid"].ToString())) {
				playerItems[i].setIsReady(true);
				//print ("gameReadyNotice:::" + i);
				break;
			}
		}
	}

    //获取该排放到列表里面的哪个位置
    private int getLocationIndex(int playerid, int srvIndex)
    {
        int myIndex = getMyIndexFromList();
        int seatIndex = 0;
        int avaterIndex = 0;
        if (playerid <= 0 && srvIndex != 0)
        {
            avaterIndex = srvIndex;
        }
        else
        {
            avaterIndex = getIndex(playerid);
        }
        if (avaterIndex < myIndex)
        {
            seatIndex = 6 - (myIndex - avaterIndex);
        }
        else
        {
            seatIndex = avaterIndex - myIndex;
        }
        return seatIndex;
    }

	//进入房间
	public void enterRoomResponse(ClientResponse response) {
        //int creatid创建者id   roomid   curtimes当前第几句   isstart房间状态0没开始  1开始      status房间到那个阶段 
        RoomJoinResponseVo createVO = new RoomJoinResponseVo();
		JsonData json = JsonMapper.ToObject(response.message);
		if (GlobalDataScript.roomVo == null) {
			GlobalDataScript.roomVo = new RoomCreateVo ();
		}
        GlobalDataScript.roomVo.createrId = Int32.Parse(json["creatid"].ToString());
        GlobalDataScript.roomVo.roomId = Int32.Parse(json["roomid"].ToString());
        GlobalDataScript.roomVo.roundNumber = Int32.Parse(json["roundNumber"].ToString());
        GlobalDataScript.roomVo.isStart = Int32.Parse(json["isstart"].ToString()) == 1 ? true : false;//
        int roomType = Int32.Parse(json["roomtype"].ToString());
        GlobalDataScript.roomVo.roomType = roomType;
        int isgoldroom = Int32.Parse(json["isgoldroom"].ToString());
        GlobalDataScript.roomVo.isGoldRoom = isgoldroom == 1 ? true : false;
        GlobalDataScript.surplusTimes = Int32.Parse(json["curtimes"].ToString());
        setRoomRemark();

        game_dichi = Int32.Parse(json["dichi"].ToString());
        game_dizhu = Int32.Parse(json["dizhu"].ToString());
        if (game_dichi > 0)
        {
            dichiAndDizhuObj.SetActive(true);
            dichiTxt.text = game_dichi + "";
            dizhuTxt.text = game_dizhu + "";
        }
	}

	//离开房间成功
	public void leaveRoomResponse(ClientResponse response) {
		JsonData json = JsonMapper.ToObject(response.message);
		int playerid = Int32.Parse (json ["playerid"].ToString ());
		if (playerid == GlobalDataScript.loginResponseData.account.uuid) {
			GlobalDataScript.isonApplayExitRoomstatus = false;
			GlobalDataScript.isOverByPlayer = true;

			//destory
			this.destoryAll();

		} else {
			int index = getIndex (playerid);
            print("leaveRoomResponse:::" + index + "   " + playerid);
			if (playerItems[index] != null) {
				playerItems [index].GetComponent<PlayerItemScript> ().leaveRoom ();
                playerItems[index].gameObject.SetActive(false);
                GlobalDataScript.getInstance().removeAvatorById(playerid);
            }
		}
        
    }
		
	private void destoryAll() {

        isClose = true;

        game_dichi = game_dizhu = 0;
        isGameStart = false;
        if (avatarList != null) {
			avatarList.Clear ();
		}
		if (GlobalDataScript.roomAvatarVoList != null) {
			GlobalDataScript.roomAvatarVoList.Clear ();
		}
		if (resultPanel != null) {
			Destroy (resultPanel);
		}
        GlobalDataScript.getInstance().isInGame = false;
        GlobalDataScript.getInstance().reset();
        GlobalDataScript.loginResponseData.roomId = 0;
		GlobalDataScript.roomVo = null;
		SeZiGlobalData.getMe ().Clear ();
		removeListener ();

        //Destroy (this);
        //Destroy(gameObject);
        exitOrDissoliveRoom();
		SoundCtrl.getInstance ().playBGM (1);

		if (!GlobalDataScript.hideChargeUI) {
			//提示充值
			if (GlobalDataScript.loginResponseData.account.gold < 1000) {
				if (GlobalDataScript.loginResponseData.account.roomcard < 30) {
					PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
				} else {
					PrefabManage.loadPerfab("Prefab/sezi/PanelCoinGame");
				}
			}
		}

        //Destroy(resultPanel);
        //判断有没有大结算
        if (allGameEndPlayerArr != null && allGameEndPlayerArr.Count > 0)
        {
            GameObject obj = PrefabManage.loadPerfab("Prefab/sezi/Panel_SZLGameOver");
            obj.GetComponent<SeZiLittleJSPanelScript>().setData(allGameEndPlayerArr, true);
            //resultPanel.GetComponent<SeZiLittleJSPanelScript>().reAddDataToUI();
        }
    }

	//同步玩家数据/进入房间之类的
	public void syncPlayerDataResponse(ClientResponse response) {
		
		JsonData json = JsonMapper.ToObject(response.message);
		print ("SyncPlayerData:::"+ response.message);
		////string headimg头像    int id   string nickname   scroe   isready   isbank  isshowQiangBtn  index
		AvatarVO vo = new AvatarVO ();
		vo.account.headicon = json ["headimg"].ToString ();
		vo.account.uuid = Int32.Parse (json ["id"].ToString ());
        vo.account.id = Int32.Parse(json["id"].ToString());
        vo.account.nickname =  json ["nickname"].ToString ();
		vo.scores = Int32.Parse (json ["scroe"].ToString ());
		vo.account.isReady = Int32.Parse(json ["isready"].ToString ());
		vo.account.index = Int32.Parse (json ["index"].ToString ());
		vo.account.gold = Int32.Parse (json ["gold"].ToString ());
        vo.isbank = json["isbank"].ToString();
        vo.account.playerStatus = Int32.Parse(json["playerStatus"].ToString());

        //		if(SeZiGlobalData.getMe().createVO.playerList == null) {
        //			SeZiGlobalData.getMe ().createVO.playerList = new List<AvatarVO> ();
        //		}
        //		SeZiGlobalData.getMe ().createVO.addAvatarVO (vo);


        addAvatarVOToList (vo);

		//
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	/// <param name="response">Response.</param>
	public void startGame(ClientResponse response)
	{
        print ("startGame====================");

        inviteButton.SetActive(false);

        GlobalDataScript.getInstance().reConnect = false;
        gameEndHandler();
        overResultObj.SetActive(false);
        ready_button.SetActive(false);

        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].GetComponent<PlayerItemScript>().itemContainer.SetActive(true);
            playerItems[i].GetComponent<PlayerItemScript>().playShockSeZiAnimation();
        }
		npscripts.initNumList ();
		isGameStart = true;
        //GlobalDataScript.getInstance().gameStart = true;
        reShockImg.gameObject.SetActive(true);

        //        GlobalDataScript.roomAvatarVoList = avatarList;
        //GlobalDataScript.surplusTimes -= 1;
        //		StartGameVO sgvo = JsonMapper.ToObject<StartGameVO>(response.message);
        //		bankerId = sgvo.bankerId;
        //		cleanGameplayUI();
        //开始游戏后不显示
        //MyDebug.Log("startGame");
        if (!GlobalDataScript.roomVo.isGoldRoom)
        {
            GlobalDataScript.surplusTimes--;
            roomRoundNumber.text = (GlobalDataScript.totalTimes - GlobalDataScript.surplusTimes) + "/" + GlobalDataScript.totalTimes;//刷新剩余圈数
        }
		
		GlobalDataScript.finalGameEndVo = null;
		//GlobalDataScript.mainUuid = avatarList[bankerId].account.uuid;
		//playerItems[curDirIndex].setbankImgEnable(true); //设置庄家


		//SetDirGameObjectAction();
		isFirstBetResponse = true;
		GlobalDataScript.isOverByPlayer = false;

		//UpateTimeReStart();

		setAllPlayerReadImgVisbleToFalse();

        dizhuTxt.text = "";
        dichiTxt.text = "";

        m_maxBei = 5;
		//m_gameStartTime = GlobalDataScript.getInstance().getTime ();
	}

    //重新摇
    public void szReShockResponse(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        print("szReShockResponse:::" + response.message);
        int playerId = int.Parse(json["uuid"].ToString());
        int index = getIndex(playerId);
        playerItems[index].playShockSeZiAnimation();
    }

    //房卡模式，当前局结束
    public void szGameLittleOverResponse(ClientResponse response)
    {
        GlobalDataScript.getInstance().reConnect = false;

        gameEndHandler();

        if (littleEndPlayerArr != null)
        {
            littleEndPlayerArr.Clear();
            losePlayerIdsArr.Clear();
        }
        else
        {
            littleEndPlayerArr = new List<LittleGameOverPlayerInfo>();
            losePlayerIdsArr = new List<int>();
        }
        JsonData json = JsonMapper.ToObject(response.message);
        print("szGameLittleOverResponse:::" + response.message);
        string over_player = json["gameover"].ToString();
        string over_card = json["gameoverCard"].ToString();
        string[] player_arr = over_player.Split(',');
        string[] card_arr = over_card.Split(',');

        little_over_openid = int.Parse(json["openid"].ToString());
        little_over_bopenid = int.Parse(json["bopenid"].ToString());
        little_over_opennum = int.Parse(json["key"].ToString());
        little_over_openpoint = int.Parse(json["value"].ToString());
        little_over_game = true;
        little_over_show_index = 0;

        int totalNumPoints = 0;

		AvatarVO avater = null;

        for (int i = 0; i < player_arr.Length / 4; i++)
        {
            LittleGameOverPlayerInfo vo = new LittleGameOverPlayerInfo();
            vo.id = int.Parse(player_arr[4 * i]);
            for (int m = 0; m < GlobalDataScript.roomAvatarVoList.Count; m++)
            {
				avater = GlobalDataScript.roomAvatarVoList[m];
                if (avater.account.uuid == vo.id)
                {
                    vo.userName = avater.account.nickname;
                    vo.headIcon = avater.account.headicon;
                    break;
                }
            }
            
            vo.totolscore = int.Parse(player_arr[4*i+1]);
            vo.score = int.Parse(player_arr[4 * i + 2]);

			if (avater != null) {
				if (GlobalDataScript.roomVo.isGoldRoom) {
					avater.account.gold = vo.totolscore;
				} else {
					avater.scores = vo.totolscore;
				}
			}

            vo.lianshen = int.Parse(player_arr[4 * i + 3]);
            vo.playerIndex = getIndex(vo.id);
            littleEndPlayerArr.Add(vo);
            playerItems[vo.playerIndex].updateScore(vo.totolscore);


            if (vo.score < 0)
            {
                losePlayerIdsArr.Add(vo.playerIndex);
            }
            //判断输赢
            if (!isGuanZhan)
            {
                if (vo.id == GlobalDataScript.loginResponseData.account.uuid)
                {
                    little_over_my_win = vo.score > 0 ? 1 : 0;
                }
            }            
            
        }

        for (int i = 0; i < card_arr.Length/6; i++)
        {
            int[] pointArr = new int[] { int.Parse(card_arr[6 * i + 1]), int.Parse(card_arr[6 * i + 2]), int.Parse(card_arr[6 * i + 3]), int.Parse(card_arr[6 * i + 4]), int.Parse(card_arr[6 * i + 5]) };
            for (int j = 0; j < littleEndPlayerArr.Count; j++)
            {
                if (littleEndPlayerArr[j].id == int.Parse(card_arr[6 * i]))
                {
                    littleEndPlayerArr[j].pointArr = pointArr;
                    break;
                }
            }
            //先判断是不是顺子
            List<int> list = new List<int>();
            bool isShunZi = true;
            for (int m = 0; m < pointArr.Length; m++)
            {
                if (list.IndexOf(pointArr[i]) != -1)
                {
                    isShunZi = false;
                }
                list.Add(pointArr[i]);
            }
            //不是顺子才算点数
            if (!isShunZi)
            {
                for (int m = 0; m < pointArr.Length; m++)
                {
                    if (pointArr[m] == little_over_openpoint)
                    {
                        totalNumPoints++;
                    }
                    if (!GlobalDataScript.isCallOne && pointArr[m] == 1)
                    {
                        totalNumPoints++;
                    }
                }
            }
            
        }
		little_over_game = true;

        overResultText.text = "一共"+ totalNumPoints + "个";
        overResultPoint.sprite = Resources.Load("sizi/size_scene/point/point_" + little_over_openpoint, typeof(Sprite)) as Sprite;
        overResultObj.SetActive(true);

        if (!GlobalDataScript.roomVo.isGoldRoom)
        {
            ready_button.SetActive(true);
        }

        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].effect_kuang != null)
            {
                playerItems[i].effect_kuang.reset(false);
            }  
        }

        //        GameObject obj = PrefabManage.loadPerfab("Prefab/sezi/Panel_SZLGameOver");
        //        obj.GetComponent<SeZiLittleJSPanelScript>().setData(littleEndPlayerArr,false);
    }

    private void gameEndHandler()
    {
		isPutCard = false;
        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].startGame(false);
        }

        for (int i = 0; i < goldContainer.transform.childCount; i++)
        {
            Destroy(goldContainer.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < goldContainer1.transform.childCount; i++)
        {
            Destroy(goldContainer1.transform.GetChild(i).gameObject);
        }

        GlobalDataScript.getInstance().reset();
        SeZiGlobalData.getMe().reset();


        GlobalDataScript.getInstance().gameStart = false;
        ready_button.SetActive(false);
        gameStart_button.SetActive(false);
        gameStart_button.SetActive(false);
        //overResultObj.SetActive(false);
        npscripts.gameEnd();
        reShockImg.gameObject.SetActive(false);
		resultImg.gameObject.SetActive(false);
		jiaodianObj.SetActive(false);

		nextGameStartObj.SetActive (false);
		jingcaiObj.SetActive (false);

        jc_playerid = bjc_playerid = 0;
        jcplayer_index = -1;
		_updateCallPointButton = false;
        m_currentBetTimes = 0;
        isFirstBet = false;
        who_bet_playerId = 0;
        who_first_bet_playerIndex = -1;
        

    }

    //同步底池，底住
    public void DizhuAndDizhu_response(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        print("DizhuAndDizhu_response:::" + response.message);
        dichiAndDizhuObj.SetActive(true);
        dizhuTxt.text = "底住：" + json["dizhu"].ToString();
        dichiTxt.text = "底池：" + json["dichi"].ToString();

		serverDizhuNum = int.Parse (json["dizhu"].ToString());
		serverDiChiNum = int.Parse (json["dichi"].ToString());
    }

    //房卡模式，当前轮结束
    public void szGameAllOverResponse(ClientResponse response)
    {

        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].startGame(false);
        }

        if (allGameEndPlayerArr != null)
        {
            allGameEndPlayerArr.Clear();
        }
        else
        {
            allGameEndPlayerArr = new List<LittleGameOverPlayerInfo>();
        }
        JsonData json = JsonMapper.ToObject(response.message);
        print("szGameAllOverResponse:::" + response.message);
        string over_player = json["_gameoverAll"].ToString();
        string[] player_arr = over_player.Split(',');

        for (int i = 0; i < player_arr.Length / 4; i++)
        {
            LittleGameOverPlayerInfo vo = new LittleGameOverPlayerInfo();
            vo.headIcon = player_arr[4 * i];
            vo.userName = player_arr[4 * i + 1];
            vo.score = int.Parse(player_arr[4 * i + 2]);
            vo.totolscore = int.Parse(player_arr[4 * i + 3]);
            allGameEndPlayerArr.Add(vo);           
        }
        allGameEndPlayerArr.Sort(delegate (LittleGameOverPlayerInfo x, LittleGameOverPlayerInfo y)
        {
            return x.totolscore.CompareTo(y.totolscore);
        });

        Invoke("autoDestory", 3);          
    }

    private void autoDestory()
    {
        this.destoryAll();
    }

	//聊天
	public void szRoomChatResponse(ClientResponse response) 
	{
		JsonData json = JsonMapper.ToObject(response.message);
		print("szRoomChatResponse:::" + response.message);
		int playerid = int.Parse(json ["uuid"].ToString());
		int msgType = int.Parse(json ["id"].ToString());
		string msgContent = json ["msg"].ToString ();
		int index = 0;
		if (playerid != GlobalDataScript.loginResponseData.account.uuid) {
			index = getIndex (playerid);
		}
		//叫开牌还有叫点不放到聊天室
		if ((msgType != (int)CHAT_TYPE.CALLPOINT_TYPE) && (msgType != (int)CHAT_TYPE.CALLKAI_TYPE)) {
			if (msgType != 5) {
				for (int i = 0; i < GlobalDataScript.roomAvatarVoList.Count; i++)
				{
					if (GlobalDataScript.roomAvatarVoList[i].account.id== playerid || GlobalDataScript.roomAvatarVoList[i].account.uuid == playerid)
					{
						chatscripts.addChatMsg(msgType, msgContent, GlobalDataScript.roomAvatarVoList[i].account.nickname);
						break;
					}
				}
			}
		}
        
        if (index == 0)
        {
            npscripts.showChatMsg(msgType, msgContent);
            return;
        }
		playerItems [index].showChatMessage (msgType,msgContent);
	}

	////在线准备
	public void szReadyOnline_Response(ClientResponse response) 
	{
        GlobalDataScript.getInstance().reConnect = false;
        isGuanZhan = false;
        JsonData json = JsonMapper.ToObject(response.message);
		print("szReadyOnline_Responses:::" + response.message);
		int uuid = int.Parse(json ["uuid"].ToString());
        int statuss = int.Parse(json["statuss"].ToString());
        isGuanZhan = false;
        for (int i = 0; i < GlobalDataScript.roomAvatarVoList.Count; i++)
        {
            GlobalDataScript.roomAvatarVoList[i].account.playerStatus = 1;
            playerItems[i].showGuanZhanImg(false);
        }
        ready_button.SetActive (false);
		gameStart_button.SetActive (false);
		gameEndHandler ();

	}

	//这个消息比较重要。我会同步什么状态。多少倒计时
	public void SE_DOLTime_Response(ClientResponse response) 
	{
        //GlobalDataScript.getInstance().gameStart = true;
		JsonData json = JsonMapper.ToObject(response.message);
		print("SE_DOLTime_Response:::" + response.message);
		jingcaiTime = int.Parse(json ["times"].ToString());
        nextGameStartObj.GetComponentInChildren<Text>().text = jingcaiTime + "秒后将开始游戏";
        goldType_Status = int.Parse(json ["statuss"].ToString());
		jingcaiGetTime = GlobalDataScript.getInstance ().getTime ();
		int playerId = int.Parse(json ["uuid"].ToString());
        //playerId = (playerId == -1) ? GlobalDataScript.loginResponseData.account.uuid : playerId;
        
        if (goldType_Status == 0 )
        {
			//0:游戏准备		
			//nextGameStartObj.SetActive(true);
			if (goldType_Status == 3) {
				
			}
        }
        else if (goldType_Status == 1) {
            //游戏刚刚开始，即摇色子状态，无需关系time这个字段
        }
        else if (goldType_Status == 3)
        {
            //3结算到下一局开始的标志
        }
        else if (goldType_Status == 6)
        {
            jingcaiObj.SetActive(true);
        }
    }

	//切换房间成功
	public void SZ_ChangeRoom_Response(ClientResponse response)
	{
		print("SZ_ChangeRoom_Response====>" + response.message);
		GlobalDataScript.getInstance ().chageDesktop = true;
	}

	//使用道具成功
	public void SZ_UseProp_Response(ClientResponse response)
	{
		print("SZ_UseProp_Response====>" + response.message);
	}


    //出来结果
    public void szGameResultResponse(ClientResponse response)
    {
        print("szGameResultResponse====>" + response.message);
    }

    //开始竞猜
	public void szGameShowGuessPanelResponse(ClientResponse response)
    {
		JsonData json = JsonMapper.ToObject(response.message);
		print("szGameShowGuessPanelResponse:::" + response.message);
		jc_playerid = int.Parse(json ["openId"].ToString());
		bjc_playerid = int.Parse(json ["bopenid"].ToString());
		int jc_num = int.Parse(json ["point"].ToString());
        int total_money = int.Parse(json["gold"].ToString());
        if (jc_playerid == GlobalDataScript.loginResponseData.account.uuid || jc_playerid == GlobalDataScript.loginResponseData.account.uuid) {
			if (GlobalDataScript.roomVo.isGoldRoom == true) {
				jingcaiTime = 10;
			}
			jingcaiObj.SetActive (true);
			return;
		}
		//播放音效
		int jc_index = getIndex (jc_playerid);
		int bjc_index = getIndex (bjc_playerid);
		if (Math.Abs (jc_index - bjc_index) >= 2) {
			SoundCtrl.getInstance ().playSoundByAction ("qiangkai");
		} else {
			SoundCtrl.getInstance ().playSoundByAction ("kai");
		}

		jingcaiPanel = PrefabManage.loadPerfab("Prefab/sezi/Panel_SZ_Guess");
		jingcaiPanel.GetComponent<SeZiGuessPanelScripts> ().setJingCaiNum(jc_num,(int)avg_num1,total_money);
    }

	//竞猜的数据
	private void szGameGuessResponse(ClientResponse response)
	{

		for (int i = 0; i < playerItems.Count; i++) {
			if (playerItems [i].effect_kuang != null) {
				playerItems [i].effect_kuang.reset (false);
			}
		}
		npscripts.stopEffect ();

		JsonData json = JsonMapper.ToObject(response.message);
		print("szGameGuessResponse:::" + response.message);
		int key = Int32.Parse (json ["key"].ToString ());
		int uuid = Int32.Parse (json ["uuid"].ToString ());
        if (jingcaiPanel != null)
        {
            jingcaiPanel.GetComponent<SeZiGuessPanelScripts>().addJingCaiNum(key, 0);
        }
		

		if (jc_playerid > 0 && bjc_playerid > 0) {
			int jc_index = getIndex (jc_playerid);
			int bjc_index = getIndex (bjc_playerid);
			jcplayer_index = getIndex (uuid);

            GameObject targetObj = null;
            if (key == 1)
            {
                //支持被竞猜者
                targetObj = playerItems[bjc_index].zhichi_img.gameObject;
                playerItems[bjc_index].showdingNum();
            }
            else
            {
                //支持竞猜者
                targetObj = playerItems[jc_index].zhichi_img.gameObject;
                playerItems[bjc_index].showdingNum();
            }
            //playerItems[jcplayer_index].zhichi_img.gameObject.SetActive(true);

            SeZiGlobalData.getMe().flyDingImg(playerItems[jcplayer_index].zhichi_img.gameObject, targetObj, playerItems[jcplayer_index].gameObject);
		}
	}

    //开始发牌
    public void pickCard(ClientResponse response) {
		JsonData json = JsonMapper.ToObject(response.message);
		print ("pickCard:::"+ response.message);
		int playerId = int.Parse(json ["uuid"].ToString ());

        //if (who_rebet_playerId != 0 || who_rebet_playerId != playerId)
        //{
        //    return;
        //}

		int pai1 = int.Parse(json ["pai1"].ToString ());
		int pai2 = int.Parse(json ["pai2"].ToString ());
		int pai3 = int.Parse(json ["pai3"].ToString ());
		int pai4 = int.Parse(json ["pai4"].ToString ());
		int pai5 = int.Parse(json ["pai5"].ToString ());
        if (pai1 == 0 || pai2 == 0 || pai3 == 0 || pai4 == 0 || pai5 == 0)
        {
            return;
        }

		GlobalDataScript.getInstance().gameStart = true;

        int[] arr = new int[] { pai1, pai2, pai3, pai4, pai5 };
        if (playerId == GlobalDataScript.loginResponseData.account.uuid || playerId == GlobalDataScript.loginResponseData.account.id)
        {
            gameStart_button.SetActive(false);

            GlobalDataScript.myPointsArr = arr;
            if (playerItems.Count > 0)
            {
                playerItems[0].showPoint(GlobalDataScript.myPointsArr);
            }            
        }
        else
        {
            int index = getIndex(playerId);
            playerItems[index].showPoint(arr);
        }
       
		//playerItems [0].maskImg.transform.gameObject.SetActive (false);
	}

	//玩家进入房间
//	public void otherUserJointRoom(ClientResponse response)
//	{
//		AvatarVO avatar = JsonMapper.ToObject<AvatarVO>(response.message);
//		addAvatarVOToList(avatar);
//
//	}

	public void exitOrDissoliveRoom()
	{
		GlobalDataScript.loginResponseData.resetData();//复位房间数据
		GlobalDataScript.loginResponseData.roomId = 0;//复位房间数据
        if (GlobalDataScript.roomVo != null)
        {
            GlobalDataScript.roomVo.roomId = 0;
        }
		
		GlobalDataScript.soundToggle = true;
		clean();
		removeListener();

		SoundCtrl.getInstance().playBGM(0);
		SoundCtrl.getInstance().playBGM(1);
		if (GlobalDataScript.homePanel != null)
		{
			GlobalDataScript.homePanel.SetActive(true);
			GlobalDataScript.homePanel.transform.SetSiblingIndex(1);
		}
		else
		{

			GlobalDataScript.homePanel = PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_HomeNew");
			GlobalDataScript.homePanel.transform.SetSiblingIndex(1);
		}

		while (playerItems.Count > 0)
		{
			PlayerItemScript item = playerItems[0];
			playerItems.RemoveAt(0);
			item.clean();
			Destroy(item.gameObject);
			Destroy(item);
		}
		Destroy(this);
		Destroy(gameObject);
	}

	/***
     * 申请解散房间回调
     */
	GameObject dissoDialog;
	public void dissoliveRoomResponse(ClientResponse response)
	{
		MyDebug.Log("dissoliveRoomResponse" + response.message);

		GlobalDataScript.isonApplayExitRoomstatus = true;
		//dissoliveRoomType = "1";
        if (dissoDialog==null)
        {
            dissoDialog = PrefabManage.loadPerfab("Prefab/Panel_Apply_Exit");
            dissoDialog.GetComponent<VoteScript>().iniUI("", avatarList);
        }
		
		
	}

	public void dissoliveRoomResultResponse(ClientResponse response)
	{
		GlobalDataScript.isonApplayExitRoomstatus = false;
		GlobalDataScript.isOverByPlayer = true;
		if (dissoDialog != null) {
			dissoDialog.GetComponent<VoteScript>().removeListener();
			Destroy(dissoDialog.GetComponent<VoteScript>());
			Destroy(dissoDialog);
		}
		//print ("dissoliveRoomResultResponsedissoliveRoomResultResponsedissoliveRoomResultResponsedissoliveRoomResultResponsedissoliveRoomResultResponse");
		this.destoryAll();
	}

	/**用户上线提醒**/
	public void onlineNotice(ClientResponse response)
	{
        int uuid = int.Parse(response.message);
        int index = getIndex(uuid);
        playerItems[index].GetComponent<PlayerItemScript>().setPlayerOnline();
    }

    /**用户离线回调**/
    public void offlineNotice(ClientResponse response)
    {
        int uuid = int.Parse(response.message);
        int index = getIndex(uuid);
        //string dirstr = getDirection(index);
        playerItems[index].GetComponent<PlayerItemScript>().setPlayerOffline();
    }

	public void micInputNotice(ClientResponse response)
	{
		int sendUUid = int.Parse(response.message);
		if (sendUUid > 0)
		{
			for (int i = 0; i < playerItems.Count; i++)
			{
				if (playerItems[i].getUuid() != -1)
				{
					if (sendUUid == playerItems[i].getUuid())
					{
						playerItems[i].showChatAction();
					}
				}
			}
		}
	}


	public void returnGameResponse(ClientResponse response)
	{
		

	}

	public void myselfSoundActionPlay()
	{
		playerItems[0].showChatAction();
	}

	private void addAvatarVOToList(AvatarVO avatar)
	{
		if (avatarList == null)
		{
			avatarList = new List<AvatarVO>();
		}
		avatar.isOnLine = true;

        for (int i = 0; i < avatarList.Count; i++)
        {
            if (avatarList[i].account.uuid == avatar.account.uuid)
            {
                avatarList.Remove(avatarList[i]);
                avatarList.Insert(i, avatar);

                GlobalDataScript.roomAvatarVoList.Remove(avatarList[i]);
                GlobalDataScript.roomAvatarVoList.Insert(i, avatar);

                updateAvatorData(avatar);
                return;
            }
        }
        
		avatarList.Add(avatar);
		setSeat(avatar);
		if (GlobalDataScript.roomAvatarVoList == null) {
			GlobalDataScript.roomAvatarVoList = new List<AvatarVO> ();
		}
		GlobalDataScript.roomAvatarVoList.Add (avatar);

	}

    private void updateAvatorData(AvatarVO vo)
    {
        bool flag = false;
		//更新UI
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].getUuid() == vo.account.uuid)
            {
                flag = true;
                playerItems[i].setAvatarVo(vo);
                break;
            }
        }
        if (!flag)
        {
            setSeat(vo);
        }
    }

	public void setRoomRemark()
	{
		RoomCreateVo roomvo = GlobalDataScript.roomVo;
		RoomJoinResponseVo joinvo = GlobalDataScript.roomJoinResponseData;
		//bool creatorjoin = true;
		GlobalDataScript.totalTimes = roomvo.roundNumber;
        if (GlobalDataScript.surplusTimes == 1)
        {
            GlobalDataScript.surplusTimes = roomvo.roundNumber;
        }
		
        if (!GlobalDataScript.roomVo.isGoldRoom)
        {
            roomRemark.text = roomvo.roomId + "";
            int leftTimes = (roomvo.roundNumber - GlobalDataScript.surplusTimes);
            roomRoundNumber.text = (leftTimes == 0 ? 1 : leftTimes) + "/" + roomvo.roundNumber;
        }
        else
        {
            roomRoundAndMarkObj.SetActive(false);
        }
		
	}

	/// <summary>
	/// 设置当前角色的座位
	/// </summary>
	/// <param name="avatar">Avatar.</param>
	/// 
	private void setSeat(AvatarVO avatar)
	{
		//游戏结束后用的数据，勿删！！！

		//GlobalDataScript.palyerBaseInfo.Add (avatar.account.uuid, avatar.account);

		if (isStart == false) {
			return;
		}
        ready_button.SetActive(false);
		gameStart_button.SetActive (false);
		if ((GlobalDataScript.roomVo.createrId == GlobalDataScript.loginResponseData.account.uuid) && !GlobalDataScript.roomVo.isGoldRoom) {
            gameStart_button.SetActive (true);
		}

		if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid)
		{
            //print ("setSeat myIndex=" + avatar.account.index);
            playerItems[0].setAvatarVo(avatar);
            playerItems[0].gameObject.SetActive(true);
            playerItems[0].showGuanZhanImg(false);
            isGuanZhan = false;
            if (GlobalDataScript.roomVo.isGoldRoom)
            {
                if (avatar.account.playerStatus == 0)//旁观
                {
                    isGuanZhan = true;
                    playerItems[0].showGuanZhanImg(true);
                    little_over_my_win = -1;
                }
            }
            else
            {
                if (avatar.account.isReady == 1)
                {
                    playerItems[0].readyImg.enabled = true;
                    playerItems[0].readyImg.transform.gameObject.SetActive(true);
                }

            }
		}
		else
		{
            int seatIndex = 0;
            if (avatar.account.index == 0)
            {
                seatIndex = getLocationIndex(avatar.account.uuid, avatar.account.index);
            }
            else
            {
                seatIndex = getLocationIndex(0, avatar.account.index);
            }
			
			playerItems [seatIndex].gameObject.SetActive (true);
			playerItems[seatIndex].setAvatarVo(avatar);
            
            playerItems[seatIndex].showGuanZhanImg(false);
            if (avatar.account.playerStatus == 0)//旁观
            {
                playerItems[seatIndex].showGuanZhanImg(true);
            }
            else
            {
                if (avatar.account.isReady == 1)
                {
                    playerItems[seatIndex].readyImg.transform.gameObject.SetActive(true);
                    playerItems[seatIndex].readyImg.enabled = true;
                }
            }
            //print ("setSeat:::" + seatIndex);
        }
    }

	/// <summary>
	/// Gets my index from list.
	/// </summary>
	/// <returns>The my index from list.</returns>
	private int getMyIndexFromList()
	{
		if (avatarList != null)
		{
			for (int i = 0; i < avatarList.Count; i++)
			{
				if (avatarList[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid || avatarList[i].account.openid == GlobalDataScript.loginResponseData.account.openid)
				{
					GlobalDataScript.loginResponseData.account.uuid = avatarList[i].account.uuid;
					MyDebug.Log("数据正常返回" + i);
					return avatarList[i].account.index;
				}
			}
		}

		MyDebug.Log("数据异常返回0");
		return 0;
	}

	private void setAllPlayerReadImgVisbleToFalse()
	{
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems [i].maskImg.transform.gameObject.SetActive (true);
			playerItems[i].readyImg.enabled = false;
		}
	}

	/// 重新开始计时
	/// </summary>
	void UpateTimeReStart()
	{
		timer = 16;      
	}

	void initPanel()
	{
		clean();
		//btnActionScript.cleanBtnShow();
		//masContaner.SetActive (false);
	}

	/// <summary>
	/// 清理桌面
	/// </summary>
	public void clean()
	{

	}

	private void cleanArrayList(List<List<GameObject>> list)
	{
		if (list != null)
		{
			while (list.Count > 0)
			{
				List<GameObject> tempList = list[0];
				list.RemoveAt(0);
				cleanList(tempList);
			}
		}
	}

	private void cleanList(List<GameObject> tempList)
	{
		if (tempList != null)
		{
			while (tempList.Count > 0)
			{
				GameObject temp = tempList[0];
				tempList.RemoveAt(0);
				GameObject.Destroy(temp);
			}
		}
	}

	private int getIndex(int uuid)
	{
        if (playerItems != null && playerItems.Count > 0)
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                AvatarVO vo = playerItems[i].GetComponent<PlayerItemScript>().getAvatarVo();
                if (vo != null && vo.account != null && vo.account.uuid == uuid)
                {
                    return i;
                }
            }
        }
		return 0;
	}

	private void removeListener()
	{
        SocketEventHandle.getInstance().whoBetResponse -= whoBetResponse;
        SocketEventHandle.getInstance().whoReShockResponse -= whoReShockResponse;
        SocketEventHandle.getInstance().betResponse -= betResponse;

        SocketEventHandle.getInstance().dissoliveRoomResponse -= dissoliveRoomResponse;
        SocketEventHandle.getInstance().dissoliveRoomResultResponse -= dissoliveRoomResultResponse;
        //SocketEventHandle.getInstance ().syncPlayerDataResponse += syncPlayerDataResponse;
        SocketEventHandle.getInstance().leaveRoomResponse -= leaveRoomResponse;

        SocketEventHandle.getInstance().StartGameNotice -= startGame;
        SocketEventHandle.getInstance().readyResponse -= gameReadyNotice;
        SocketEventHandle.getInstance().szGetPointResponse -= pickCard;
        SocketEventHandle.getInstance().szReShockResponse -= szReShockResponse;
        SocketEventHandle.getInstance().szGameGuessResponse -= szGameGuessResponse;
        SocketEventHandle.getInstance().szGameShowGuessPanelResponse -= szGameShowGuessPanelResponse;
        SocketEventHandle.getInstance().szGameResultResponse -= szGameResultResponse;
        SocketEventHandle.getInstance().szGameLittleOverResponse -= szGameLittleOverResponse;
        SocketEventHandle.getInstance().szGameAllOverResponse -= szGameAllOverResponse;

        SocketEventHandle.getInstance().DizhuAndDizhu_response -= DizhuAndDizhu_response;

        SocketEventHandle.getInstance().szRoomChatResponse -= szRoomChatResponse;

        SocketEventHandle.getInstance().SE_ReadyOnline_Response -= szReadyOnline_Response;


        SocketEventHandle.getInstance().SZ_ChangeRoom_Response -= SZ_ChangeRoom_Response;
        SocketEventHandle.getInstance().SZ_UseProp_Response -= SZ_UseProp_Response;

        //SocketEventHandle.getInstance().outRoomCallback += outRoomCallbak;
        //SocketEventHandle.getInstance().gameReadyNotice += gameReadyNotice;
        SocketEventHandle.getInstance().offlineNotice -= offlineNotice;
        SocketEventHandle.getInstance().onlineNotice -= onlineNotice;
        //CommonEvent.getInstance().readyGame += markselfReadyGame;
        //CommonEvent.getInstance().closeGamePanel += exitOrDissoliveRoom;
        SocketEventHandle.getInstance().micInputNotice -= micInputNotice;
    }

    private void flyJiaBeiObjomplete()
    {
        glodJiabeiObj.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (allGameEndPlayerArr!= null && allGameEndPlayerArr.Count > 0)
        {
            return;
        }

		SoundCtrl.getInstance ().refresh ();

		double nowTime = GlobalDataScript.getInstance().getTime();

        //金币模式的一些处理
        if (GlobalDataScript.isGoldQuickStar || GlobalDataScript.roomVo.isGoldRoom)
        {
			//飞加倍的object
			if ((nowTime - flyTimes >= 3*1000) && glodJiabeiObj.activeSelf) {

                flyTimes = nowTime;
                Tweener t = glodJiabeiObj.transform.DOLocalMove(new Vector3(900, glodJiabeiObj.transform.localPosition.y, glodJiabeiObj.transform.localPosition.z), 0.5f);
                t.OnComplete(flyJiaBeiObjomplete);
			}
        }

//        if (m_gameStartTime > 0) {
//			if (nowTime - m_enterRoomTime >= 980) {
//				m_enterRoomTime = nowTime;
//				m_gameStartTime--;
//				//显示UI开始时间
//			}
//
//		}

        if (isGameStart)
        {
            
            new_y = Input.acceleration.y;
            d_y = new_y - old_y;
            old_y = new_y;
            if (d_y >= 2)
            {
				if (isFirstBetResponse)
                {
                    reShockImg.gameObject.SetActive(false);
                    //处理UI
                    playerItems[0].playShockSeZiAnimation();
                    //发送协议
                    CustomSocket.getInstance().sendMsg(new SZReShockRequest());
                }
				if (GlobalDataScript.getInstance().zhendong) {
					Handheld.Vibrate(); //手机的震动效果  
				}
            }
        }

        //小局开奖的处理
        if (little_over_game == true && littleEndPlayerArr != null)
        {

            int flyIndex = 0;
            int flyIndex1 = 0;
            int playerIndex = 0;

            if (little_over_show_index >= littleEndPlayerArr.Count)
            {
                
                dichiAndDizhuObj.SetActive(false);

                //显示输赢图片

				resultImg.gameObject.SetActive(true);
				if (little_over_my_win == 1)
				{
					if (GlobalDataScript.roomVo.isGoldRoom)
					{
						hiddenFlyImg.sprite = Resources.Load("sizi/goldpk_doudou", typeof(Sprite)) as Sprite;
						resultImg.sprite = Resources.Load("sizi/shenli", typeof(Sprite)) as Sprite;
					}
					else
					{
						hiddenFlyImg.sprite = Resources.Load("sizi/fangkapk_flyicon", typeof(Sprite)) as Sprite;
					}
				}
				else if (little_over_my_win == 0)
				{
					if (GlobalDataScript.roomVo.isGoldRoom)
					{
						hiddenFlyImg.sprite = Resources.Load("sizi/goldpk_doudou", typeof(Sprite)) as Sprite;
						resultImg.sprite = Resources.Load("sizi/shenli", typeof(Sprite)) as Sprite;
					}
					else
					{
						resultImg.sprite = Resources.Load("sizi/shibai", typeof(Sprite)) as Sprite;
						hiddenFlyImg.sprite = Resources.Load("sizi/fangkapk_flyicon", typeof(Sprite)) as Sprite;
					}

				}

                showResultImgTime = 200;
                little_over_game = false;
                
                GameObject needCloneObj = GameObject.Instantiate(hiddenFlyImg.gameObject);
                
                //飞竞猜者和被竞猜者之间的金币
                if (jc_playerid != 0 && bjc_playerid != 0)
                {
                    int jc_index = getIndex(jc_playerid);
                    int bjc_index = getIndex(bjc_playerid);
                    if (littleEndPlayerArr[jc_index].score > 0)
                    {
                        flyIndex1 = bjc_index;
                        //底池飞到竞猜者
                        SeZiGlobalData.getMe().flyGoldOrPK(needCloneObj, playerItems[jc_index].headerIcon.gameObject, gameObject, 2, 6, false, false);
                    }
                    else if (littleEndPlayerArr[bjc_index].score > 0)
                    {
                        flyIndex1 = jc_index;
                        //底池飞到被竞猜者
                        SeZiGlobalData.getMe().flyGoldOrPK(needCloneObj, playerItems[bjc_index].headerIcon.gameObject, gameObject, 2, 6, false, false);
                    }
                }
                else
                {
                    if (GlobalDataScript.roomAvatarVoList.Count == 2)
                    {
                        //==================玩家与玩家之间飞金币=====================
                        if (littleEndPlayerArr[0].score > 0)
                        {
                            playerIndex = littleEndPlayerArr[0].playerIndex;
                            //显示获利多少
                            playerItems[playerIndex].showWinNum(littleEndPlayerArr[0].score);
                        }
                        else
                        {
                            playerIndex = littleEndPlayerArr[1].playerIndex;
                            //显示获利多少
                            playerItems[playerIndex].showWinNum(littleEndPlayerArr[1].score);
                        }
                        if (losePlayerIdsArr.Count > 0)
                        {
                            flyIndex = losePlayerIdsArr[0];
                            losePlayerIdsArr.RemoveAt(0);
                            SeZiGlobalData.getMe().flyGoldOrPK(playerItems[flyIndex].goldImg.gameObject, playerItems[playerIndex].headerIcon.gameObject, playerItems[flyIndex].gameObject, 2, 5, false, false);
                        }                        
                        //底池
                        SeZiGlobalData.getMe().flyGoldOrPK(needCloneObj, playerItems[playerIndex].headerIcon.gameObject, gameObject, 2, 6, false, false);
                        
                    }
                    else
                    {
                        //==================玩家与玩家之间飞金币=====================
                        for (int i = 0; i < littleEndPlayerArr.Count; i++)
                        {
                            playerIndex = littleEndPlayerArr[i].playerIndex;
                            if (littleEndPlayerArr[i].score > 0)
                            {
                                if (losePlayerIdsArr.Count > 0)
                                {
                                    flyIndex = losePlayerIdsArr[0];
                                    losePlayerIdsArr.RemoveAt(0);
                                }
                                else
                                {
                                    flyIndex = flyIndex1;
                                }
                                SeZiGlobalData.getMe().flyGoldOrPK(playerItems[flyIndex].goldImg.gameObject, playerItems[i].headerIcon.gameObject, playerItems[flyIndex].gameObject, 2, 5, false, false);

                                //显示获利多少
                                playerItems[playerIndex].showWinNum(littleEndPlayerArr[i].score);
                            }
                        }
                    }
                }
                
                              

                if (GlobalDataScript.roomVo.isGoldRoom) {

                    //自动切换房间
                    if (GlobalDataScript.getInstance().chageDesktop)
                    {
                        SZGoldChangeRoomReuquestVO vo = new SZGoldChangeRoomReuquestVO();
                        CustomSocket.getInstance().sendMsg(new SZChangeRoomRequest(""));
                    }
                    GlobalDataScript.getInstance().chageDesktop = false;

                } else {
					resultPanel = PrefabManage.loadPerfab("Prefab/sezi/Panel_SZLGameOver");
					resultPanel.GetComponent<SeZiLittleJSPanelScript>().setData(littleEndPlayerArr,false);
				}
                return;
            }
			if (nowTime - little_over_excu_time >= 300)
            {
				little_over_excu_time = nowTime;
                playerItems[littleEndPlayerArr[little_over_show_index].playerIndex].showResult(little_over_openpoint,little_over_opennum,littleEndPlayerArr[little_over_show_index]);
                little_over_show_index++;
            }
        }

		//竞猜倒计时
		if (jingcaiTime > 0) {
			if (nowTime - jingcaiGetTime >= 1000) {
				jingcaiTime--;
				jingcaiGetTime = nowTime;
				if (goldType_Status == 0) {
					//下次游戏开始倒计时
					//nextGameStartObj.GetComponentInChildren<Text> ().text =jingcaiTime + "秒后将开始游戏";
				} else if(goldType_Status == 6){
                    //显示UI开始时间，当前为竞猜模式
                    jingcaiObj.GetComponentInChildren<Text>().text = jingcaiTime + "秒";
				}
				if (jingcaiTime <= 0) {
					nextGameStartObj.SetActive (false);
					jingcaiObj.SetActive (false);
				}
			}
		}

        //倒计时隐藏result图片
        if (showResultImgTime >= 0)
        {
            showResultImgTime--;
            if (showResultImgTime <= 0)
            {
                resultImg.gameObject.SetActive(false);
                //隐藏结果
                overResultObj.SetActive(false);

            }
        }

	}

}

[Serializable]
public class LittleGameOverPlayerInfo
{
    public int id ;
    public int totolscore;
    public int score;
    public int lianshen;
    public int playerIndex;
    public string userName;
    public string headIcon;

    public int[] pointArr;
}
