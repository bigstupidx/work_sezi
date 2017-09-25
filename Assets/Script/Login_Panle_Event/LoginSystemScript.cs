using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using cn.sharesdk.unity3d;


public class LoginSystemScript : MonoBehaviour {


    //public ShareSDK shareSdk;
    private GameObject panelCreateDialog;

    public Toggle agreeProtocol;

    public Text versionText;
    public Text progressText;

    private int tapCount = 0;//点击次数
    public GameObject watingPanel;

    public List<Toggle> users;


    void Start() {

        //this.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        //shareSdk.showUserHandler = getUserInforCallback;//注册获取用户信息回调
        CustomSocket.hasStartTimer = false;
        SoundCtrl.getInstance().playBGM(1);
        SocketEventHandle.getInstance().LoginCallBack += LoginCallBack;
        SocketEventHandle.getInstance().RoomBackResponse += RoomBackResponse;


        GlobalDataScript.isonLoginPage = true;
        versionText.text = "版本号：" + Application.version;
        //WxPayImpl test = new WxPayImpl(gameObject);
        //test.callTest ("dddddddddddddddddddddddddddd");
        if (watingPanel != null)
        {
            watingPanel.SetActive(false);
        }
        StartCoroutine(ConnectTime1(1f, 1));


        SoundCtrl.getInstance().StartaudioS();
        SoundCtrl.getInstance().StartSound();
        SoundCtrl.getInstance().startSoundByAction();
        SoundCtrl.getInstance().StartaudioS();
        

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Escape)) { //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
            if (panelCreateDialog == null) {
                panelCreateDialog = Instantiate(Resources.Load("Prefab/Panel_Exit")) as GameObject;
                panelCreateDialog.transform.parent = gameObject.transform;
                panelCreateDialog.transform.localScale = Vector3.one;
                //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
                panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            }

        }

    }

    int count = 0;

    IEnumerator ConnectTime1(float time, byte type)
    {
        connectRetruen = false;
        yield return new WaitForSeconds(time);
        if (!connectRetruen)
        {//超过5秒还没连接成功显示失败


            if (type == 1)
            {

                CustomSocket.hasStartTimer = false;
                CustomSocket.getInstance().Connect();
                ChatSocket.getInstance().Connect();
                GlobalDataScript.isonLoginPage = true;

            }
            else if (type == 2)
            {

            }

        }
    }


    public void login() {
        MyDebug.Log("----------------1-------------------");
        if (!CustomSocket.getInstance().isConnected) {
            CustomSocket.getInstance().Connect();
            ChatSocket.getInstance().Connect();
            tapCount = 0;
            MyDebug.Log("----------------2------------------");
            return;
        }



        GlobalDataScript.reinitData();//初始化界面数据
        if (agreeProtocol.isOn) {
            MyDebug.Log("----------------3------------------");
            progressText.text = "正在初始化界面数据...";
            doLogin();
            watingPanel.SetActive(true);
            InvokeRepeating("GoSlider", 0f, 0.02f);
        } else {
            MyDebug.Log("请先同意用户使用协议");
            TipsManagerScript.getInstance().setTips("请先同意用户使用协议");
        }

        tapCount += 1;
        Invoke("resetClickNum", 10f);

    }

    bool connectRetruen = false;


    IEnumerator ConnectTime(float time, byte type)
    {
        connectRetruen = false;
        yield return new WaitForSeconds(time);
        if (!connectRetruen)
        {//超过5秒还没连接成功显示失败

            if (watingPanel != null)
            {
                watingPanel.SetActive(false);
            }

        }
    }

    public void loginFromUnity()
    {
        MyDebug.Log("----------------1-------------------");
        if (!CustomSocket.getInstance().isConnected)
        {
            CustomSocket.getInstance().Connect();
            ChatSocket.getInstance().Connect();
            tapCount = 0;
            MyDebug.Log("----------------2------------------");
            return;
        }



        GlobalDataScript.reinitData();//初始化界面数据
        if (agreeProtocol.isOn)
        {
            watingPanel.SetActive(true);
            InvokeRepeating("GoSlider", 0f, 0.02f);
            MyDebug.Log("----------------3------------------");
            int id = -1;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].isOn)
                {
                    id = i + 1;
                    break;
                }
            }

            if (id >= 0)
            {
                CustomSocket.getInstance().sendMsg(new LoginRequest(id));
                return;
            }
           
        }
        else
        {
            MyDebug.Log("请先同意用户使用协议");
            TipsManagerScript.getInstance().setTips("请先同意用户使用协议");
        }

        tapCount += 1;
        Invoke("resetClickNum", 10f);
            }



     public void doLogin(){
        StartCoroutine(ConnectTime(10, 0));
        GlobalDataScript.getInstance().wechatOperate.login();
        return;
        int id = -1;
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].isOn)
            {
                id = i + 1;
                break;
            }
        }

        if (id >= 0)
        {
            CustomSocket.getInstance().sendMsg(new LoginRequest(id));
            return;
        }
        watingPanel.SetActive(true);
        //用于测试 不用微信登录
        //CustomSocket.getInstance().sendMsg(new LoginRequest(null));
        //GlobalDataScript.getInstance ().wechatOperate.login ();*/
    }

    private ClientResponse response1;
    public void LoginCallBack(ClientResponse response){

        Debug.Log("ranger1" + response.message);
		if (watingPanel != null) {
           
            progressText.text = "正在进入游戏...";

            Invoke("waiter", 2f);
		}
        response1 = response;

        SoundCtrl.getInstance().playBGM(1);
		
	}
    private void GoSlider()
    {
        watingPanel.GetComponentInChildren<Slider>().value += 0.4f*Time.deltaTime;
    }
    private void  waiter()
    {
        watingPanel.SetActive(false);
        CancelInvoke();
        if (response1.status == 1)
        {
            if (GlobalDataScript.homePanel != null)
            {
                GlobalDataScript.homePanel.GetComponent<HomePanelScript>().removeListener();
                Destroy(GlobalDataScript.homePanel);
            }


            if (GlobalDataScript.gamePlayPanel != null)
            {
                //GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript> ().exitOrDissoliveRoom ();
                GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript>().exitOrDissoliveRoom();
            }

            GlobalDataScript.loginResponseData = JsonMapper.ToObject<AvatarVO>(response1.message);
            ChatSocket.getInstance().sendMsg(new LoginChatRequest(GlobalDataScript.loginResponseData.account.uuid));
            panelCreateDialog = Instantiate(Resources.Load("Prefab/YueqinPanel/Panel_HomeNew")) as GameObject;
            panelCreateDialog.transform.parent = GlobalDataScript.getInstance().canvsTransfrom;
            panelCreateDialog.transform.localScale = Vector3.one;
            panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            GlobalDataScript.homePanel = panelCreateDialog;
            removeListener();
            Destroy(this);
            Destroy(gameObject);
            Debug.Log("ranger1   Prefab/YueqinPanel/Panel_HomeNew");
        }
    }

    GameObject Panel_xieyi;
    public void xieyi()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);

		return;
        if (Panel_xieyi == null)
        {
            Panel_xieyi = PrefabManage.loadPerfab("Prefab/Panel_xieyi");
        }
        else
        {
            Panel_xieyi.SetActive(true);
        }


    }

    public void closexieyi()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);
       // if (Panel_xieyi != null)
            // Panel_xieyi.SetActive(false);
            Destroy(Panel_xieyi);

    }
	private void removeListener(){
		SocketEventHandle.getInstance ().LoginCallBack -= LoginCallBack;
		SocketEventHandle.getInstance ().RoomBackResponse -= RoomBackResponse;
	}

	private void RoomBackResponse(ClientResponse response){

		watingPanel.SetActive(false);

		if (GlobalDataScript.homePanel != null) {
			GlobalDataScript.homePanel.GetComponent<HomePanelScript> ().removeListener ();
			Destroy (GlobalDataScript.homePanel);
		}


		if (GlobalDataScript.gamePlayPanel != null) {
			//GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript> ().exitOrDissoliveRoom ();
			GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript> ().exitOrDissoliveRoom ();
		}
		GlobalDataScript.reEnterRoomData = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);

		for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			AvatarVO itemData =	GlobalDataScript.reEnterRoomData.playerList [i];
			if (itemData.account.openid == GlobalDataScript.loginResponseData.account.openid) {
				GlobalDataScript.loginResponseData.account.uuid = itemData.account.uuid;
				ChatSocket.getInstance ().sendMsg (new LoginChatRequest(GlobalDataScript.loginResponseData.account.uuid));
				break;
			}
		}

		GlobalDataScript.gamePlayPanel =  PrefabManage.loadPerfab ("Prefab/Panel_GamePlay(Clone)");
		removeListener ();
		Destroy (this);
		Destroy (gameObject);
	
	}


	private void resetClickNum(){
		tapCount = 0;
	}


}
