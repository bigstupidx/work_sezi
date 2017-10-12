using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	/// <summary>
	/// 消息分发类
	/// </summary>
	public class SocketEventHandle:MonoBehaviour
	{
		private static SocketEventHandle _instance;

		public  delegate void ServerCallBackEvent (ClientResponse response);
		public  delegate void ServerDisconnectCallBackEvent ();
		public ServerCallBackEvent LoginCallBack;//登录回调
		public ServerCallBackEvent CreateRoomCallBack;//创建房间回调
        public ServerCallBackEvent QuickStartCreateRoomCallBack;//快速开始  若没有闲置的房间。则需要创建一个房间 
	    
		public ServerCallBackEvent JoinRoomCallBack;//加入房间回调
	    public ServerCallBackEvent QuickJoinRoomCallBack; //快速开始加入房间
		public ServerCallBackEvent StartGameNotice;//
		public ServerCallBackEvent pickCardCallBack;//自己摸牌
		public ServerCallBackEvent otherPickCardCallBack;//别人摸牌通知
		public ServerCallBackEvent putOutCardCallBack;//出牌通知
        public ServerCallBackEvent ChiCardCallBack;//chi牌通知
        
		public ServerCallBackEvent otherUserJointRoomCallBack;
	    public ServerCallBackEvent PengCardCallBack;//碰牌回调
        public ServerCallBackEvent ChiDataCallBack;//吃牌回调
	    public ServerCallBackEvent GangCardCallBack;//杠牌回调
		public ServerCallBackEvent HupaiCallBack;//胡牌回调
		public ServerCallBackEvent FinalGameOverCallBack;//全局结束回调
	    public ServerCallBackEvent gangCardNotice;//
		public ServerCallBackEvent btnActionShow;//碰杠行为按钮显示

		public ServerCallBackEvent outRoomCallback;//退出房间回调
		public ServerCallBackEvent dissoliveRoomResponse;
		public ServerCallBackEvent gameReadyNotice;//准备游戏通知返回
		public ServerCallBackEvent micInputNotice;
		public ServerCallBackEvent messageBoxNotice;
		public ServerCallBackEvent serviceErrorNotice;//错误信息返回
		public ServerCallBackEvent backLoginNotice;//玩家断线重连
		public ServerCallBackEvent RoomBackResponse;//掉线后返回房间
		public ServerCallBackEvent cardChangeNotice;//房卡数据变化
		public ServerCallBackEvent offlineNotice;//离线通知
		public ServerCallBackEvent onlineNotice;//上线通知
		//public ServerCallBackEvent rewardRequestCallBack;//投资请求返回
		public ServerCallBackEvent giftResponse;//奖品回调
		public ServerCallBackEvent returnGameResponse;
        public ServerCallBackEvent gameFollowBanderNotice;//跟庄
		public ServerCallBackEvent gameBroadcastNotice;//游戏公告


		public ServerDisconnectCallBackEvent disConnetNotice;//断线
		public ServerCallBackEvent contactInfoResponse;//联系方式回调
		public ServerCallBackEvent hostUpdateDrawResponse;//抽奖信息变化
		public ServerCallBackEvent zhanjiResponse;//房间战绩返回数据
		public ServerCallBackEvent zhanjiDetailResponse;//房间战绩返回数据

		public ServerCallBackEvent gameBackPlayResponse;//回放返回数据
		public ServerCallBackEvent otherTeleLogin;//其他设备登陆账户

        public ServerCallBackEvent gangtouResponse; //杠头牌返回数据
        public ServerCallBackEvent gangtouotherResponse;//其他玩家杠头返回数据

        public ServerCallBackEvent gangCompleteResponse; //起手杠全部完毕

	    public ServerCallBackEvent huataitiaoResponse;// 花台条返回数据

	    public ServerCallBackEvent goldResponse;//金币返回数据
        //private List<ClientResponse> callBackResponseList;

		public ServerCallBackEvent whoBetResponse;//谁先出牌
		public ServerCallBackEvent betResponse;//出牌的数据，房间通知
		public ServerCallBackEvent whoReShockResponse;//yao she zi

		public ServerCallBackEvent enterRoomResponse;//进入房间回调
		public ServerCallBackEvent syncPlayerDataResponse;//同步玩家信息
		public ServerCallBackEvent leaveRoomResponse;//离开房间反馈
		public ServerCallBackEvent dissoliveRoomResultResponse;//销毁房间的协议
		public ServerCallBackEvent readyResponse;//准备的协议
		public ServerCallBackEvent szGetPointResponse;//发牌的通知
        public ServerCallBackEvent szReShockResponse;//重新摇
        public ServerCallBackEvent szGameResultResponse;
		public ServerCallBackEvent szGameShowGuessPanelResponse;//开始竞猜
        public ServerCallBackEvent szGameLittleOverResponse;//小结果，当前局结束
        public ServerCallBackEvent szGameAllOverResponse;//大结果，所有圈数结束
		public ServerCallBackEvent szRoomChatResponse;	//聊天
		public ServerCallBackEvent szGameGuessResponse;//竞猜回调

		public ServerCallBackEvent SE_ReadyOnline_Response;//竞猜回调
		public ServerCallBackEvent SE_DOLTime_Response;//进入房间后的状态
		public ServerCallBackEvent SZ_UseProp_Response;//使用道具的回调
		public ServerCallBackEvent SZ_ChangeRoom_Response;//切换房间成功
        public ServerCallBackEvent DizhuAndDizhu_response;//底池和底住
        public ServerCallBackEvent SZ_Reconnect_Response;//断线重连
        public ServerCallBackEvent rank_response;//断线重连
        public ServerCallBackEvent SZChargeResponse;//充值回调

        private List<ClientResponse> callBackResponseList;

		private bool isDisconnet = false;


		public SocketEventHandle ()
		{
			callBackResponseList = new List<ClientResponse> ();
		}

		void Start(){
			SocketEventHandle.getInstance ();
		}

		public static SocketEventHandle getInstance(){
			if (_instance == null) {
				GameObject temp = new GameObject ();
				_instance = temp.AddComponent<SocketEventHandle> ();
			}
			return _instance;
		}

		void Update () {
			
		}

		void FixedUpdate(){
			while(callBackResponseList.Count >0){
				ClientResponse response = callBackResponseList [0];
				callBackResponseList.RemoveAt (0);
				dispatchHandle (response);
			}

			if (isDisconnet) {
				isDisconnet = false;
				disConnetNotice ();
			}

		}

		private void dispatchHandle(ClientResponse response){
            if (response.headCode != 49)
            {
                //int test = 111;
            }
			//print ("FixedUpdate::" + response.message);
			switch(response.headCode){
			case APIS.CLOSE_RESPONSE:
				TipsManagerScript.getInstance ().setTips ("服务器关闭了");
				CustomSocket.getInstance ().closeSocket ();
				break;
			case APIS.LOGIN_RESPONSE:
				if (LoginCallBack != null) {
					LoginCallBack(response);
				}
				break;
			case APIS.CREATEROOM_RESPONSE:
				
                    if (PlayerHuaTaiTiao.getMe().isQuickStart)
                    {
                        if (QuickStartCreateRoomCallBack != null)
                        {
                            QuickStartCreateRoomCallBack(response);
                        }
                    }
                    else
                    {
                        if (CreateRoomCallBack != null)
                        {
                            CreateRoomCallBack(response);
                        }
                        
                    }
                    PlayerHuaTaiTiao.getMe().isQuickStart = false;

				break;
			case APIS.JOIN_ROOM_RESPONSE:
			    if (PlayerHuaTaiTiao.getMe().isQuickStart)
			    {
			        if (QuickJoinRoomCallBack != null)
			        {
			            QuickJoinRoomCallBack(response);
			        }
			    }
			    else
			    {
			        if (JoinRoomCallBack != null)
			        {
			            JoinRoomCallBack(response);
			        }
                        
			    }
			    PlayerHuaTaiTiao.getMe().isQuickStart = false;
				
				break;
			case APIS.STARTGAME_RESPONSE_NOTICE:
				if (StartGameNotice != null) {
					StartGameNotice (response);
				}
				break;
			case APIS.PICKCARD_RESPONSE:
				if (pickCardCallBack != null) {
					pickCardCallBack (response);
				}
				break;
			case APIS.OTHER_PICKCARD_RESPONSE_NOTICE:
				if (otherPickCardCallBack != null) {
					otherPickCardCallBack (response);
				}
				break;
			case APIS.CHUPAI_RESPONSE:
				if(putOutCardCallBack != null){
					putOutCardCallBack (response);
				}
				break;
			case APIS.JOIN_ROOM_NOICE:
				if (otherUserJointRoomCallBack != null) {
					otherUserJointRoomCallBack (response);
				}
				break;
            case APIS.PENGPAI_RESPONSE:
                    if (PengCardCallBack != null)
                    {
                        PengCardCallBack(response);
                    }
                    break;
            case APIS.CHIPAIDATA_RESPONSE:
                    if (ChiDataCallBack != null)
                    {
                        ChiDataCallBack(response);
                    }
                    break;
            case APIS.CHIPAI_RESPONSE:
                    if (ChiCardCallBack != null)
                    {
                        ChiCardCallBack(response);
                    }
                    break;
            case APIS.GANGPAI_RESPONSE:
			        if (GangCardCallBack != null)
			        {
			            GangCardCallBack(response);
			        }
			        break;
                case APIS.OTHER_GANGPAI_NOICE:
			        if (gangCardNotice != null)
			        {
			            gangCardNotice(response);
			        }
                    break;
				case APIS.RETURN_INFO_RESPONSE:
					if (btnActionShow != null) {
						btnActionShow (response);
					}
					break;
			case APIS.HUPAI_RESPONSE:
				if (HupaiCallBack != null) {
					HupaiCallBack (response);
				}
				break;
			case APIS.HUPAIALL_RESPONSE:
				if (FinalGameOverCallBack != null) {
					FinalGameOverCallBack(response);
				}
				break;

			case APIS.OUT_ROOM_RESPONSE:
				if (outRoomCallback != null) {
					outRoomCallback (response);
				}
				break;
			case APIS.headRESPONSE:
                    //print("headRESPONSE");
				break;
			case APIS.Disspose_Room_Response:
				if (dissoliveRoomResponse != null) {
					dissoliveRoomResponse (response);
				}
				break;
			case APIS.PrepareGame_MSG_RESPONSE:
				if (gameReadyNotice != null) {
					gameReadyNotice (response);
				}
				break;
			case APIS.MicInput_Response:
				if (micInputNotice != null) {
					micInputNotice (response);
				}
				break;
			case APIS.MessageBox_Notice:
				if (messageBoxNotice != null) {
					messageBoxNotice (response);
				}
				break;
			case APIS.ERROR_RESPONSE:
				if(serviceErrorNotice !=null){
					serviceErrorNotice(response);
				}
				break;
			case APIS.BACK_LOGIN_RESPONSE:
				if (RoomBackResponse != null) {
					RoomBackResponse (response);
				}

				break;
			case APIS.CARD_CHANGE:
				if (cardChangeNotice != null) {
					cardChangeNotice (response);
				}
				break;
			case APIS.OFFLINE_NOTICE:
				if (offlineNotice != null) {
					offlineNotice (response);
				}
				break;
			case APIS.RETURN_ONLINE_RESPONSE:
				
				if (returnGameResponse != null) {
					returnGameResponse (response);
				}
				break;
			case APIS.PRIZE_RESPONSE:
				if (giftResponse != null) {
					giftResponse (response);
				}
				break;

            case APIS.Game_FollowBander_Notice:
                if (gameFollowBanderNotice != null)
                    {
                        gameFollowBanderNotice(response);
                    }
                break;
            

			case APIS.ONLINE_NOTICE:
				if (onlineNotice != null) {
					onlineNotice (response);
				}
				break;
			
			case APIS.GAME_BROADCAST:
				if (gameBroadcastNotice != null) {
					gameBroadcastNotice (response);
				}
				break;

			case APIS.CONTACT_INFO_RESPONSE:
				if (contactInfoResponse != null) {
					contactInfoResponse (response);
				}
				break;
			case APIS.HOST_UPDATEDRAW_RESPONSE:
				if (hostUpdateDrawResponse != null) {
					hostUpdateDrawResponse (response);
				}
				break;
            case APIS.zhanji_response:
				if (zhanjiResponse != null) {
					zhanjiResponse (response);
				}
				break;
			case APIS.ZHANJI_DETAIL_REPORTER_REPONSE:
				if (zhanjiDetailResponse != null) {
					zhanjiDetailResponse (response);
				}
				break;
			case APIS.GAME_BACK_PLAY_RESPONSE:
				if (gameBackPlayResponse != null) {
					gameBackPlayResponse (response);
				}
				break;
			case APIS.TIP_MESSAGE:
				TipsManagerScript.getInstance ().setTips (response.message);
				break;
			case APIS.OTHER_TELE_LOGIN:
				if (otherTeleLogin != null) {
					otherTeleLogin (response);
				}
				break;
            case APIS.HUA_RESPONSE:
                if (gangtouResponse != null)
                {
                    gangtouResponse(response);
                }
                break;
            case APIS.GANGTOOTHER_RESPONSE:
                if (gangtouotherResponse != null)
                {
                    gangtouotherResponse(response);
                }
                break;
            case APIS.GANGCOMPLETE_RESPONSE:
                if (gangCompleteResponse != null)
                {
                    gangCompleteResponse(response);
                }
                break;
            case APIS.HuaTiaoTai_RESPONSE:
                PlayerHuaTaiTiao.getMe().plyaerData = response.message;
                break;
            case APIS.Gold_change_REQUEST:
                if (goldResponse != null)
                {
                    goldResponse(response);
                }
                break;

				//======================色子的===========================
			case APIS.WHO_BET_RESPONSE:
				if (whoBetResponse != null)
				{
					whoBetResponse(response);
				}
				break;
			case APIS.BET_RESPONSE:
				if (betResponse != null)
				{
					betResponse(response);
				}
				break;

			case APIS.WHO_RE_SHOCK_REQUEST:
				if (whoReShockResponse != null)
				{
					whoReShockResponse(response);
				}
				break;

			case APIS.Denter_game_response:
				if (enterRoomResponse != null)
				{
					enterRoomResponse(response);
				}
				break;
			case APIS.Leave_Room_response:
				if (leaveRoomResponse != null)
				{
					leaveRoomResponse(response);
				}
				break;

			case APIS.DSync_PlayerData_response:
				if (syncPlayerDataResponse != null)
				{
					syncPlayerDataResponse(response);
				}
				break;
			case APIS.Disspose_Confirm_Response:
				if (dissoliveRoomResultResponse != null)
				{
					dissoliveRoomResultResponse(response);
				}
				break;

			case APIS.SZ_Ready_Response:
				if (readyResponse != null)
				{
					readyResponse(response);
				}
				break;
			case APIS.SZ_GET_POINT_Response:
				if (szGetPointResponse != null)
				{
					szGetPointResponse(response);
				}
				break;
            case APIS.WHO_RE_SHOCK_RESPONSE:
                if (szReShockResponse != null)
                {
                    szReShockResponse(response);
                }
                    break;
            case APIS.SE_GameOver_Response:
                if (szGameLittleOverResponse != null)
                {
                    szGameLittleOverResponse(response);
                }
                    break;
            case APIS.SE_GameAllOver_Response:
                if (szGameAllOverResponse != null)
                {
                        szGameAllOverResponse(response);
                }
                break;
            case APIS.SE_Guess_Response:
                if (szGameShowGuessPanelResponse != null)
                {
                    szGameShowGuessPanelResponse(response);
                }
                break;
			case APIS.SE_RoomChat_Response:
				if (szRoomChatResponse != null)
				{
					szRoomChatResponse(response);
				}
				break;
             case APIS.SE_Guess_Result_Response:
                    if (szGameGuessResponse != null)
                    {
                        szGameGuessResponse(response);
                    }
                    break;
			case APIS.SE_ReadyOnline_Response:
				if (SE_ReadyOnline_Response != null)
				{
					SE_ReadyOnline_Response(response);
				}
				break;
			case APIS.SE_DOLTime_Response:
				if (SE_DOLTime_Response != null)
				{
					SE_DOLTime_Response(response);
				}
				break;
			case APIS.SZ_UseProp_Response:
				if (SZ_UseProp_Response != null)
				{
					SZ_UseProp_Response(response);
				}
				break;
				break;
			case APIS.SZ_ChangeRoom_Response:
				if (SZ_ChangeRoom_Response != null)
				{
					SZ_ChangeRoom_Response(response);
				}
				break;
                case APIS.DizhuAndDizhu_response:
                    if (DizhuAndDizhu_response != null)
                    {
                        DizhuAndDizhu_response(response);
                    }
                    break;
                case APIS.SZ_Reconnect_Response:
                    if (SZ_Reconnect_Response != null)
                    {
                        SZ_Reconnect_Response(response);
                    }
                    break;
                case APIS.rank_response:
                    if (rank_response != null)
                    {
                        rank_response(response);
                    }
                    break;

                case APIS.SZ_Charge_Response:
                    if (SZChargeResponse != null)
                    {
                        SZChargeResponse(response);
                    }
                    break;
            }


        }

		public void addResponse(ClientResponse response){
			callBackResponseList.Add (response);
		}


		public void noticeDisConect(){
			isDisconnet = true;
		}
	}
}

