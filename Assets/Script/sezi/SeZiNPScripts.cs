using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using global;

public class SeZiNPScripts : MonoBehaviour {

    public effbiankuang effect_kuang;

	public Button num_button;
	public Text num_txt;
	public Text gailv_txt;
	public Button point_button;
	public GameObject point_img;
	public Button call_grayButton;
	public Button call_normalButton;
	public Text gold_txt;
	public Transform num_content;
	public Transform point_content;
	public GameObject num_scrollView;
	public GameObject point_scrollView;

    public GameObject chatBQObj;
    public GameObject chatNormal;
    public Image faceImg;

    private int showTime = 0;
    private int showTime1 = 0;

    private int myClickPoint;
	private int myClickNum;
	private int playerCallPoint;
	private int playerCallNum;
	public Image kuang_img;
	public Image qk_bei_img;

	public bool canShowNumAndPoint = false;

	// Use this for initialization
	void Start () {
		initUI ();
	}
    
    //显示抢开的倍数图片
    public void showQKBeiImg(int show_bei,bool value)
    {
        qk_bei_img.sprite = Resources.Load("sizi/Battle_0/qk_" + show_bei, typeof(Sprite)) as Sprite;
        qk_bei_img.enabled = value;
        qk_bei_img.transform.gameObject.SetActive(value);
    }

    public void gameEnd()
    {
		playerCallNum = playerCallPoint = myClickNum = myClickPoint = 0;
        gailv_txt.text = "";
		changeToGrayButtonState ();

		showTime = 0;
		if (chatBQObj) {
			chatBQObj.SetActive (false);
			chatNormal.SetActive (false);
		}
    }

	//true显示灰色，false显示亮色
	public void changeToGrayButtonState(bool value = true){
        //print("changeToGrayButtonState=====" + value);
		call_grayButton.transform.gameObject.SetActive(value);
		call_normalButton.transform.gameObject.SetActive(!value);
	}

	private void initUI() {
        effect_kuang.reset(false);
        chatBQObj.SetActive(false);
        chatNormal.SetActive(false);

        qk_bei_img.transform.gameObject.SetActive (false);
		kuang_img.transform.gameObject.SetActive (false);

		for (int i = 2; i < 8; i++) {
			GameObject obj = Instantiate (Resources.Load("Prefab/sezi/SZItemPoint")) as GameObject;
			obj.transform.SetParent (point_content);
			obj.transform.localScale = Vector3.one;
			if (i == 7) {
				//1特殊处理
				obj.GetComponent<SeZiItemPointScript> ().showDDImg ((int)IMG_ENUM.ZHAI, true);
				obj.GetComponent<SeZiItemPointScript> ().setId (1);
			} else {
				obj.GetComponent<SeZiItemPointScript>().setId(i);
			}
			obj.GetComponentInChildren<Button> ().onClick.AddListener (delegate {
				onPointClick(obj);	
			});
		}

		num_scrollView.SetActive (false);
		point_scrollView.SetActive (false);

		changeToGrayButtonState ();
    }

    //初始化数量列表
    public void initNumList()
    {
		int startNum = GlobalDataScript.roomAvatarVoList.Count + 1;
		myClickNum = startNum;
		myClickPoint = 2;

		//call_grayButton.transform.gameObject.SetActive (false);
		//call_normalButton.transform.gameObject.SetActive (true);

		//		myClickNum = num;
		//		myClickPoint = 2;

		//赋值，然后切换筛子的个数图片
		num_txt.text = startNum.ToString ();
		point_img.GetComponent<Image>().sprite = Resources.Load ("sizi/size_scene/point/point_" + startNum, typeof(Sprite)) as Sprite;

        showNumUI(startNum);
    }

