using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class GlobalDataScript
{
    public bool isInGame = false;
	public static bool hideChargeUI = false;
    public static bool isGoldQuickStar = false;
    public static bool canOpenSZ = false;
    public static bool sendedOpenSZRequest = false;
    public static bool isCallOne = false;
    public static int[] myPointsArr;
	public static int currentBeiShu = 0;
    public bool gameStart = false;
	public bool chageDesktop;//是否发送切换桌子请求
	public bool sendGoldAutoExitRequest;//是否发送自动退出
    public static bool shouchongrugou = true;  //true  商城进入 false 砖石进入
    public bool reConnect;//是否断线重连
	public static List<string> dyChatList = new List<string> {
        "好啊！各位！",
        "嘿嘿！这把我可赢定了！",
        "你玩的这么溜，你家里人知道么？",
        "呵呵，不坐死就不会死！",
        "哎，感觉这把要跪啊！",
        "人生如戏，全靠演技！",
        "别走啊，有本事再来一把！",
        "小心点，当心顺子和豹子！"
	};

    public static bool isDrag = false;
    /**登陆返回数据**/
    public static AvatarVO loginResponseData;
    /**加入房间返回数据**/
    public static RoomJoinResponseVo roomJoinResponseData;
    /**房间游戏规则信息**/
    public static RoomCreateVo roomVo = new RoomCreateVo();
    /**单局游戏结束服务器返回数据**/
    public static HupaiResponseVo hupaiResponseVo;
    /**全局游戏结束服务器返回数据**/
    public static FinalGameEndVo finalGameEndVo;

    public static int mainUuid;
    /**房间成员信息**/
	public static List<AvatarVO> roomAvatarVoList = new List<AvatarVO>();

    //	public static Dictionary<int, Account > palyerBaseInfo = new Dictionary<int, Account> (); 

    public static GameObject homePanel;//主界面
    public static GameObject gamePlayPanel;//游戏界面

    /**麻将剩余局数**/
    public static int surplusTimes;
    /**总局数**/
    public static int totalTimes;
    /**默认面板**/
    public static int userMaJiangKind;
    /// <summary>
    /// 最顶层的容器
    /// </summary>
    public Transform canvsTransfrom;
    /**重新加入房间的数据**/
    public static RoomJoinResponseVo reEnterRoomData;

    public WechatOperateScript wechatOperate;
    /// <summary>
    /// 声音开关
    /// </summary>
    public static bool soundToggle = true;

    public static List<String> messageBoxContents = new List<string>();
    /// <summary>
    /// 单局结算面板
    /// </summary>
    public static List<GameObject> singalGameOverList = new List<GameObject>();


    public static bool isonLoginPage;//是否在登陆页面

    //public SocketEventHandle socketEventHandle;
    /// <summary>
    /// 抽奖数据
    /// </summary>
    public static List<LotteryData> lotteryDatas;
    public static bool isonApplayExitRoomstatus = false;//是否处于申请解散房间状态
    public static bool isOverByPlayer = false;//是否由用用户选择退出而退出的游戏
    public static LoginVo loginVo;//登录数据
    public static List<String> noticeMegs = new List<string>();


    //储存本局的花和杠头
    public int hua;
    public int gangtou;
    public int gui;


	public bool music = true;
	public bool yuyin = true;
	public bool yinxiao = true;
	public bool zhendong = true;

    public List<int> gangTouData=new List<int>();

    public List<List<int>> GangtouList;
    /**
     * 重新初始化数据
    */
    public static void reinitData()
    {
        isDrag = false;
        loginResponseData = null;
        roomJoinResponseData = null;
        roomVo = new RoomCreateVo();
        hupaiResponseVo = null;
        finalGameEndVo = null;
        roomAvatarVoList = null;
        surplusTimes = 0;
        totalTimes = 0;
        userMaJiangKind = 0;
        reEnterRoomData = null;
        singalGameOverList = new List<GameObject>();
        lotteryDatas = null;
        isonApplayExitRoomstatus = false;
        isOverByPlayer = false;
        loginVo = null;

      
    }


    public void init()
    {
        //socketEventHandle = GameObject.Find ("Canvas").transform.GetComponent<SocketEventHandle> ();
        canvsTransfrom = GameObject.Find("container").transform;
        TipsManagerScript.getInstance().parent = GameObject.Find("Canvas").transform;
        wechatOperate = GameObject.Find("Canvas").GetComponent<WechatOperateScript>();
        initMessageBox();
        hua = 0;
        gangtou = 0;
        gui = 0;
    }

    void initMessageBox()
    {
        messageBoxContents.Add("快点啦，时间很宝贵的！");
        messageBoxContents.Add("又断线了，网络怎么这么差！");
        messageBoxContents.Add("不要走决战到天亮。");
        messageBoxContents.Add("你的牌打的太好了！");
        messageBoxContents.Add("你是美眉还是哥哥？");
        messageBoxContents.Add("和你合作真是太愉快了！");
        messageBoxContents.Add("大家好很高兴见到大家！");
        messageBoxContents.Add("各位真不好意思,我要离开一会");
        messageBoxContents.Add("有什么好吵的,专心玩游戏吧！");
        messageBoxContents.Add("我到底跟你有什么仇什么怨！");
        messageBoxContents.Add("哎呀！打错了怎么办");
    }

    private static GlobalDataScript _instance;
    public static GlobalDataScript getInstance()
    {
        if (_instance == null)
        {
            _instance = new GlobalDataScript();
        }
        return _instance;
    }

    public GlobalDataScript()
    {
        init();
    }

    public string getIpAddress()
    {
        string tempip = "";
        try
        {
            WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("[") + 1;
            int end = all.IndexOf("]");
            int count = end - start;
            tempip = all.Substring(start, count);
            sr.Close();
            s.Close();
        }
        catch
        {
        }
        return tempip;
    }


	//获取unix时间戳
	public double getTime() {
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);  
		return ts.TotalMilliseconds;
	}

	public void reset() {
		canOpenSZ = false;
		sendedOpenSZRequest = false;
		isCallOne = false;
		//myPointsArr = new int[]{ };
		currentBeiShu = 0;
        gameStart = false;
		chageDesktop = false;
        sendGoldAutoExitRequest = false;
    }

    public void removeAvatorById(int playerId)
    {
        if (roomAvatarVoList != null && roomAvatarVoList.Count > 0)
        {
            for (int i = 0; i < roomAvatarVoList.Count; i++)
            {
                if (roomAvatarVoList[i].account.uuid == playerId || roomAvatarVoList[i].account.id == playerId)
                {
                    roomAvatarVoList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    //把数字转换成W
    public string chageNumToW(int num)
    {
        if (num >= 10000)
        {
            float num1 = num / 10000;

            return Math.Round(num1,2)+"W";
        }
        return num.ToString();
    }

}

