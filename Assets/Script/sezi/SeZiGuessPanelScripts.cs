using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using LitJson;
using AssemblyCSharp;

public class SeZiGuessPanelScripts : MonoBehaviour {


	public Button yes_button;
	public Button no_button;
    public Button close_button;

    public Image zhai_img;
    public Image point_img;
    public Text num_txt;
    public Text ge_txt;
    public Text totalMoney_txt;
    public Text daojishi_txt;
    public GameObject dizhu_content;


	public Text yes_text;
	public Text no_text;
	private int _jcPoint;
	private int daojishi;
	private double daojishiStartTime;
	private bool toupiao;

	// Use this for initialization
	void Start () {
		toupiao = false;
		zhai_img.transform.gameObject.SetActive (false);
		if (GlobalDataScript.isGoldQuickStar|| GlobalDataScript.roomVo.isGoldRoom) {
			//金币场
			daojishi_txt.text = "10";
			daojishi = 10;
		} else {
			daojishi_txt.text = "";
			daojishi = 0;
		}

		if (_jcPoint > 0) {
			if (_jcPoint == 1) {
				zhai_img.gameObject.SetActive (true);
				zhai_img.sprite = Resources.Load ("sizi/jingcai/zhai",typeof(Sprite)) as Sprite;
			}
			point_img.sprite = Resources.Load("sizi/size_scene/point/point_" + _jcPoint, typeof(Sprite)) as Sprite;
        }
	}

	public void setJingCaiNum(int point,int num1, int totalMoney)
	{
		_jcPoint = point;
		ge_txt.text = "其余人均" + num1 + "个";
        if (totalMoney > 0)
        {
            totalMoney_txt.gameObject.SetActive(true);
        }
        totalMoney_txt.text = totalMoney.ToString(); ;
    }

    private int yesNum;
    private int noNum;
	public void addJingCaiNum(int yes,int num)
	{
		if (yes == 1) {
            yesNum++;
            yes_text.text = yesNum.ToString ();
		} else {
            noNum++;
			no_text.text = noNum.ToString ();
		}
	}

	public void onYesClick() {
		startTouPiao (1);

        destorySelf();
    }

	public void onNoClick() {
		startTouPiao (0);
        destorySelf();
	}

    public void destorySelf()
    {
		if (toupiao == false && daojishi > 0) {
			//倒计时结束后，没有参与投票就不参加
			startTouPiao (0);
		}
		daojishi = 0;
        Destroy(this);
        Destroy(gameObject);
    }

	private void startTouPiao(int result) {

		SoundCtrl.getInstance ().playSoundByAction ("ding" );

		toupiao = true;
		daojishi = 0;
		SZGuessRequestVO vo = new SZGuessRequestVO();
		vo.yes = result;
		string sendMsg = JsonMapper.ToJson(vo);
		CustomSocket.getInstance().sendMsg(new SZGuessRequest(sendMsg));
	}

	// Update is called once per frame
	void Update () {
		if (daojishi > 0) {
			double nowTime = GlobalDataScript.getInstance ().getTime ();
			if (nowTime - daojishiStartTime >= 1000) {
				daojishiStartTime = nowTime;
				daojishi--;
				if (daojishi == 0) {
					destorySelf ();
					return;
				}
				daojishiStartTime = nowTime;
				daojishi_txt.text = daojishi + "秒后结束竞猜";
			}
		}
	}
}
