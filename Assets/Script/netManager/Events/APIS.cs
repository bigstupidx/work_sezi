using System;

namespace AssemblyCSharp
{
	public class APIS
	{
		public APIS ()
		{
		}
        //public const string UPDATE_INFO_JSON_URL = "http://182.254.146.120:8080/MaJiangManage/images/update.xml";//服务器上最新的软件版本信息存储文件
		public const string UPDATE_INFO_JSON_URL = "http://www.weipaigame.com/downLoad/appDown/update.xml";//服务器上最新的软件版本信息存储文件
        //public const string UPDATE_INFO_JSON_URL = "http://182.254.146.120/test/update.xml";
        public const string chatSocketUrl = "120.77.148.252";//；语音服务器
		//public const string chatSocketUrl = "192.168.0.112";
		//public const string socketUrl = "118.178.20.36";

        // public const string socketUrl = "120.77.148.252";//测试服务器

    //   public const string socketUrl = "101.201.43.166";//外网测试服务器
		public const string socketUrl = "112.74.52.173";//自有外网测试服务器
        public const string webUrl = "http://112.74.52.173/shaiziwang/";//自带web服务器
        //public const string socketUrl = "127.0.0.1";//自有外网测试服务器
        public const string PIC_PATH = "http://101.200.179.62:8080/";
	//	public const string apkDownLoadurl="192.168.0.111/aaa/weipai.apk";
        public const string ImgUrl = "http://182.254.146.120:9096/weiPaiImage/";
		public const int head = 0x000030;
		public const int headRESPONSE = 0x000031;

		//游戏关闭返回
		public const int CLOSE_RESPONSE = 0x000000;
        public const int LOGIN_REQUEST = 0x000001; //登陆请求码
		public const int LOGIN_RESPONSE = 0x000002;//登陆返回码

        public const int JOIN_ROOM_REQUEST = 0x000003;//加入房间请求码
        public const int JOIN_ROOM_RESPONSE = 0x000004;//加入房间返回码
		public const int JOIN_ROOM_NOICE = 0x10a004;//其它 人加入房间通知


		public const int CREATEROOM_REQUEST = 0x200001;//创建房间请求码
        public const int CREATEROOM_RESPONSE = 0x00010;//创建房间返回吗


		public const int PICKCARD_RESPONSE = 0x100004;//自己摸牌 
		public const int OTHER_PICKCARD_RESPONSE_NOTICE = 0x100014;//别人摸牌通知

		public const int RETURN_INFO_RESPONSE =  0x100000;
		public const int CHUPAI_REQUEST = 0x100001;//出牌请求
		public const int CHUPAI_RESPONSE = 0x100002;//出牌通知

        public const int PENGPAI_REQUEST = 0x100005;//碰牌请求
        public const int PENGPAI_RESPONSE = 0x100006;//碰牌通知

        public const int CHIPAI_REQUEST = 0x100011;//吃牌请求
        public const int CHIPAI_RESPONSE = 0x100012;//吃牌通知 


        public const int CHIPAIDATA_REQUEST = 0x003003;//吃牌先向服务器请求数据
        public const int CHIPAIDATA_RESPONSE = 0x003004;//吃牌先向服务器请求数据返回

        public const int GANGPAI_REQUEST = 0x100007;//杠牌请求
        public const int GANGPAI_RESPONSE = 0x100008;//杠牌返回
	    public const int OTHER_GANGPAI_NOICE = 0x10a008;//杠牌通知
        public const int HUPAI_REQUEST = 0x100009;//胡牌请求
        public const int HUPAI_RESPONSE = 0x100010;//胡牌通知
		public const int HUPAIALL_RESPONSE = 0x100110;//全局结束通知
        public const int GAVEUP_REQUEST = 0x100015;//放弃（胡，杠，碰，吃）

		public const int BACK_LOGIN_REQUEST = 0x001001;//掉线后重新登录查询当前牌桌情况请求
		public const int BACK_LOGIN_RESPONSE= 0x001002;//掉线后重新登录查询当前牌桌情况返回

		public const int OUT_ROOM_REQUEST = 0x000013;//退出房间请求
		public const int OUT_ROOM_RESPONSE =0x000014;//退出房间返回数7

		public const int DISSOLIVE_ROOM_REQUEST = 0x2000051;//申请解散房间
		public const int DISSOLIVE_ROOM_RESPONSE = 0X000114;//解散房间回调

		public const int  PrepareGame_MSG_REQUEST = 0x333333;//
		public const int  PrepareGame_MSG_RESPONSE = 0x444444;//

		public const int ERROR_RESPONSE = 0xffff09;//错误回调

		public const int MicInput_Request = 200;
		public const int MicInput_Response = 201;

		public const int LoginChat_Request = 202;

		public const int MessageBox_Request = 203;
		public const int MessageBox_Notice = 204;

		public const int QUITE_LOGIN=0x555555;//退出登录调用，仅限于正常登录
		public const int CARD_CHANGE=0x777777;

		//public const int OUT_ROOM_RESPONSE = 0x001002;//离线通知
		public const int OFFLINE_NOTICE = 0x000015;
	    public const int ONLINE_NOTICE = 0x001111;

        public const int PRIZE_RESPONSE = 0x999999;//抽奖接口
		public const int GET_PRIZE=0x888888;//抽奖请求接口

		public const int RETURN_ONLINE_RESPONSE = 0x001003;//断线重连返回最后一次打牌数据
		public const int REQUEST_CURRENT_DATA = 0x001004;//申请最后打牌数据数据

	    public const int Game_FollowBander_Notice = 0x100016;//跟庄