    private void showNumUI(int startNum)
    {
        for (int i = 0; i < num_content.childCount; i++)
        {
            Destroy(num_content.GetChild(i).gameObject);
        }

        for (int i = startNum; i < 37; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("Prefab/sezi/SZItemNum")) as GameObject;
            obj.transform.SetParent(num_content);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<SeZiItemNumScript>().setId(i);
            obj.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                onNumClick(obj);
            });
        }
    }

    //更新数组的位置和点数的排序
	public void updateNumPosAndPoint(int put_num,int put_point) {
		
		if (put_num == 0 || put_point == 0)
        {
            return;
        }
		playerCallNum = put_num;
		playerCallPoint = put_point;
        //数字滚动到某个位置
        showNumUI(playerCallNum);

        //开始处理点
        int tuijian_point = 2;
        int[] num_arr = new int[6];

        if (playerCallPoint < 6)
        {
            myClickNum = playerCallNum + 1;
            tuijian_point = playerCallPoint;
        }
        else if (playerCallPoint == 6)
        {
            myClickNum = playerCallNum + 1;
            tuijian_point = playerCallPoint;
        }
        num_txt.text = myClickNum.ToString() ;       
        myClickPoint = tuijian_point;

        int total = tuijian_point + 5;
        int f_index = 0;
        for (int i = tuijian_point; i < total; i++)
        {
            f_index = (i < 6) ? i : (total - i);
            //print("updateNumPosAndPoint====" + f_index);
            GameObject itemObj = point_content.GetChild(f_index).gameObject;
            SeZiItemPointScript itemScript = itemObj.GetComponent<SeZiItemPointScript>() as SeZiItemPointScript;
			itemScript.showDDImg(-1, false);
            if (itemScript.getId() == tuijian_point)
            {
                itemScript.showDDImg((int)IMG_ENUM.TUI, true);
            }
            if (f_index == 5)
            {
                itemScript.showDDImg((int)IMG_ENUM.ZHAI, true);
            }
        }

//		for (int i = 2; i < 8; i++) {
//			GameObject itemObj = point_content.GetChild(f_index).gameObject;
//			SeZiItemPointScript itemScript = itemObj.GetComponent<SeZiItemPointScript>() as SeZiItemPointScript;
//			if (itemScript.getId() == tuijian_point)
//			{
//				itemScript.showDDImg((int)IMG_ENUM.TUI, true);
//			}
//			f_index ++;
//		}

		point_img.GetComponent<Image>().sprite = Resources.Load ("sizi/size_scene/point/point_" + tuijian_point, typeof(Sprite)) as Sprite;

    }

    public void turnMyCall()
    {
		effect_kuang.reset(true);
        canShowNumAndPoint = true;
        kuang_img.gameObject.SetActive(true);
		updateNumPosAndPoint(playerCallNum,playerCallPoint);

		//changeButtonState (false);
    }

	private void onPointClick(GameObject obj) {
		int point = obj.GetComponent<SeZiItemPointScript> ().getId ();
        //SeZiGlobalData.getMe ().myClickPoint = point;
        myClickPoint = point;
		point_img.GetComponent<Image>().sprite = Resources.Load ("sizi/size_scene/point/point_" + point, typeof(Sprite)) as Sprite;
		//myClickPoint = SeZiGlobalData.getMe ().myClickPoint;
	}

	private void onNumClick(GameObject obj) {
		int num = obj.GetComponent<SeZiItemNumScript> ().id;
		num_txt.text = num.ToString ();
		myClickNum = num;
        //SeZiGlobalData.getMe ().myClickNum = num;
		//myClickNum = SeZiGlobalData.getMe ().myClickNum;

	}

	public void onPointButtonClick() {

		if (canShowNumAndPoint == false) {
			return;
		}

		if (point_scrollView.activeSelf == true) {
			point_scrollView.SetActive (false);
		} else {
			num_scrollView.SetActive (false);
			point_scrollView.SetActive (true);
		}

	}

	public void onNumButtonClick() {

		if (canShowNumAndPoint == false) {
			return;
		}

		if (num_scrollView.activeSelf == true) {
			num_scrollView.SetActive (false);
		} else {
			point_scrollView.SetActive (false);
			num_scrollView.SetActive (true);
		}
	}

	public void onCallClick() {

        if (GlobalDataScript.myPointsArr == null || GlobalDataScript.myPointsArr.Length == 0)
        {
            return;
        }

		num_scrollView.SetActive (false);
		point_scrollView.SetActive (false);

		int o_p = playerCallPoint;
		int o_n = playerCallNum;

		if (myClickNum < o_n) {
			TipsManagerScript.getInstance().setTips("您叫的个数不能小于上家的个数");
			return;
		}
		if (myClickNum <= o_n && myClickPoint < o_p) {
			TipsManagerScript.getInstance().setTips("您叫的点数不能小于上家的点数");
			return;
		}
		if (myClickNum == o_n && myClickPoint == o_p) {
			TipsManagerScript.getInstance().setTips("您叫的点数和个数不能和上家的一样");
			return;
		}
		effect_kuang.reset(false);
        this.sendReq();
	}

    public void stopEffect()
    {
        if (effect_kuang)
        {
            effect_kuang.reset(false);
        }
    }

    public void sendReq()
    {
        BetSeZiRequestVo vo = new BetSeZiRequestVo();
        vo.keyNum = myClickNum;
        vo.valueNum = myClickPoint;
        string sendmsgstr = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new PutOutCardRequest(sendmsgstr));

        //print("call pointandnum:" + sendmsgstr);

        //发送消息
        SZRoomChatVO chatVo = new SZRoomChatVO();
        chatVo.id = 3;
        chatVo.msg = myClickNum + "个 " + myClickPoint;
        string sendmsgstr1 = JsonMapper.ToJson(chatVo);
        CustomSocket.getInstance().sendMsg(new SZRoomChatRequest(sendmsgstr1));

        call_grayButton.transform.gameObject.SetActive(true);
        call_normalButton.transform.gameObject.SetActive(false);
    }

    public void showChatMsg(int msgType, string msgContent)
    {
        string newContent = msgContent;
        if (msgType == (int)CHAT_TYPE.TEXT_TYPE)
        {
            showTime = PlayerItemScript.SHOW_CHAT_TIME;
            if (msgContent.Length > 5)
            {
                newContent = msgContent.Substring(0, 5) + "...";
            }
            chatNormal.GetComponent<Text>().text = newContent;
          //  chatNormal.GetComponentInChildren<Text>().text = newContent;
            chatNormal.transform.GetChild(1).GetComponent<Text>().text = newContent;
            chatNormal.SetActive(true);
        }
        else if (msgType == (int)CHAT_TYPE.DUANYU_TYPE)
        {
            showTime = PlayerItemScript.SHOW_CHAT_TIME;
            msgContent = GlobalDataScript.dyChatList[int.Parse(msgContent)];
            SoundCtrl.getInstance().playSoundByAction("quick_chat_" + msgContent);
            chatNormal.GetComponent<Text>().text = newContent;
          //  chatNormal.GetComponentInChildren<Text>().text = newContent;
            chatNormal.transform.GetChild(1).GetComponent<Text>().text = newContent;
            chatNormal.SetActive(true);
        }
        else if (msgType == (int)CHAT_TYPE.CALLPOINT_TYPE)
        {
            showTime1 = PlayerItemScript.SHOW_CHAT_TIME;
            chatBQObj.SetActive(true);
            string[] arr = msgContent.Split(' ');
            if (arr.Length == 2)
            {
                int index = int.Parse(arr[1]);
                faceImg.sprite = Resources.Load("sizi/size_scene/point/point_" + index, typeof(Sprite)) as Sprite;
            }
            else
            {
                faceImg.gameObject.SetActive(false);
            }
            
            chatBQObj.GetComponentInChildren<Text>().text = arr[0];
            
            chatBQObj.SetActive(true);
        }
        else if (msgType == (int)CHAT_TYPE.FACE_TYPE)
        {
            showTime1 = PlayerItemScript.SHOW_CHAT_TIME;
            chatBQObj.GetComponentInChildren<Text>().text = "";
            faceImg.sprite = Resources.Load("sizi/Emoji_0/" + msgContent, typeof(Sprite)) as Sprite;
            chatBQObj.SetActive(true);
		}
		else if (msgType == (int)CHAT_TYPE.CALLKAI_TYPE)
		{
			showTime1 = PlayerItemScript.SHOW_CHAT_TIME;
			chatBQObj.GetComponentInChildren<Text>().text = "";
			faceImg.sprite = Resources.Load("sizi/call_kai", typeof(Sprite)) as Sprite;
			chatBQObj.SetActive(true);
		}
    }
	
	
	// Update is called once per frame
	void Update () {
        if (showTime > 0)
        {
            showTime--;
			if (showTime <= 0)
            {
                chatBQObj.SetActive(false);
            }
        }

        if (showTime1 > 0)
        {
            showTime1--;
            if (showTime1 <= 0)
            {
                chatNormal.SetActive(false);
            }
        }
    }
}