		public const int GAME_BROADCAST = 0x157777;//游戏公告
		public const int CONTACT_INFO_REQUEST= 0x156666;//添加房卡请求数据
		public const int CONTACT_INFO_RESPONSE = 0x155555;//添加房卡返回数据
		public const int HOST_UPDATEDRAW_RESPONSE = 0x010111;//抽奖信息变化
		public const int ZHANJI_REPOTER_REQUEST = 0x002001;//战绩请求
		public const int ZHANJI_REPORTER_REPONSE=0x002002;//房间战绩返回数据
		public const int ZHANJI_DETAIL_REPORTER_REPONSE=0x002003;//某个房间详细每局战绩
		public const int ZHANJI_SEARCH_REQUEST= 0x002004;//搜索房间对应战绩

		public const int GAME_BACK_PLAY_REQUEST=0x003001;//回放请求
		public const int GAME_BACK_PLAY_RESPONSE = 0x003002;//回放返回数据
		public const int TIP_MESSAGE = 0x160016;

		public const int OTHER_TELE_LOGIN = 0x211211;//其他设备登录

        public const int HUA_RESPONSE = 0x004001;  //杠头 通知
        public const int GANGTOOTHER_RESPONSE = 0x003005; //杠头和白板杠 的其他家的显示
        public const int GANGCOMPLETE_RESPONSE = 0x003006; //起手杠全部完毕

	    public const int HuaTiaoTai_RESPONSE = 0x003007; //算分接口

		
        public const int Gold_change_REQUEST = 0x003009; //玩家金币发生变化接口




		public const int Denter_game_response = 0x200002;//进入房间回调
		//int creatid创建者id   roomid   curtimes当前第几句   isstart房间状态0没开始  1开始      status房间到那个阶段 

		public const int DSync_PlayerData_response = 0x200003;//同步玩家信息
		//string headimg头像    int id   string nickname   scroe   isready   isbank  isshowQiangBtn
		public const int Denter_game_request = 0x200004;//进入房间
		//int roomid

		//int playerid
		public const int WHO_BET_RESPONSE = 0x200030;//开始游戏后服务器主动推
		//int playerid  
		public const int BET_REQUEST = 0x200031;
		//int keyNum   int value Num
		public const int BET_RESPONSE = 0x200032;//广播
		//int playerid  int keyNum   int valueNum
		public const int WHO_RE_SHOCK_REQUEST = 0x200037;//重新摇色子
		//playerid
		public const int WHO_RE_SHOCK_RESPONSE = 0x200038;//重新摇色子
		//playerid

		public const int Leave_Room_request = 0x200005;//离开房间
		public const int Leave_Room_response = 0x200006;//离开房间反馈

        public const int STARTGAME_REQUEST = 0x200007;//开始游戏
        public const int STARTGAME_RESPONSE_NOTICE = 0x200008;//开始游戏

		public const int Disspose_Room_Request = 0x200009;//请求解散房间
		public const int Disspose_Room_Response = 0x200010;//请求解散房间反馈 	
		public const int Disspose_Confirm_Request = 0x200011;//是否解散房间
		//int isdispose  0fou  1shi
		public const int Disspose_Confirm_Response = 0x200012;//解散房间fankui

		public const int SZ_Ready_Request = 0x200013;//准备	
		public const int SZ_Ready_Response = 0x200014;//准备反馈
		//int playerid  

		public const int SZ_GET_POINT_Response = 0x200019;//发牌消息
        public const int SE_OpenSeZi_Request = 0x200033;//开启别人的骰子
        public const int SE_Guess_Response = 0x200034;//开始竞猜，弹出面板
       
        public const int SE_Guess_Request = 0x200035;//开始竞猜  yes 0表示不够，1表示够
        public const int SE_Guess_Result_Response = 0x200039;//竞猜结果        int key    0   1    uuid
        public const int SE_GameOver_Response = 0x200020;//游戏当前局结束
        //id,totolscore,score
        //id,card1,card2,card3,card4,card5
        //小结算字段
        public const int SE_GameAllOver_Response = 0x200021;//房卡当前轮结束
        //headimg,nickname,playtiems,totolscore
        //===================大结算字段=============

		public const int SE_RoomChat_Request = 0x200022;//聊天
		public const int SE_RoomChat_Response = 0x200023;//聊天

		public const int QickStart_REQUEST = 0x200054; //快速开始请求
		// int time  int statuss
		public const int SE_ReadyOnline_Response=0x200050;//金币模式的在线准备
		// int uuid  int statuss
		public const int SE_DOLTime_Response=0x200051;  //这个消息比较重要。我会同步什么状态。多少倒计时
                                                        // int time  int statuss int uuid

        public const int Auto_play_Canel_request = 0x200055;//托管
        public const int SZ_ChangeRoom_Request = 0x200052;	//切换房间请求
		public const int SZ_ChangeRoom_Response = 0x200053;	//切换房间回调

		public const int SZ_ExitGoldRoom_Request = 0x200060;	//离开金币房间(房间游戏开始了)

        public const int DizhuAndDizhu_response = 0x200056;//同步底注和低池
        //int type  0 地主  1低池      value 多少

        public const int SZ_UseProp_Request = 0x200062;	//使用2倍/5倍道具   
		//type 2:1倍底注	2:5倍抢开  均是游戏开始第一个玩家出牌前使用
		public const int SZ_UseProp_Response = 0x200063;//使用成功

        public const int SZ_Exchange_Request = 0x200057;//兑换金币

        public const int zhanji_request = 0x200026;  //请求战绩
        public const int zhanji_response = 0x200027; //战绩回调

        //请求排行
        public const int rank_request = 0x200028;  //int type 0金币排行  1钻石排行
        //排行回调
        public const int rank_response = 0x200029;

        public const int SZ_Reconnect_Response = 0x200024;//断线重连
        // int roomid  0没有房间   否则有房间


    }



}

